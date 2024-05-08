// https://github.com/adrenak/univoice-audiosource-output/blob/master/Assets/Adrenak.UniVoice.AudioSourceOutput/Runtime/CircularAudioClip.cs

using System;
using UnityEngine;

namespace Assets.Metater.MetaVoiceChat.Output
{
    public class VcAudioClip : IDisposable
    {
        public readonly VcConfig config;

        public AudioClip AudioClip { get; private set; }

        private readonly float[] emptySegment;
        private readonly float[] emptyData;

        public VcAudioClip(VcConfig config)
        {
            this.config = config;
            AudioClip = AudioClip.Create(nameof(VcAudioClip), config.OutputSamplesLength, config.ChannelCount, config.Frequency, false);

            emptySegment = new float[config.SegmentLength];
            emptyData = new float[config.OutputSamplesLength];
        }

        public void WriteSegment(int offset, float[] segment)
        {
            segment ??= emptySegment;

            if (segment.Length != config.SegmentLength)
            {
                throw new Exception("Voice chat audio clip segment length does not match the config!");
            }

            AudioClip.SetData(segment, config.SegmentLength * offset);
        }

        public int GetOffset(int segmentIndex)
        {
            return segmentIndex % config.OutputSegmentCount;
        }

        public void Clear(int offset)
        {
            AudioClip.SetData(emptySegment, config.SegmentLength * offset);
        }

        public void Clear()
        {
            AudioClip.SetData(emptyData, 0);
        }

        public void Dispose()
        {
            UnityEngine.Object.Destroy(AudioClip);
            AudioClip = null;
        }
    }
}