using System.Collections;
using ScriptTags;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Misc
{
    public class MainSceneDataSaver : MonoBehaviour
    {
        public static MainSceneDataSaver mainSceneDataSaver;

        private Vector3 _lastPlayerPosition;

        private Quaternion _lastPlayerRotation;

        public LoadGUI curMenu;

        private void Awake()
        {
            if (mainSceneDataSaver != null)
            {
                Debug.LogError("Loaded persistent objects twice!");
                Destroy(mainSceneDataSaver.gameObject);
            }
            mainSceneDataSaver = this;
            DontDestroyOnLoad(this.gameObject);
        }

        public void PrepareForUnload()
        {
            if (Player.player == null)
                return;
            Transform player = Player.player.transform;
            if (player != null)
            {
                _lastPlayerPosition = player.position;
                _lastPlayerRotation = player.rotation;
            }
        }

        public void ReloadMainScene()
        {
            BGMManager.bgmManager.StopBGM();
            SceneManager.LoadScene(5);
            SceneManager.sceneLoaded += ONReload;
        }

        private void ONReload(Scene activeScene, LoadSceneMode loadSceneMode)
        {
            if (activeScene.name.Equals("Modern Map"))
            {
                Player.player.transform.SetPositionAndRotation(_lastPlayerPosition, _lastPlayerRotation);
            }
            
        }
    }
}
