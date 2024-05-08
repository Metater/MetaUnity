using System.Collections.Generic;
using UnityEngine;

namespace Assets.Metater.MetaRefs
{
    public abstract class MetaMb<T> : MonoBehaviour where T : MonoBehaviour
    {
        private readonly static List<T> instances = new();
        public static IReadOnlyList<T> Instances => instances;

        private readonly static List<IMrListener<T>> listeners = new();

        private T This
        {
            get
            {
                T instance = this as T;
                if (instance == null)
                {
                    Error.TypeMismatch(this);
                }

                return instance;
            }
        }

#if UNITY_ASSERTIONS
        public MetaMb()
        {
            Error.IfUsingMethod(nameof(OnEnable), nameof(MetaOnEnable), this);
            Error.IfUsingMethod(nameof(OnDisable), nameof(MetaOnDisable), this);

            if (GetType() != typeof(T))
            {
                Error.TypeMismatch(this);
            }
        }
#endif

        private void OnEnable()
        {
            T instance = This;

            Debug.Assert(!instances.Contains(instance), this);

            instances.Add(instance);

            MetaOnEnable();
            listeners.RemoveAll(l =>
            {
                if (IMrListener<T>.IsDead(l))
                {
                    return true;
                }

                l.OnInstanceEnabled(instance, false);
                return false;
            });
        }
        protected virtual void MetaOnEnable() { }

        private void OnDisable()
        {
            T instance = This;

            Debug.Assert(instances.Contains(instance), this);

            MetaOnDisable();
            listeners.RemoveAll(l =>
            {
                if (IMrListener<T>.IsDead(l))
                {
                    return true;
                }

                l.OnInstanceDisabled(instance, false);
                return false;
            });

            instances.Remove(instance);

            Debug.Assert(!instances.Contains(instance), this);
        }
        protected virtual void MetaOnDisable() { }

        public static void Subscribe(IMrListener<T> listener)
        {
            if (IMrListener<T>.IsDead(listener))
            {
                return;
            }

            listeners.RemoveAll(l => IMrListener<T>.IsDead(l));

            if (!listeners.Contains(listener))
            {
                listeners.Add(listener);

                foreach (var instance in instances)
                {
                    listener.OnInstanceEnabled(instance, false);
                }
            }
        }
    }
}