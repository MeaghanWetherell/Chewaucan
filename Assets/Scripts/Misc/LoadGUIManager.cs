using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Misc
{
    public class LoadGUIManager : MonoBehaviour
    {
        public static LoadGUIManager loadGUIManager;
        
        private List<String> openGUIs = new List<string>();
        
        //store a ref to the HUD object
        private GameObject _hud;

        private List<string> doNotUnload = new List<string>();

        private void Awake()
        {
            if (loadGUIManager != null)
            {
                Debug.LogError("Loaded persistent objects twice!");
                Destroy(loadGUIManager.gameObject);
            }
            loadGUIManager = this;
            DontDestroyOnLoad(this.gameObject);
        }
        
        private void OnEnable()
        {
            SceneManager.sceneLoaded += OnSceneLoad;
        }

        private void OnDisable()
        {
            SceneManager.sceneLoaded -= OnSceneLoad;
        }
        
        private void OnSceneLoad(Scene loaded, LoadSceneMode mode)
        {
            if (mode == LoadSceneMode.Single)
            {
                openGUIs = new List<string>();
            }
        }
        
        //scenes on this list can only be closed by passing their name to CloseOpenGUI
        public void AddToUncloseable(string toAdd)
        {
            doNotUnload.Add(toAdd);
        }

        public void CloseOpenGUI()
        {
            if (openGUIs == null || openGUIs.Count == 0)
                return;
            PauseCallback.pauseManager.Resume();
            foreach (string GUI in openGUIs)
            {
                if(!doNotUnload.Contains(GUI))
                    SceneManager.UnloadSceneAsync(GUI);
            }
            if(_hud == null)
                _hud = GameObject.Find("HUD");
            if(_hud != null)
                _hud.SetActive(true);
            openGUIs = new List<string>();
        }

        public bool CloseOpenGUI(string toClose)
        {
            if (openGUIs == null || openGUIs.Count == 0)
                return false;
            foreach (string GUI in openGUIs)
            {
                if (GUI.Equals(toClose))
                {
                    doNotUnload.Remove(toClose);
                    SceneManager.UnloadSceneAsync(GUI);
                    openGUIs.Remove(GUI);
                    if (openGUIs.Count == 0)
                    {
                        PauseCallback.pauseManager.Resume();
                        if(_hud == null)
                            _hud = GameObject.Find("HUD");
                        if(_hud != null)
                            _hud.SetActive(true);
                    }

                    return true;
                }
            }
            return false;
        }
        
        public void Load(String toLoad)
        {
            if (toLoad.Equals(openGUIs[0]) || toLoad.Equals(""))
            {
                CloseOpenGUI();
                return;
            }
            CloseOpenGUI();
            openGUIs = new List<string> {toLoad};
            PauseCallback.pauseManager.Pause();
            if(_hud == null)
                _hud = GameObject.Find("HUD");
            if(_hud != null)
                _hud.SetActive(false);
            SceneManager.LoadScene(openGUIs[0], LoadSceneMode.Additive);
        }

        public void Load(String toLoad, bool loadingMode)
        {
            if (!loadingMode)
            {
                Load(toLoad);
                return;
            }
            if (CloseOpenGUI(toLoad))
                return;
            if (openGUIs.Count == 0)
            {
                PauseCallback.pauseManager.Pause();
                if(_hud == null)
                    _hud = GameObject.Find("HUD");
                if(_hud != null)
                    _hud.SetActive(false);
            }
            openGUIs.Add(toLoad);
            SceneManager.LoadScene(openGUIs[0], LoadSceneMode.Additive);
            Scene scene = SceneManager.GetSceneByName(toLoad);
            GameObject[] objs = scene.GetRootGameObjects();
            foreach (GameObject obj in objs)
            {
                Canvas canvas = obj.GetComponent<Canvas>();
                if (canvas != null)
                {
                    canvas.sortingOrder = openGUIs.Count;
                    break;
                }
            }
        }

        public bool isGUIOPen()
        {
            return (openGUIs.Count > 0);
        }
    }
}
