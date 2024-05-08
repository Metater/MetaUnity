using UnityEngine;
using UnityEditor;

// https://forum.unity.com/threads/read-only-fields.68976/#post-2729947

#if UNITY_EDITOR
namespace Assets.Metater.UnityEditor
{
    [CustomPropertyDrawer(typeof(ReadOnlyAttribute))]
    public class ReadOnlyDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            GUI.enabled = false;
            EditorGUI.PropertyField(position, property, label, true);
            GUI.enabled = true;
        }
    }
}
#endif