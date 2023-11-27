using System.Collections;
using ScriptTags;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Misc
{
    public class MainSceneDataSaver : MonoBehaviour
    {
        public static MainSceneDataSaver mainSceneDataSaver;

        private Vector3 lastPlayerPosition;

        private Quaternion lastPlayerRotation;

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

        public void prepareForUnload()
        {
            Transform player = Player.player.transform;
            if (player != null)
            {
                lastPlayerPosition = player.position;
                lastPlayerRotation = player.rotation;
            }
        }

        public void reloadMainScene()
        {
            SceneManager.LoadScene(5);
            SceneManager.sceneLoaded += onReload;
        }

        private void onReload(Scene activeScene, LoadSceneMode loadSceneMode)
        {
            if (activeScene.name.Equals("Modern Map"))
            {
                Player.player.transform.SetPositionAndRotation(lastPlayerPosition, lastPlayerRotation);
            }
            
        }
    }
}
