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
        //whether the gui is currently open
        private bool _guiOpen = false;

        [Tooltip("Scene to load")]
        public String loadScene;

        //if the gui is open, reenable player movement and close it
        //otherwise close any other active gui and open this one
        public virtual void ONOpenTrigger()
        {
            LoadGUIManager.loadGUIManager.CloseOpenGUI();
            if (!_guiOpen)
            {
                LoadGUIManager.loadGUIManager.Load(loadScene);
            }
            _guiOpen = !_guiOpen;
        }
    }
}
