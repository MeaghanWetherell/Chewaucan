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

        public bool isLoading = false;

        private string loadScene = "";
        
        [Tooltip("List of scenes in modern map")] public List<String> modernMapScenes;

        [Tooltip("List of scenes in pleistocene map")] public List<String> pleistoceneMapScenes;

        public readonly UnityEvent OnLoadScene = new UnityEvent();

        private void Awake()
        {
            if (sceneLoadWrapper != null)
            {
                Destroy(gameObject);
                return;
            }
            StartCoroutine(fadeInOnStart());
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
            SceneManager.sceneLoaded += registerLoad;
        }

        private void OnDisable()
        {
            SceneManager.sceneLoaded -= FadeOnSceneLoad;
            SceneManager.sceneLoaded -= registerLoad;
        }

        private void registerLoad(Scene scene, LoadSceneMode mode)
        {
            if (scene.name.Equals(loadScene))
            {
                StartCoroutine(regLoadActual());
            }
        }

        private IEnumerator regLoadActual()
        {
            yield return new WaitForSeconds(0);
            isLoading = false;
            loadScene = "";
        }

        public void LoadScene(String sceneName)
        {
            if (PauseCallback.pauseManager.isPaused)
            {
                PauseCallback.pauseManager.Resume();
            }
            OnLoadScene.Invoke();
            SoundManager.soundManager.StopBGM();
            if (modernMapScenes.Contains(sceneName))
            {
                currentSceneType = 0;
            }
            else if(pleistoceneMapScenes.Contains(sceneName))
            {
                currentSceneType = 1;
            }
            isLoading = true;
            loadScene = sceneName;
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
