using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.Windows;

namespace Misc
{
    public class SnapshotCreator : EditorWindow
    {
        private Camera snapCam;
        private RenderTexture fromTexture;
        public string filename;
        public string path;

        private SerializedObject so;
        private Vector3 initPosition;
        private Quaternion initRotation;

        [MenuItem("Tools/SnapshotCreator")]
        public static void ShowWindow()
        {
            GetWindow<SnapshotCreator>("Snapshot View");
        }

        private void OnEnable()
        {
            so = new SerializedObject(this);
        }

        private void OnGUI()
        {
            EditorGUILayout.PropertyField(so.FindProperty("filename"));
            EditorGUILayout.PropertyField(so.FindProperty("path"));
            so.ApplyModifiedProperties();
            GameObject temp = GameObject.Find("SnapshotCamera");
            if (temp == null || temp.GetComponent<Camera>() == null)
            {
                Debug.LogWarning("Couldn't find camera with name SnapshotCamera");
                return;
            }
            snapCam = temp.GetComponent<Camera>();
            var snapTrans = snapCam.transform;
            initPosition = snapTrans.position;
            initRotation = snapTrans.rotation;
            fromTexture = snapCam.targetTexture;
            if (GUILayout.Button("Take Snapshot With Editor Cam Alignment"))
            {
                Camera editorCam = SceneView.lastActiveSceneView.camera;
                var camTrans = editorCam.transform;
                snapCam.transform.SetPositionAndRotation(camTrans.position, camTrans.rotation);
                saveText();
                snapCam.transform.SetPositionAndRotation(initPosition, initRotation);
            }
            if (GUILayout.Button("Take Snapshot"))
            {
                saveText();
            }
        }

        private void saveText()
        {
            snapCam.Render();
            Texture2D newText = toTexture2D(fromTexture);
            byte[] imgData = newText.EncodeToPNG();
            if (string.IsNullOrEmpty(path))
            {
                File.WriteAllBytes($"{Application.dataPath}/Scripts/Match3/Resources/SnapTextures/{filename}.png", imgData);
            }
            else
            {
                File.WriteAllBytes($"{Application.dataPath}/{path}/{filename}.png", imgData);
            }
        }

        private Texture2D toTexture2D(RenderTexture from)
        {
            Texture2D retval = new Texture2D(from.width, from.height);
            RenderTexture.active = from;
            retval.ReadPixels(new Rect(0, 0, from.width, from.height), 0, 0);
            retval.Apply();
            RenderTexture.active = null;
            return retval;
        }
    }
}
