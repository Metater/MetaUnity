using System;
using UnityEngine;
using Assets.Metater.UnityEditor;

namespace Assets.Metater.MetaUtils
{
    [Serializable]
    public struct MetaTimeoutValue<T>
    {
        [SerializeField] private float timeoutSeconds;

#if UNITY_EDITOR
        [ReadOnly]
#endif
        [SerializeField] private T value;

        private MetaInstant instant;

        public void Set(T value, MetaInstant instant)
        {
            this.value = value;
            this.instant = instant;
        }

        public T Get()
        {
            if (!instant.IsInitialized)
            {
                value = default;
                return default;
            }

            if (instant.ElapsedSecondsF < timeoutSeconds)
            {
                return value;
            }

            value = default;
            return default;
        }
    }
}