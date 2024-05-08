using System.Collections.Generic;
using Mirror;
using UnityEngine;

namespace Assets.Metater.MetaRefs
{
    public abstract class MetaNbs<T> : NetworkBehaviour where T : NetworkBehaviour
    {
        private static T instance;
        public static T Instance
        {
            get
            {
                if (instance == null)
                {
                    Error.NullInstance();
                }

                return instance;
            }
        }
        public static T NullableInstance => instance;
        public static bool HasInstance => instance != null;

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
        public MetaNbs()
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
            if (instance == null)
            {
                instance = This;
            }
            else if (instance != This)
            {
                Error.MultipleInstances(this);
            }

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
            Debug.Assert(instance == this);

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

            instance = null;
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

                if (HasInstance)
                {
                    listener.OnInstanceEnabled(instance, instance.isLocalPlayer);
                }
            }
        }
    }
}