using Misc;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class OpenTimeTravelUI : MonoBehaviour
{
    public InputActionReference astrolabeRef;

    private void OnEnable()
    {
        astrolabeRef.action.performed += openTimeTravelUI;
    }

    private void OnDisable()
    {
        astrolabeRef.action.performed -= openTimeTravelUI;
    }

    public void openTimeTravelUI(InputAction.CallbackContext context)
    {
        //load time travel scene on top of this scene
        if (LoadGUIManager.loadGUIManager.isGUIOPen())
        {
            LoadGUIManager.loadGUIManager.CloseOpenGUI();
        }
        else
        {
            AstrolabeUI.astrolabeUI.ONOpenTrigger();
        }
    }
}
