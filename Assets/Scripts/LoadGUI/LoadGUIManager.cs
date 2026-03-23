using System;
using System.Collections.Generic;
using System.Linq;
using Misc;
using ScriptTags;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace LoadGUIFolder
{
    public class LoadGUIManager : MonoBehaviour
    {
        //singleton
        public static LoadGUIManager loadGUIManager;

        public AudioSource menuCloseEffect;
        
        //name of the active GUI
        private String GUIName;

        //all active pop-ups
        private List<GameObject> popUps = new List<GameObject>();
        
        //store a ref to the HUD object
        private GameObject _hud;

        //pop-up prefab
        private GameObject popUp;

        //yes/no pop-up prefab
        private GameObject YNPopUp;

        //called when unloading a GUI (including pop-ups)
        private UnityEvent<string> OnGUIUnload = new UnityEvent<string>();
        
        //called when loading a GUI (including pop-ups)
        private UnityEvent<string> OnGUILoad = new UnityEvent<string>();

        //save a reference to camera look
        private CameraLook cacheCamLook;

        public void SubtoLoad(UnityAction<String> action)
        {
            OnGUILoad.AddListener(action);
        }

        public void UnsubtoLoad(UnityAction<String> action)
        {
            OnGUILoad.RemoveListener(action);
        }
        
        public void SubtoUnload(UnityAction<String> action)
        {
            OnGUIUnload.AddListener(action);
        }

        public void UnsubtoUnload(UnityAction<String> action)
        {
            OnGUIUnload.RemoveListener(action);
        }
        
        private void Awake()
        {
            if (loadGUIManager != null)
            {
                Destroy(gameObject);
                return;
            }
            popUp = Resources.Load<GameObject>("PopUp");
            YNPopUp = Resources.Load<GameObject>("YNPopup");
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
        
        //when a scene is loaded in single mode, all active additive scenes are removed. update internals to reflect this
        private void OnSceneLoad(Scene loaded, LoadSceneMode mode)
        {
            if (mode == LoadSceneMode.Single)
            {
                GUIName = null;
                popUps = new List<GameObject>();
            }
        }

        //sub a function to the on close event for the top-most pop-up
        public bool SubToTopPopUp(UnityAction<string> toSub)
        {
            if (popUps.Count < 1) return false;
            GameObject pUp = popUps[^1];
            PopUpManager manager = pUp.GetComponent<PopUpManager>();
            if(manager != null)
                manager.onClose.AddListener(toSub);
            else
            {
                return false;
            }
            return true;
        }

        //create a new yes/no pop-up with the passed title and message
        //onConfirm runs when the player clicks the positive option, onPopUpClosed is invoked when the pop-up is closed without confirming
        //conf/decText are the text displayed on the buttons to confirm or decline
        public void InstantiateYNPopUp(string title, string msg, List<UnityAction<string>> onConfirm, string confText = "Confirm", string decText = "Decline",
            List<UnityAction<string>> onPopUpClosed = null)
        {
            GameObject window = Instantiate(YNPopUp);
            if (onPopUpClosed == null)
                onPopUpClosed = new List<UnityAction<string>>();
            YNPopUpManager manager = window.GetComponent<YNPopUpManager>();
            manager.SetText(title, msg, decText, confText);
            manager.index = popUps.Count;
            foreach (UnityAction<string> act in onPopUpClosed)
            {
                manager.onClose.AddListener(act);
            }
            foreach (UnityAction<string> conf in onConfirm)
            {
                manager.onConfirm.AddListener(conf);
            }
            window.GetComponent<Canvas>().sortingOrder += popUps.Count;
            popUps.Add(window);
            OnGUILoad.Invoke(title);
            if (Player.player != null)
            {
                cacheCamLook = Player.player.gameObject.GetComponent<CameraLook>();
                if(cacheCamLook!= null)
                    cacheCamLook.OnPause();
            }
        }
        
        //overload of InstantiateYNPopUp that takes an existing pop-up object and registers it properly with the manager
        //actions in onConfirm and onPopUpClosed are *added* to existing subscribed actions
        public void InstantiateYNPopUp(GameObject inPopUp, string title, string msg, List<UnityAction<string>> onConfirm, string confText = "Confirm", string decText = "Decline",
            List<UnityAction<string>> onPopUpClosed = null)
        {
            GameObject window = inPopUp;
            if (onPopUpClosed == null)
                onPopUpClosed = new List<UnityAction<string>>();
            YNPopUpManager manager = window.GetComponent<YNPopUpManager>();
            manager.SetText(title, msg, decText, confText);
            manager.index = popUps.Count;
            foreach (UnityAction<string> act in onPopUpClosed)
            {
                manager.onClose.AddListener(act);
            }
            foreach (UnityAction<string> conf in onConfirm)
            {
                manager.onConfirm.AddListener(conf);
            }
            window.GetComponent<Canvas>().sortingOrder += popUps.Count;
            popUps.Add(window);
            OnGUILoad.Invoke(title);
            if (Player.player != null)
            {
                cacheCamLook = Player.player.gameObject.GetComponent<CameraLook>();
                if(cacheCamLook!= null)
                    cacheCamLook.OnPause();
            }
        }

        //creates a new pop-up with the passed title and message. actions in onPopUpClosed run when that popUp is closed
        public void InstantiatePopUp(String title, String msg, List<UnityAction<string>> onPopUpClosed = null)
        {
            GameObject window = Instantiate(popUp);
            if (onPopUpClosed == null)
                onPopUpClosed = new List<UnityAction<string>>();
            PopUpManager manager = window.GetComponent<PopUpManager>();
            manager.SetText(title, msg);
            manager.index = popUps.Count;
            foreach (UnityAction<string> act in onPopUpClosed)
            {
                manager.onClose.AddListener(act);
            }
            window.GetComponent<Canvas>().sortingOrder += popUps.Count;
            popUps.Add(window);
            OnGUILoad.Invoke(title);
            if (Player.player != null)
            {
                cacheCamLook = Player.player.gameObject.GetComponent<CameraLook>();
                if(cacheCamLook!= null)
                    cacheCamLook.OnPause();
            }
        }
        
        //registers custom popUp with LoadGUI. most setup should be done outside loadGUI unless running with "instantiatePrefab=true". must have a PopUpTextManager and PopUpOnClick attached.
        public void InstantiatePopUp(GameObject inPopUp, string popUpName, string msg = "", List<UnityAction<string>> onPopUpClosed = null, bool instantiatePrefab = false)
        {
            if (instantiatePrefab)
            {
                inPopUp = Instantiate(inPopUp);
            }
            if (onPopUpClosed == null)
                onPopUpClosed = new List<UnityAction<string>>();
            GameObject window = inPopUp;
            PopUpManager manager = window.GetComponent<PopUpManager>();
            manager.SetText(popUpName, msg);
            manager.index = popUps.Count;
            foreach (UnityAction<string> act in onPopUpClosed)
            {
                manager.onClose.AddListener(act);
            }
            window.GetComponent<Canvas>().sortingOrder += popUps.Count;
            popUps.Add(inPopUp);
            OnGUILoad.Invoke(popUpName);
            if (Player.player != null)
            {
                cacheCamLook = Player.player.gameObject.GetComponent<CameraLook>();
                if(cacheCamLook!= null)
                    cacheCamLook.OnPause();
            }
        }

        //registers that the top pop-up has closed
        public void RegisterPopUpClose()
        {
            if (popUps.Count == 0) return;
            string title = popUps[popUps.Count - 1].GetComponent<PopUpManager>().title;
            OnGUIUnload.Invoke(title);
            popUps.RemoveAt(popUps.Count-1);
            if (!isGUIOpen() && popUps.Count == 0)
            {
                if(cacheCamLook != null)
                    cacheCamLook.OnResume();
                PauseCallback.pauseManager.Resume();
            }
        }

        //closes the top pop-up
        public void ClosePopUp()
        {
            if (popUps.Count == 0)
                return;
            Destroy(popUps.Last());
            RegisterPopUpClose();
        }

        //registers the pop-up at the passed index as closed
        public void RegisterPopUpClose(int index)
        {
            string title = popUps[index].GetComponent<PopUpManager>().title;
            OnGUIUnload.Invoke(title);
            popUps.RemoveAt(index);
            if (!isGUIOpen() && popUps.Count == 0)
            {
                if(cacheCamLook != null)
                    cacheCamLook.OnResume();
                PauseCallback.pauseManager.Resume();
            }
        }

        //closes the pop-up at the passed index
        public void ClosePopUp(int index)
        {
            if (popUps.Count <= index)
                return;
            Destroy(popUps[index]);
            RegisterPopUpClose(index);
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
            menuCloseEffect.Play();
            PauseCallback.pauseManager.Resume();
            OnGUIUnload.Invoke(GUIName);
            SceneManager.UnloadSceneAsync(GUIName);
            GUIName = null;
            return true;
        }

        //closes the open GUI if it has the passed name. returns true if it successfully closes a GUI
        public bool CloseOpenGUI(String gui)
        {
            if (GUIName == null)
                return false;
            if (GUIName.Equals(gui))
            {
                PauseCallback.pauseManager.Resume();
                OnGUIUnload.Invoke(GUIName);
                SceneManager.UnloadSceneAsync(GUIName);
                GUIName = null;
                return true;
            }

            return false;
        }

        //additively loads the GUI specified by the passed string
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
            SceneManager.LoadScene(GUIName, LoadSceneMode.Additive);
            OnGUILoad.Invoke(GUIName);
            return true;
        }

        //returns whether there is currently an additively loaded GUI open
        public bool isGUIOpen()
        {
            return (GUIName != null);
        }
    }
}
