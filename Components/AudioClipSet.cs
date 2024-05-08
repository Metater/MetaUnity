using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Assets.Metater.Components
{
    public class AudioClipSet : MonoBehaviour
    {
        public AudioSource audioSource;
        public List<AudioClip> clips;

        private AudioClip RandomAudioClip => clips[Random.Range(0, clips.Count)];
        private int nextIndex = 0;

        public void PlayRandom(float volumeScale)
        {
            if (clips.Count == 0)
            {
                Debug.LogWarning($"No audio clips on gameobject with name \"{gameObject.name}\".", gameObject);
                return;
            }

            audioSource.PlayOneShot(RandomAudioClip, volumeScale);
        }

        public void PlayLerp(float t, float volumeScale)
        {
            if (clips.Count == 0)
            {
                Debug.LogWarning($"No audio clips on gameobject with name \"{gameObject.name}\".", gameObject);
                return;
            }

            int i = Mathf.RoundToInt(clips.Count * t);
            i = Math.Clamp(i, 0, clips.Count);
            audioSource.PlayOneShot(clips[i], volumeScale);
        }

        public void PlayCyclical(float volumeScale)
        {
            if (clips.Count == 0)
            {
                Debug.LogWarning($"No audio clips on gameobject with name \"{gameObject.name}\".", gameObject);
                return;
            }

            int i = nextIndex++;
            if (i >= clips.Count)
            {
                i = 0;
                nextIndex = 1;
            }

            audioSource.PlayOneShot(clips[i], volumeScale);
        }
    }
}
