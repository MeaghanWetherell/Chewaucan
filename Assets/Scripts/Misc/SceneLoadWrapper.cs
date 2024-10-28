using System;
using System.Collections;
using System.Collections.Generic;
using Audio;
using ScriptTags;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace Misc
{
    public class SceneLoadWrapper : MonoBehaviour
    {
        public static SceneLoadWrapper sceneLoadWrapper;

        public int currentSceneType = 0;
        
        [Tooltip("List of scenes in modern map")] public List<String> modernMapScenes;

        [Tooltip("List of scenes in pleistocene map")] public List<String> pleistoceneMapScenes;

        public readonly UnityEvent OnLoadScene = new UnityEvent();

        private void Awake()
        {
            StartCoroutine(fadeInOnStart());
            if (sceneLoadWrapper != null)
            {
                Destroy(sceneLoadWrapper.gameObject);
            }
            sceneLoadWrapper = this;
            DontDestroyOnLoad(this.gameObject);
        }

        private IEnumerator fadeInOnStart()
        {
            while (GameObject.Find("Fader") == null) yield return new WaitForSeconds(0);
            GameObject fader = GameObject.Find("Fader");
            FadeFromBlack fadeActual;
            if (fader != null)
            {
                fadeActual = fader.GetComponent<FadeFromBlack>();
                if (fadeActual != null)
                {
                    fadeActual.FadeIn(6);
                }
            }
        }

        private void OnEnable()
        {
            SceneManager.sceneLoaded += FadeOnSceneLoad;
        }

        private void OnDisable()
        {
            SceneManager.sceneLoaded -= FadeOnSceneLoad;
        }

        public void LoadScene(String sceneName)
        {
            if (PauseCallback.pauseManager.isPaused)
            {
                PauseCallback.pauseManager.Resume();
            }
            OnLoadScene.Invoke();
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

        private void FadeOnSceneLoad(Scene scene, LoadSceneMode mode)
        {
            if (mode != LoadSceneMode.Single) return;
            GameObject fader = GameObject.Find("Fader");
            FadeFromBlack fadeActual;
            if (fader != null)
            {
                fadeActual = fader.GetComponent<FadeFromBlack>();
                if (fadeActual != null)
                {
                    fadeActual.FadeIn(6);
                }
            }
        }
    }
}
