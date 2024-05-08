using System;
using UnityEngine;

namespace Assets.Metater.MetaVoiceChat
{
    [Serializable]
    public struct VcConfig
    {
        [Header("General")]
        [SerializeField] private MonoBehaviour coroutineProvider; // MetaVc MonoBehaviour
        [SerializeField] private int frequency; // 16000
        [SerializeField] private int segmentDurationMs; // 25

        [Header("Detection")]
        [SerializeField] private float detectionValue; // 0.002
        [SerializeField] private float detectionPercentage; // 0.05
        [SerializeField] private float detectionLatchSeconds; // 0.1

        [Header("Input")]
        [SerializeField] private int inputLoopSeconds; // 1

        [Header("Output")]
        [SerializeField] private int outputSegmentCount; // 40
        [SerializeField] private Vector2Int outputSegmentBufferDeadzone; // new(4, 6)
        [SerializeField] private float outputPitchPercentPerSegmentOfError; // 0.01

        public readonly MonoBehaviour CoroutineProvider => coroutineProvider;
        public readonly int Frequency => frequency;
        public readonly int SegmentDurationMs => segmentDurationMs;
        public readonly int ChannelCount => 1;
        public readonly int SegmentRate => 1000 / SegmentDurationMs;
        public readonly int SegmentLength => Frequency * ChannelCount / SegmentRate;

        public readonly float DetectionValue => detectionValue;
        public readonly float DetectionPercentage => detectionPercentage;
        public readonly float DetectionLatchSeconds => detectionLatchSeconds;

        public readonly int InputLoopSeconds => inputLoopSeconds;

        public readonly int OutputSegmentCount => outputSegmentCount;
        public readonly int OutputSamplesLength => SegmentLength * OutputSegmentCount;
        public readonly Vector2Int OutputSegmentBufferDeadzone => outputSegmentBufferDeadzone;
        public readonly int OutputSegmentBufferAverage => (outputSegmentBufferDeadzone.x + outputSegmentBufferDeadzone.y) / 2;
        public readonly float OutputPitchPercentPerSegmentOfError => outputPitchPercentPerSegmentOfError;
    }
}