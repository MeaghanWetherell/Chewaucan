using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Misc
{
    public class LoadGUIManager : MonoBehaviour
    {
        public static LoadGUIManager loadGUIManager;
        
        private String GUIName;
        
        //store a ref to the HUD object
        private GameObject _hud;
        
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
        
        public void OnEnable()
        {
            SceneManager.sceneLoaded += OnSceneLoad;
        }

        public void OnDisable()
        {
            SceneManager.sceneLoaded -= OnSceneLoad;
        }
        
        private void OnSceneLoad(Scene loaded, LoadSceneMode mode)
        {
            if (mode == LoadSceneMode.Single)
            {
                GUIName = null;
            }
        }

        public void CloseOpenGUI()
        {
            if (GUIName == null)
                return;
            PauseCallback.pauseManager.Resume();
            SceneManager.UnloadSceneAsync(GUIName);
            if(_hud == null)
                _hud = GameObject.Find("HUD");
            if(_hud != null)
                _hud.SetActive(true);
            GUIName = null;
        }

        public void Load(String toLoad)
        {
            CloseOpenGUI();
            GUIName = toLoad;
            PauseCallback.pauseManager.Pause();
            if(_hud == null)
                _hud = GameObject.Find("HUD");
            if(_hud != null)
                _hud.SetActive(false);
            SceneManager.LoadScene(GUIName, LoadSceneMode.Additive);
        }

        public bool isGUIOPen()
        {
            return (GUIName != null);
        }
    }
}
