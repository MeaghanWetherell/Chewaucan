using System;
using System.Collections;
using System.Collections.Generic;
using Misc;
using ScriptTags;
using UnityEngine;

public class Tumbleweed : MonoBehaviour
{
    [Tooltip("Stacking modifier to movespeed when hit by a tumbleweed")]
    public float moveSpeedMult;

    public float multDuration;

    public Animator controller;

    private void OnEnable()
    {
        PauseCallback.pauseManager.SubscribeToPause(OnPause);
        PauseCallback.pauseManager.SubscribeToResume(OnResume);
    }

    private void OnDisable()
    {
        PauseCallback.pauseManager.UnsubToPause(OnPause);
        PauseCallback.pauseManager.UnsubToResume(OnResume);
    }

    private void OnPause()
    {
        controller.speed = 0;
    }
    
    private void OnResume()
    {
        controller.speed = 1;
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.collider.GetComponent<Player>() != null)
        {
            other.gameObject.GetComponent<LandMovement>().ChangeMoveSpeedMultForTime(moveSpeedMult, multDuration);
        }
        Destroy(gameObject);
    }
}
