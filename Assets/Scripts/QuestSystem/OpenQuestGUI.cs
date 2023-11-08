using Misc;
using ScriptTags;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

namespace QuestSystem
{
    //handles opening and closing the quest gui
    public class OpenQuestGUI : LoadGUI
    {

        //action to open gui
        public InputActionReference openGUI;

        private void OnEnable()
        {
            openGUI.action.performed += OnKPressed;
        }

        private void OnDisable()
        {
            openGUI.action.performed -= OnKPressed;
        }

        //if the quest gui is open, reenable player movement and close it, otherwise do the opposite
        private void OnKPressed(InputAction.CallbackContext callbackContext)
        {
            onOpenTrigger();
        }
    }
}
