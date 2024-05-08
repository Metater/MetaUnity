using Assets.Metater.MetaUtils;
using UnityEditor;
using UnityEngine;

// https://forum.unity.com/threads/a-small-script-that-attaches-an-audiolistener-to-the-editor-scene-view-camera.539774/

namespace Assets.Metater.UnityEditor
{
    public class SceneViewFreecam : MonoBehaviour
    {
#if UNITY_EDITOR
        public bool isEnabled;
        public KeyCode toggleKeyCode;
        public MetaTimeoutValue<bool> justEnabled;

        private void Awake()
        {
            if (isEnabled)
            {
                justEnabled.Set(true, MetaInstant.Realtime);
            }
        }

        private void Update()
        {
            if (Input.GetKeyDown(toggleKeyCode))
            {
                isEnabled = !isEnabled;

                if (isEnabled)
                {
                    justEnabled.Set(true, MetaInstant.Realtime);
                }
                else
                {
                    var cameraTransform = Camera.main.transform;
                    cameraTransform.SetPositionAndRotation(Vector3.zero, Quaternion.identity);
                }
            }
        }

        private void LateUpdate()
        {
            // Camera.current && EditorWindow.focusedWindow is SceneView
            if (isEnabled && SceneView.lastActiveSceneView != null)
            {
                SceneView.lastActiveSceneView.camera.transform.GetPositionAndRotation(out var position, out var rotation);

                var cameraTransform = Camera.main.transform;
                cameraTransform.SetPositionAndRotation(position, rotation);
            }

            AudioListener.pause = justEnabled.Get();
        }

        private void OnGUI()
        {
            if (!isEnabled)
            {
                return;
            }

            // GUILayout.Label($"Freecam Enabled ({toggleKeyCode})");
        }
#endif
    }
}