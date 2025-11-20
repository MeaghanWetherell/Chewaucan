using System;
using System.Collections;
using System.Collections.Generic;
using ScriptTags;
using UnityEngine;
using UnityEngine.InputSystem;

public class ChangePlayerInputOnEnable : MonoBehaviour
{
    public bool enableOrDisable;

    public bool resetOnDisable = true;

    private PlayerInput playerInput;

    private void OnEnable()
    {
        playerInput = Player.player.GetComponent<PlayerInput>();
        playerInput.enabled = enableOrDisable;
    }

    private void OnDisable()
    {
        if(resetOnDisable)
            playerInput.enabled = !enableOrDisable;
    }
}
