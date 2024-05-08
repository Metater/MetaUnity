// https://github.com/adrenak/univoice-audiosource-output/blob/master/Assets/Adrenak.UniVoice.AudioSourceOutput/Runtime/UniVoiceAudioSourceOutput.cs

using System;
using System.Collections;
using UnityEngine;

namespace Assets.Metater.MetaVoiceChat.Output
{
    public class VcAudioOutput : IDisposable
    {
        public readonly VcConfig config;
        public readonly AudioSource audioSource;

        private readonly VcAudioClip vcAudioClip;
        private readonly int[] outputSegmentIndicies;
        private readonly Coroutine updateCoroutine;

        private int greatestSegmentIndex = -1;

        public VcAudioOutput(VcConfig config, AudioSource audioSource)
        {
            this.config = config;
            this.audioSource = audioSource;

            vcAudioClip = new(config);
            outputSegmentIndicies = new int[config.OutputSegmentCount];
            for (int i = 0; i < outputSegmentIndicies.Length; i++)
            {
                outputSegmentIndicies[i] = -1;
            }

            updateCoroutine = config.CoroutineProvider.StartCoroutine(RoutineUpdate());

            audioSource.loop = true;
            audioSource.clip = vcAudioClip.AudioClip;
        }

        private IEnumerator RoutineUpdate()
        {
            while (greatestSegmentIndex == -1)
            {
                yield return null;
            }

            audioSource.Play();

            while (true)
            {
                float latencySeconds = GetLatencySeconds();
                float latencySegments = latencySeconds * config.SegmentRate;
                float errorSegments = config.OutputSegmentBufferAverage - latencySegments;
                if (latencySegments >= config.OutputSegmentBufferDeadzone.x && latencySegments <= config.OutputSegmentBufferDeadzone.y)
                {
                    audioSource.pitch = 1f;
                }
                else
                {
                    errorSegments += Mathf.Sign(errorSegments) * config.OutputPitchPercentPerSegmentOfError;

                    float correction = -errorSegments * config.OutputPitchPercentPerSegmentOfError;
                    correction = Mathf.Clamp(correction, -0.5f, 1f);
                    audioSource.pitch = 1f + correction;
                }

                int offset = vcAudioClip.GetOffset(greatestSegmentIndex);
                int progressOffset = audioSource.timeSamples / config.SegmentLength;
                int expectedSegmentIndex = greatestSegmentIndex;

                int i = offset;
                while (i != progressOffset)
                {
                    int segmentIndex = outputSegmentIndicies[i];
                    if (segmentIndex != -1 && segmentIndex != expectedSegmentIndex)
                    {
                        vcAudioClip.Clear(i);
                        outputSegmentIndicies[i] = -1;
                    }

                    expectedSegmentIndex--;

                    i--;
                    if (i < 0)
                    {
                        i = config.OutputSegmentCount - 1;
                    }
                }

                yield return null;
            }
        }

        private float GetLatencySeconds()
        {
            float writeTime = (float)vcAudioClip.GetOffset(greatestSegmentIndex) * config.SegmentDurationMs / 1000f;
            float playTime = audioSource.time;
            float latency = writeTime - playTime;
            if (latency < 0)
            {
                latency = vcAudioClip.AudioClip.length + latency;
            }

            return latency;
        }

        public void FeedSegment(int segmentIndex, float[] segment = null)
        {
            int offset = vcAudioClip.GetOffset(segmentIndex);
            vcAudioClip.WriteSegment(offset, segment);
            outputSegmentIndicies[offset] = segmentIndex;

            if (segmentIndex > greatestSegmentIndex)
            {
                greatestSegmentIndex = segmentIndex;
            }
        }

        public void Dispose()
        {
            config.CoroutineProvider.StopCoroutine(updateCoroutine);
            vcAudioClip.Dispose();
        }
    }
}