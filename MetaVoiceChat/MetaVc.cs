#define ECHO
// ^^^ Use this to echo the local voice.

// This is a derivative work of the following projects, which were created by Vatsal Ambastha:
// https://github.com/adrenak/univoice
// https://github.com/adrenak/univoice-unimic-input
// https://github.com/adrenak/unimic
// https://github.com/adrenak/univoice-audiosource-output

// This was created by Connor (Metater):
// https://github.com/Metater

// Take the following steps to adapt this to your own project:
// 1. Ensure you have the MetaRoutines project accessible to this.
// 2. Ensure you have the MetaRefs project accessible to this.
// 3. Ensure you have the MetaUtils project accessible to this.

// Known issues:
// 1:
// High contigious packet loss and/or a momentary high jitter causes
// Need to catch up (squeaky voice) or slowdown because latency passed zero then went to the clip length
// Solutions:
// 1: Pause audio source until latency passes zero going from large number limit to zero.
// Probably want to prevent exact overlap of playing time and writing time, dont want to set a segment that is currently being played
// Mirror does this:
/*
    [Tooltip("Local timeline acceleration in % while catching up.")]
    [Range(0, 1)]
    public double catchupSpeed = 0.02f; // see snap interp demo. 1% is too slow.

    [Tooltip("Local timeline slowdown in % while slowing down.")]
    [Range(0, 1)]
    public double slowdownSpeed = 0.04f; // slow down a little faster so we don't encounter empty buffer (= jitter)
*/

// Missing functionality:
// 1:
// Spacial audio controls, i.e. muting of a player's mouth after a certain distance
// 2:
// Audio encoding, probably Opus
// 3:
// Support for only single channel microphones.
// Unity's Microphone class may be limited to only one, not sure

using Assets.Metater.MetaRefs;
using Assets.Metater.MetaUtils;
using Assets.Metater.MetaVoiceChat.Input;
using Assets.Metater.MetaVoiceChat.Output;
using Mirror;
using UnityEngine;

namespace Assets.Metater.MetaVoiceChat
{
    public class MetaVc : MetaNbl<MetaVc>
    {
        [Header("General")]
        public VcConfig config;
        public AudioSource outputAudioSource;

        [Header("Values")]
        public MetaValue<bool> isDeafened;
        public MetaValue<bool> isInputMuted;
        public MetaValue<bool> isOutputMuted;
        public MetaValue<bool> isSpeaking;

        private VcAudioInput audioInput;
        private VcAudioOutput audioOutput;

        private void Awake()
        {
            VcSegment.Registry.SetSegmentLength(config.SegmentLength);
        }

        protected override void MetaOnStartClient()
        {
            if (isLocalPlayer)
            {
                audioInput = new(config);
                audioInput.OnSegmentReady += SendAudioSegment;
            }
#if !ECHO
            else
#endif
            {
                audioOutput = new(config, outputAudioSource);
            }
        }

        private void SendAudioSegment(int segmentIndex, float[] segment)
        {
            bool shouldSendEmpty = segment == null || isDeafened || isInputMuted;
            isSpeaking.Value = !shouldSendEmpty;

            if (isServer)
            {
                if (shouldSendEmpty)
                {
                    RpcReceiveEmptyAudioSegment(segmentIndex);
                }
                else
                {
                    RpcReceiveAudioSegment(segmentIndex, new(netId, segmentIndex, segment));
                }
            }
            else
            {
                if (shouldSendEmpty)
                {
                    CmdRelayEmptyAudioSegment(segmentIndex);
                }
                else
                {
                    CmdRelayAudioSegment(segmentIndex, new(netId, segmentIndex, segment));
                }
            }
        }

        [Command(channel = Channels.Unreliable)]
        private void CmdRelayAudioSegment(int segmentIndex, VcSegment segment)
        {
            RpcReceiveAudioSegment(segmentIndex, segment);
        }

        [Command(channel = Channels.Unreliable)]
        private void CmdRelayEmptyAudioSegment(int segmentIndex)
        {
            RpcReceiveEmptyAudioSegment(segmentIndex);
        }

#if ECHO
        [ClientRpc(channel = Channels.Unreliable, includeOwner = true)]
#else
        [ClientRpc(channel = Channels.Unreliable, includeOwner = false)]
#endif
        private void RpcReceiveAudioSegment(int segmentIndex, VcSegment segment)
        {
            if (LocalPlayerInstance.isDeafened || isOutputMuted)
            {
                isSpeaking.Value = false;
                audioOutput.FeedSegment(segmentIndex);
            }
            else
            {
                isSpeaking.Value = true;
                audioOutput.FeedSegment(segmentIndex, segment.UncompressedBuffer);
            }
        }

#if ECHO
        [ClientRpc(channel = Channels.Unreliable, includeOwner = true)]
#else
        [ClientRpc(channel = Channels.Unreliable, includeOwner = false)]
#endif
        private void RpcReceiveEmptyAudioSegment(int segmentIndex)
        {
            isSpeaking.Value = false;
            audioOutput.FeedSegment(segmentIndex);
        }

        protected override void MetaOnStopClient()
        {
            if (audioInput != null)
            {
                audioInput.OnSegmentReady -= SendAudioSegment;
                audioInput.Dispose();
            }

            audioOutput?.Dispose();
        }
    }
}