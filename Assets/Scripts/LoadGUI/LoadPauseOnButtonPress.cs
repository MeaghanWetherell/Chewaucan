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
        if (!SceneLoadWrapper.sceneLoadWrapper.isLoading && !SceneManager.GetActiveScene().name.Equals("MainMenuUI") && !IsAnyCutscenePlaying())
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
    
    private bool IsAnyCutscenePlaying() {
        var directors = FindObjectsByType<UnityEngine.Playables.PlayableDirector>(FindObjectsSortMode.None);
        foreach (var d in directors) {
            if (d.state == UnityEngine.Playables.PlayState.Playing) return true;
        }
        return false;
    }
}
