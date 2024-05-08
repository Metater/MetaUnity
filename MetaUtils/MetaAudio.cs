using UnityEngine;

namespace Assets.Metater.MetaUtils
{
    public static class MetaAudio
    {
        /// <summary>
        /// This assumes the AudioSource and AudioListener have low relative velocities
        /// </summary>
        public static float PlayRealistically(this AudioSource audioSource, float speedOfSound = 343)
        {
            float delaySeconds = GetDelaySeconds(audioSource.transform.position, speedOfSound);
            audioSource.PlayDelayed(delaySeconds);
            return delaySeconds;
        }

        /// <summary>
        /// This assumes the AudioSource and AudioListener have low relative velocities
        /// </summary>
        public static float GetDelaySeconds(Vector3 sourcePosition, float speedOfSound = 343)
        {
            var listener = MetaCache.Object<AudioListener>();
            float distance = Vector3.Distance(sourcePosition, listener.transform.position);
            return distance / speedOfSound;
        }
    }
}