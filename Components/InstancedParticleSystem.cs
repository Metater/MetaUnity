using UnityEngine;

namespace Assets.Metater.Components
{
    public class InstancedParticleSystem : MonoBehaviour
    {
        public ParticleSystem vfx;

        private void Update()
        {
            if (!vfx.isPlaying)
            {
                Destroy(gameObject);
            }
        }

        public InstancedParticleSystem Create(Vector3 position, Quaternion? rotation = null, Transform transform = null)
        {
            var instance = Instantiate(this, position, rotation == null ? Quaternion.identity : rotation.Value, transform);
            return instance;
        }
    }
}