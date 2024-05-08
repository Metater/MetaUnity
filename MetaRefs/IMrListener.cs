using UnityEngine;

namespace Assets.Metater.MetaRefs
{
    public interface IMrListener<T> where T : MonoBehaviour
    {
        public virtual void OnInstanceEnabled(T instance, bool isLocalPlayer) { }
        public virtual void OnInstanceDisabled(T instance, bool isLocalPlayer) { }

        public static bool IsDead(IMrListener<T> listener)
        {
            return listener as Object == null;
        }
    }
}
