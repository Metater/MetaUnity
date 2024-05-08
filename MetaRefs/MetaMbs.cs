using System.Collections.Generic;
using UnityEngine;

namespace Assets.Metater.MetaRefs
{
    public abstract class MetaMbs<T> : MonoBehaviour where T : MonoBehaviour
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
        public MetaMbs()
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
            if (instance == null)
            {
                instance = This;
            }
            else if (instance != This)
            {
                Error.MultipleInstances(this);
            }

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
            Debug.Assert(instance == this);

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

            instance = null;
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

                if (HasInstance)
                {
                    listener.OnInstanceEnabled(instance, false);
                }
            }
        }
    }
}