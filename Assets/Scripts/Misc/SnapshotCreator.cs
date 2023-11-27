using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.Windows;

namespace Misc
{
    public class SnapshotCreator : EditorWindow
    {
        private Camera _snapCam;
        private RenderTexture _fromTexture;
        public string filename;
        public string path;

        private SerializedObject _so;
        private Vector3 _initPosition;
        private Quaternion _initRotation;

        [MenuItem("Tools/SnapshotCreator")]
        public static void ShowWindow()
        {
            GetWindow<SnapshotCreator>("Snapshot View");
        }

        private void OnEnable()
        {
            _so = new SerializedObject(this);
        }

        private void OnGUI()
        {
            EditorGUILayout.PropertyField(_so.FindProperty("filename"));
            EditorGUILayout.PropertyField(_so.FindProperty("path"));
            _so.ApplyModifiedProperties();
            GameObject temp = GameObject.Find("SnapshotCamera");
            if (temp == null || temp.GetComponent<Camera>() == null)
            {
                Debug.LogWarning("Couldn't find camera with name SnapshotCamera");
                return;
            }
            _snapCam = temp.GetComponent<Camera>();
            var snapTrans = _snapCam.transform;
            _initPosition = snapTrans.position;
            _initRotation = snapTrans.rotation;
            _fromTexture = _snapCam.targetTexture;
            if (GUILayout.Button("Take Snapshot With Editor Cam Alignment"))
            {
                Camera editorCam = SceneView.lastActiveSceneView.camera;
                var camTrans = editorCam.transform;
                _snapCam.transform.SetPositionAndRotation(camTrans.position, camTrans.rotation);
                SaveText();
                _snapCam.transform.SetPositionAndRotation(_initPosition, _initRotation);
            }
            if (GUILayout.Button("Take Snapshot"))
            {
                SaveText();
            }
        }

        private void SaveText()
        {
            _snapCam.Render();
            Texture2D newText = ToTexture2D(_fromTexture);
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

        private Texture2D ToTexture2D(RenderTexture from)
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
