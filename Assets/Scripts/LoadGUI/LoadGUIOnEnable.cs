using System;
using System.Collections;
using System.Collections.Generic;
using LoadGUIFolder;
using Misc;
using ScriptTags;
using UnityEngine;
using UnityEngine.InputSystem;

public class LoadGUIOnEnable : LoadGUI
{
    private PlayerInput playerInput;

    //TODO pretty sure this breaks controller support for menus when its enabled
    [Tooltip("Whether to disable player input while the GUI is loaded.")]
    public bool disablePlayerInput;

    [Tooltip("Whether audio should be enabled while the GUI is loaded")]
    public bool enableAudio;

    [Tooltip("Wether we should be paused while the GUI is loaded")]
    public bool pause;
    
    private void OnEnable()
    {
        if(Player.player != null)
            playerInput = Player.player.GetComponent<PlayerInput>();
        if (disablePlayerInput && playerInput != null) 
            playerInput.enabled = false;
        ONOpenTrigger();
        if (enableAudio)
            AudioListener.pause = false;
        if(!pause)
            PauseCallback.pauseManager.Resume();
    }

    private void OnDisable()
    {
        if (disablePlayerInput)
            playerInput.enabled = true;
        LoadGUIManager.loadGUIManager.CloseOpenGUI(loadScene);
    }
}
