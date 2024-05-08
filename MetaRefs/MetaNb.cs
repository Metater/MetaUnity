using System.Collections.Generic;
using Mirror;
using UnityEngine;

namespace Assets.Metater.MetaRefs
{
    public abstract class MetaNb<T> : NetworkBehaviour where T : NetworkBehaviour
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
        public MetaNb()
        {
            Error.IfUsingMethod(nameof(OnStartClient), nameof(MetaOnStartClient), this);
            Error.IfUsingMethod(nameof(OnStopClient), nameof(MetaOnStopClient), this);

            if (GetType() != typeof(T))
            {
                Error.TypeMismatch(this);
            }
        }
#endif

        public override void OnStartClient()
        {
            T instance = This;

            Debug.Assert(!instances.Contains(instance), this);

            instances.Add(instance);

            MetaOnStartClient();
            listeners.RemoveAll(l =>
            {
                if (IMrListener<T>.IsDead(l))
                {
                    return true;
                }

                l.OnInstanceEnabled(instance, isLocalPlayer);
                return false;
            });
        }
        protected virtual void MetaOnStartClient() { }

        public override void OnStopClient()
        {
            T instance = This;

            Debug.Assert(instances.Contains(instance), this);

            MetaOnStopClient();
            listeners.RemoveAll(l =>
            {
                if (IMrListener<T>.IsDead(l))
                {
                    return true;
                }

                l.OnInstanceDisabled(instance, isLocalPlayer);
                return false;
            });

            instances.Remove(instance);

            Debug.Assert(!instances.Contains(instance), this);
        }
        protected virtual void MetaOnStopClient() { }

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
                    listener.OnInstanceEnabled(instance, instance.isLocalPlayer);
                }
            }
        }
    }
}