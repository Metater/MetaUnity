// using Assets.Metater.MetaUtils;
// using UnityEngine;

// namespace Assets.Metater.Components
// {
//     [RequireComponent(typeof(AudioSource))]
//     public class InstancedAudioSource : MonoBehaviour
//     {
//         private const float MarginSeconds = 1;

//         public AudioSource audioSource;

//         private void Awake()
//         {
//             if (audioSource.playOnAwake)
//             {
//                 Debug.LogWarning($"Play on awake is enabled for the instanced audio source \"{gameObject.name}\".");

//                 audioSource.playOnAwake = false;
//                 audioSource.Stop();
//             }

//             float delaySeconds = audioSource.PlayRealistically();
//             Destroy(gameObject, delaySeconds + audioSource.clip.length + MarginSeconds);
//         }

//         public InstancedAudioSource Create(Vector3 position, Quaternion? rotation = null, Transform transform = null)
//         {
//             var instance = Instantiate(this, position, rotation == null ? Quaternion.identity : rotation.Value, transform);
//             return instance;
//         }
//     }
// }
