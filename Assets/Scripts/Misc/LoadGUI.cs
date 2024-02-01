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
        [Tooltip("Scene to load")]
        public String loadScene;

        //if the gui is open, reenable player movement and close it
        //otherwise close any other active gui and open this one
        public virtual void ONOpenTrigger()
        {
            LoadGUIManager.loadGUIManager.Load(loadScene);
        }
    }
}
