using System;
using System.Collections;
using System.Collections.Generic;
using Misc;
using ScriptTags;
using Unity.VisualScripting;
using UnityEngine;

public class Tumbleweed : MonoBehaviour
{
    [Tooltip("Stacking modifier to movespeed when hit by a tumbleweed")]
    public float moveSpeedMult;

    [Tooltip("Length of the modifier to move speed")]
    public float multDuration;

    [Tooltip("Overlay sprite to be passed to the hud for display when the player is hit")]
    public Sprite overlay;

    public Animator controller;

    [Tooltip("Reference to a prefab that plays sound after the tumbleweed has been destroyed")]
    public GameObject tumbleweedSoundObj;
    
    [Tooltip("Length of time before the tumbleweed fades out")]
    public float lifetime;

    [Tooltip("Time to fade out after lifetime expires")]
    public float cullTime;

    public MeshRenderer tumbleweedRenderer;
    
    [Tooltip("Reference to a transparent version of the tumbleweed's material")]
    public Material transMat;

    private void OnEnable()
    {
        PauseCallback.pauseManager.SubscribeToPause(OnPause);
        PauseCallback.pauseManager.SubscribeToResume(OnResume);
        StartCoroutine(Lifetime());
    }

    private void OnDisable()
    {
        StopAllCoroutines();
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

    //track lifetime, fade the tumbleweed out after it expires
    private IEnumerator Lifetime()
    {
        while (lifetime > 0)
        {
            if (!PauseCallback.pauseManager.isPaused)
                lifetime -= 0.05f;
            yield return new WaitForSeconds(0.05f);
        }

        tumbleweedRenderer.material = transMat;
        float curCullTime = 0;
        while (curCullTime < cullTime)
        {
            curCullTime += 0.05f;
            float val = Mathf.Lerp(1, 0, curCullTime / cullTime);
            Color temp = tumbleweedRenderer.material.color;
            tumbleweedRenderer.material.color = new Color(temp.r, temp.g, temp.b, val);
            yield return new WaitForSeconds(0.05f);
        }
        Destroy(gameObject);
    }
    

    //when the player is hit, change their move speed for a while, play some sounds, display the hit to the hud, and destroy the tumbleweed
    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Player>() != null)
        {
            other.gameObject.GetComponent<LandMovement>().ChangeMoveSpeedMultForTime(moveSpeedMult, multDuration);
            HUDManager.hudManager.DisplayMessageToHUDForTime("You got hit by a tumbleweed! Watch out!", 3);
            HUDManager.hudManager.CreateFadingOverlay(overlay, multDuration);
            Player.playerA.PlayAHHH();
            Instantiate(tumbleweedSoundObj).transform.position = transform.position;
        }
        Destroy(gameObject);
    }
}
