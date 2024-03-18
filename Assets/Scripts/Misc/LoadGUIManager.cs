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
        
        private String GUIName;

        private List<GameObject> popUps = new List<GameObject>();
        
        //store a ref to the HUD object
        private GameObject _hud;

        private GameObject popUp;
        
        private void Awake()
        {
            if (loadGUIManager != null)
            {
                Debug.LogError("Loaded persistent objects twice!");
                Destroy(loadGUIManager.gameObject);
            }
            loadGUIManager = this;
            DontDestroyOnLoad(this.gameObject);
            popUp = Resources.Load<GameObject>("PopUp");
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

        public void InstantiatePopUp(String title, String msg)
        {
            GameObject window = Instantiate(popUp);
            window.GetComponent<PopUpTextManager>().SetText(title, msg);
            window.GetComponent<PopUpOnClick>().index = popUps.Count;
            window.GetComponent<Canvas>().sortingOrder += popUps.Count;
            popUps.Add(window);
            PauseCallback.pauseManager.Pause();
            GameObject hud = GameObject.Find("HUD");
            if(hud != null)
                hud.SetActive(false);
        }

        public void ClosePopUp()
        {
            Destroy(popUps.Last());
            popUps.RemoveAt(popUps.Count-1);
            if (!isGUIOpen() && popUps.Count == 0)
            {
                PauseCallback.pauseManager.Resume();
            }
        }

        public void ClosePopUp(int index)
        {
            Destroy(popUps[index]);
            popUps.RemoveAt(index);
            if (!isGUIOpen() && popUps.Count == 0)
            {
                PauseCallback.pauseManager.Resume();
            }
        }

        //returns true if there are no open guis after close, false otherwise
        public bool CloseOpenGUI()
        {
            if (GUIName == null)
                return true;
            if (popUps.Count > 0)
            {
                ClosePopUp();
                return false;
            }
            PauseCallback.pauseManager.Resume();
            SceneManager.UnloadSceneAsync(GUIName);
            if(_hud == null)
                _hud = GameObject.Find("HUD");
            if(_hud != null)
                _hud.SetActive(true);
            GUIName = null;
            return true;
        }

        public bool Load(String toLoad)
        {
            if (toLoad.Equals(GUIName) || toLoad.Equals(""))
            {
                CloseOpenGUI();
                return false;
            }
            if (!CloseOpenGUI())
            {
                return false;
            }
            GUIName = toLoad;
            PauseCallback.pauseManager.Pause();
            if(_hud == null)
                _hud = GameObject.Find("HUD");
            if(_hud != null)
                _hud.SetActive(false);
            SceneManager.LoadScene(GUIName, LoadSceneMode.Additive);
            return true;
        }

        public bool isGUIOpen()
        {
            return (GUIName != null);
        }
    }
}
