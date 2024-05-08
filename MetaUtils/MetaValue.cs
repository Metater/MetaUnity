// Serialized value that has events for value changes
// Allows for preconfiguration and runtime editing and viewing of this value

// May want:
// Make an add function with just an action for an event that never intends to unsubscribe, maybe?????

using System;
using System.Collections.Generic;
using Assets.Metater.UnityEditor;
using UnityEngine;

namespace Assets.Metater.MetaUtils
{
    [Serializable]
    public class MetaValue<T> : ISerializationCallbackReceiver where T : IEquatable<T>
    {
        private readonly List<Subscription> subscriptions = new();

        [SerializeField] private T serializedValue;

#if UNITY_EDITOR
        [ReadOnly]
#endif
        [SerializeField] private T value = default;
        public T Value
        {
            get
            {
                return value;
            }
            set
            {
                Set(value);
            }
        }

        public void AddListener(object subscriber, Action<T> action)
        {
            RemoveListener(subscriber);
            RemoveListener(action);

            Subscription subscription = new(subscriber, action);
            if (subscription.TryInvoke(value))
            {
                subscriptions.Add(subscription);
            }
        }

        public void RemoveListener(object subscriber)
        {
            subscriptions.RemoveAll(s => s.subscriber == subscriber || s.IsInactive);
        }

        public void RemoveListener(Action<T> action)
        {
            subscriptions.RemoveAll(s => s.action == action || s.IsInactive);
        }

        public void OnBeforeSerialize()
        {
            Value = serializedValue;
        }

        public void OnAfterDeserialize()
        {
            Value = serializedValue;
        }

        private void Set(T value)
        {
            if (value == null)
            {
                if (this.value != null)
                {
                    subscriptions.RemoveAll(s => !s.TryInvoke(value));
                }
                else
                {
                    subscriptions.RemoveAll(s => s.IsInactive);
                }
            }
            else if (!value.Equals(this.value))
            {
                subscriptions.RemoveAll(s => !s.TryInvoke(value));
            }
            else
            {
                subscriptions.RemoveAll(s => s.IsInactive);
            }

            this.value = value;
            serializedValue = value;
        }

        public static implicit operator T(MetaValue<T> value) => value.value;

        private readonly struct Subscription
        {
            public readonly object subscriber;
            public readonly Action<T> action;

            public readonly bool IsInactive => subscriber == null || action == null;

            public Subscription(object subscriber, Action<T> action)
            {
                this.subscriber = subscriber;
                this.action = action;
            }

            public bool TryInvoke(T value)
            {
                if (IsInactive)
                {
                    return false;
                }

                action(value);
                return true;
            }
        }
    }
}