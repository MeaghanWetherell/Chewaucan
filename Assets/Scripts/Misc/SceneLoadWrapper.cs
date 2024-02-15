using System;
using System.Collections.Generic;
using Audio;
using ScriptTags;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Misc
{
    public class SceneLoadWrapper : MonoBehaviour
    {
        public static SceneLoadWrapper sceneLoadWrapper;

        public int currentSceneType = 0;
        
        [Tooltip("List of scenes in modern map")] public List<String> modernMapScenes;

        [Tooltip("List of scenes in pleistocene map")] public List<String> pleistoceneMapScenes;

        public void Awake()
        {
            if (sceneLoadWrapper != null)
            {
                Debug.LogError("Loaded persistent objects twice!");
                Destroy(sceneLoadWrapper.gameObject);
            }
            sceneLoadWrapper = this;
            DontDestroyOnLoad(this.gameObject);
        }

        public void LoadScene(String sceneName)
        {
            GameObject player = GameObject.FindWithTag("Player");
            if (player != null)
            {
                if (modernMapScenes.Contains(SceneManager.GetActiveScene().name))
                {
                    PlayerPositionManager.playerPositionManager.setPlayerPosition(player.transform.position, 0);
                }
                else
                {
                    PlayerPositionManager.playerPositionManager.setPlayerPosition(player.transform.position, 1);
                }
            }
            SoundManager.soundManager.StopBGM();
            if (modernMapScenes.Contains(sceneName))
            {
                currentSceneType = 0;
            }
            else
            {
                currentSceneType = 1;
            }
            SceneManager.LoadScene(sceneName);
        }
    }
}
