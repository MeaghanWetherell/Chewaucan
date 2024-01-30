using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Misc
{
    //open the pause menu on escape key callback. If a menu is already open, escape instead closes it
    public class OpenPauseMenu : MonoBehaviour
    {
        [Tooltip("Reference to the pause key")]
        public InputActionReference escape;

        private void OnEnable()
        {
            escape.action.performed += OpenPause;
        }

        private void OnDisable()
        {
            escape.action.performed -= OpenPause;
        }

        private void OpenPause(InputAction.CallbackContext context)
        {
            if (LoadGUIManager.loadGUIManager.isGUIOPen())
            {
                LoadGUIManager.loadGUIManager.CloseOpenGUI();
            }
            else
            {
                PauseMenu.pauseMenu.ONOpenTrigger();
            }
        }
    }
}
