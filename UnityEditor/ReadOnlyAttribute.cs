using UnityEngine;

// https://forum.unity.com/threads/read-only-fields.68976/#post-2729947

#if UNITY_EDITOR
namespace Assets.Metater.UnityEditor
{
    public class ReadOnlyAttribute : PropertyAttribute { }
}
#endif