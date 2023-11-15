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
            lastPlayerPosition = player.position;
            lastPlayerRotation = player.rotation;
        }

        public void reloadMainScene(bool reloadMenu = false)
        {
            StartCoroutine(reloadSceneCoroutine(reloadMenu));
        }

        private IEnumerator reloadSceneCoroutine(bool reloadMenu)
        {
            SceneManager.LoadScene(5);
            while (Player.player != null)
            {
                yield return null;
            }
            Player.player.transform.SetPositionAndRotation(lastPlayerPosition, lastPlayerRotation);
            if (reloadMenu)
            {
                curMenu.onOpenTrigger();
            }
        }
    }
}
