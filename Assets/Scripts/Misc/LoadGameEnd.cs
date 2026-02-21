using System;
using System.Collections;
using System.Collections.Generic;
using LoadGUIFolder;
using Misc;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class LoadGameEnd : MonoBehaviour
{
    public InputActionReference eg;

    private void Awake()
    {
        if (eg != null)
        {
            eg.action.started += End;
        }
    }

    private void OnDisable()
    {
        if (eg != null)
        {
            eg.action.started -= End;
        }
    }

    private void End(InputAction.CallbackContext c)
    {
        End();
    }

    public void End()
    {
        LoadGUIManager.loadGUIManager.InstantiateYNPopUp("End Game?", "Hit confirm to tell the little kid their birthday story and end the game.", new List<UnityAction<string>>(){LoadEnd});
    }

    public void LoadEnd(string n)
    {
        SceneLoadWrapper.sceneLoadWrapper.LoadScene("GameEnd");
    }
}
