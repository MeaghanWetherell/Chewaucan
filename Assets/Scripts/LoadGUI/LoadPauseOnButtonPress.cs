using System.Collections;
using System.Collections.Generic;
using LoadGUIFolder;
using Misc;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class LoadPauseOnButtonPress : LoadGUIOnButtonPress
{
    protected override void OnButtonPressed(InputAction.CallbackContext callbackContext)
    {
        if (!SceneLoadWrapper.sceneLoadWrapper.isLoading && !SceneManager.GetActiveScene().name.Equals("MainMenuUI"))
        {
            if (LoadGUIManager.loadGUIManager.isGUIOpen())
            {
                LoadGUIManager.loadGUIManager.CloseOpenGUI();
            }
            else
            {
                base.OnButtonPressed(callbackContext);
            }
        }
            
    }
}
