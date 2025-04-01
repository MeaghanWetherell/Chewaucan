using System.Collections;
using System.Collections.Generic;
using LoadGUIFolder;
using Misc;
using UnityEngine;
using UnityEngine.InputSystem;

public class LoadPauseOnButtonPress : LoadGUIOnButtonPress
{
    protected override void OnButtonPressed(InputAction.CallbackContext callbackContext)
    {
        if (!SceneLoadWrapper.sceneLoadWrapper.isLoading)
        {
            base.OnButtonPressed(callbackContext);
        }
            
    }
}
