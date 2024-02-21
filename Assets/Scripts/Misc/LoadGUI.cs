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

        [Tooltip("True will make the GUI a pop up over existing GUIs, otherwise it will close open GUIs")]
        public bool loadingMode;

        [Tooltip("If checked, this GUI cannot be unclosed by another LoadGUI and must be closed manually by a LoadGUI of itself")]
        public bool setUnclosable;

        //if the gui is open, reenable player movement and close it
        //otherwise close any other active gui and open this one
        public virtual void ONOpenTrigger()
        {
            if(setUnclosable)
                LoadGUIManager.loadGUIManager.AddToUncloseable(loadScene);
            LoadGUIManager.loadGUIManager.Load(loadScene, loadingMode);
        }

        //static method to facilitate loading from other scripts
        //optional load mode will load as a pop up if true
        //optional uncloseable will prevent default closing of the scene without directly requesting it be closed
        public static void Open(string toLoad, bool loadMode = false, bool uncloseable = false)
        {
            if(uncloseable)
                LoadGUIManager.loadGUIManager.AddToUncloseable(toLoad);
            LoadGUIManager.loadGUIManager.Load(toLoad, loadMode);
        }
    }
}
