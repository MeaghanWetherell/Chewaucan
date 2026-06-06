using System;
using Misc;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

/**
 * Controller for switching between land and water movement,
 * much more organized than having both movement types in a single script
 * This script is attached to the player, as are SwimmingMovement and LandMovement
 * 
 * The SwimmingMovement and LandMovement components should never be both enabled,
 * only ever 1 enabled and the other disabled or both disbled when paused.
 * 
 * On 6-6-2026 Meaghan added some adjustments here to make this play better
 * with the ambient sound manager system and also with the new "move the camera closer during swimming"
 * stuff. These adjustments were done with Claude Code, and are indicated in case something breaks.
 */
public class PlayerMovementController : MonoBehaviour
{
    public InputActionReference endClimb;

    [Tooltip("Local water sounds you shouldn't hear underwater")]
    public List<AudioSource> stopOnStartDiving;
    
    private LandMovement landMovement;
    private SwimmingMovement swimmingMovement;
    private ActiveSoundManager activeSoundManager; //added 6-6-26

    private ClimbingMovement climbingMovement;

    private bool walking;
    private bool swimming;
    private bool climbing;
    
    // Start is called before the first frame update
    void Awake()
    {
        landMovement = GetComponent<LandMovement>();
        swimmingMovement = GetComponent<SwimmingMovement>();
        climbingMovement = GetComponent<ClimbingMovement>();
        activeSoundManager = GetComponentInChildren<ActiveSoundManager>(true); // 6-6-26 addition

        landMovement.enabled = true;
        swimmingMovement.enabled = false;
        climbingMovement.enabled = false;
        walking = true;
        swimming = false;
        climbing = false;

        PauseCallback.pauseManager.SubscribeToPause(OnPause);
        PauseCallback.pauseManager.SubscribeToResume(OnResume);
    }

    private void EndClimb(InputAction.CallbackContext context)
    {
        if (climbing)
        {
            SwitchToWalking();
        }
    }

    private void OnEnable()
    {
        endClimb.action.started += EndClimb;
        SetScripts();
    }

    private void OnDisable()
    {
        endClimb.action.started -= EndClimb;
    }

    private void OnDestroy()
    {
        PauseCallback.pauseManager.UnsubToPause(OnPause);
        PauseCallback.pauseManager.UnsubToResume(OnResume);
    }

    public void SwitchToClimbing(Collider curCollider)
    {
        climbingMovement.curCollider = curCollider;
        climbingMovement.moveInput = landMovement.moveInput;
        walking = false;
        swimming = false;
        climbing = true;
        HUDManager.hudManager.DisplayMessageToHUD("Press C to stop climbing");
        SetScripts();
    }

    public void SwitchToSwimming(Vector3 waterLevel)
    {
        walking = false;
        swimming = true;
        climbing = false;
        //gameObject.GetComponentInChildren<RandomAmbientSound>().enabled = false; //6-6-26 turned off
        if (activeSoundManager != null) activeSoundManager.DisableAmbientSounds(); //6-6-26
       // foreach (var src in stopOnStartDiving)
         //   src.Stop();
        SetScripts();
        swimmingMovement.SetSwimming(waterLevel);
        
        
    }

    public bool isWalkingOrClimbing()
    {
        return walking || climbing;
    }

    public void SwitchToWalking()
    {
        walking = true;
        swimming = false;
        climbing = false;
        //RandomAmbientSound rand = gameObject.GetComponentInChildren<RandomAmbientSound>(true);  /6-6-26 disabled
        //if(rand != null)
        //  rand.enabled = true;

        if (activeSoundManager != null) activeSoundManager.EnableAmbientSounds(); //enabled 6-6-26
       // foreach (var src in stopOnStartDiving)
         //   src.Play();
        HUDManager.hudManager.CloseMessage();
        SetScripts();
    }

    private void SetScripts()
    {
        swimmingMovement.enabled = swimming;
        landMovement.enabled = walking;
        if(climbing && climbingMovement.enabled)
            climbingMovement.OnEnable();
        else
        {
            climbingMovement.enabled = climbing;
        }
        
    }

    //disable movement when paused
    private void OnPause()
    {
        swimmingMovement.enabled = false;
        landMovement.enabled = false;
        climbingMovement.enabled = false;
    }

    //reenable movement on resume
    private void OnResume()
    {
        SetScripts();
    }

}
