using System;
using ScriptTags;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

//handles GUI additive loading
namespace Misc
{
    public class LoadGUI : MonoBehaviour
    {
        //store a ref to the HUD object
        private static GameObject _hud;

        //whether the gui is currently open
        private bool _guiOpen = false;

        [Tooltip("Scene to load")]
        public String loadScene;
        
        //the gui that is currently open
        private static LoadGUI _curGUI;

        private void Awake()
        {
            if(_hud == null)
                _hud = GameObject.Find("HUD");
        }

        //if the gui is open, reenable player movement and close it
        //otherwise close any other active gui and open this one
        public virtual void ONOpenTrigger()
        {
            if (_guiOpen)
            {
                PauseCallback.pauseManager.Resume();
                MainSceneDataSaver.mainSceneDataSaver.curMenu = null;
                SceneManager.UnloadSceneAsync(loadScene);
                if(_hud != null)
                    _hud.SetActive(true);
            }
            else
            {
                if(MainSceneDataSaver.mainSceneDataSaver.curMenu != null)
                    MainSceneDataSaver.mainSceneDataSaver.curMenu.ONOpenTrigger();
                MainSceneDataSaver.mainSceneDataSaver.curMenu = this;
                PauseCallback.pauseManager.Pause();
                if(_hud != null)
                    _hud.SetActive(false);
                SceneManager.LoadScene(loadScene, LoadSceneMode.Additive);
                _curGUI = this;
            }
            _guiOpen = !_guiOpen;
        }
    }
}
