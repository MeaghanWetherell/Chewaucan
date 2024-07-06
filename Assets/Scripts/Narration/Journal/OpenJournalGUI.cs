using System;
using LoadGUIFolder;
using Misc;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Narration.Journal
{
    public class OpenJournalGUI : LoadGUI
    {
        public InputActionReference openTrigger;
        
        private void OnEnable()
        {
            openTrigger.action.performed += OpenJournal;
        }

        private void OnDisable()
        {
            openTrigger.action.performed -= OpenJournal;
        }

        private void OpenJournal(InputAction.CallbackContext context)
        {
            
        }
    }
}
