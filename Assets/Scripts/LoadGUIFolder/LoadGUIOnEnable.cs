using System;
using System.Collections;
using System.Collections.Generic;
using LoadGUIFolder;
using ScriptTags;
using UnityEngine;
using UnityEngine.InputSystem;

public class LoadGUIOnEnable : LoadGUI
{
    private PlayerInput playerInput;

    public bool disablePlayerInput;
    
    private void OnEnable()
    {
        playerInput = Player.player.GetComponent<PlayerInput>();
        if (disablePlayerInput)
            playerInput.enabled = false;
        ONOpenTrigger();
    }

    private void OnDisable()
    {
        if (disablePlayerInput)
            playerInput.enabled = true;
        ONOpenTrigger();
    }
}
