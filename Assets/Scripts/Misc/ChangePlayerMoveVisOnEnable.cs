using System;
using System.Collections;
using System.Collections.Generic;
using ScriptTags;
using UnityEngine;

public class ChangePlayerMoveVisOnEnable : MonoBehaviour
{
    public bool movementEnabled;

    public bool playerCameraEnabled;

    public bool rendererEnabled;

    private PlayerMovementController _movementController;

    private LandMovement _landMovement;

    private GameObject mainCamera;

    private MeshRenderer meshRenderer;

    public bool resetOnDisable = true;

    private void OnEnable()
    {
        _movementController = Player.player.GetComponent<PlayerMovementController>();
        _landMovement = Player.player.GetComponent<LandMovement>();
        _movementController.enabled = movementEnabled;
        _landMovement.enabled = movementEnabled;
        mainCamera = GameObject.FindGameObjectWithTag("PlayerFollow");
        mainCamera.SetActive(playerCameraEnabled);
        meshRenderer = Player.player.GetComponent<MeshRenderer>();
        meshRenderer.enabled = rendererEnabled;
    }

    private void OnDisable()
    {
        if (resetOnDisable)
        {
            if(_movementController!=null)
                _movementController.enabled = !movementEnabled;
            if(mainCamera != null)
                mainCamera.SetActive(!playerCameraEnabled);
            if(meshRenderer != null)
                meshRenderer.enabled = !rendererEnabled;
        }
    }
}
