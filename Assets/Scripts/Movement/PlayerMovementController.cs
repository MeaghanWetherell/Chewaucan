using Misc;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * Controller for switching between land and water movement,
 * much more organized than having both movement types in a single script
 * This script is attached to the player, as are SwimmingMovement and LandMovement
 * 
 * The SwimmingMovement and LandMovement components should never be both enabled,
 * only ever 1 enabled and the other disabled or both disbled when paused.
 */
public class PlayerMovementController : MonoBehaviour
{
    private LandMovement landMovement;
    private SwimmingMovement swimmingMovement;
    private ClimbingMovement climbingMovement;

    private bool walking;
    private bool swimming;
    private bool climbing;
    
    
    
    // Start is called before the first frame update
    void Start()
    {
        landMovement = GetComponent<LandMovement>();
        swimmingMovement = GetComponent<SwimmingMovement>();
        climbingMovement = GetComponent<ClimbingMovement>();

        landMovement.enabled = true;
        swimmingMovement.enabled = false;
        climbingMovement.enabled = false;
        walking = true;
        swimming = false;
        climbing = false;

        PauseCallback.pauseManager.SubscribeToPause(OnPause);
        PauseCallback.pauseManager.SubscribeToResume(OnResume);
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
        SetScripts();
    }

    public void SwitchToSwimming(Vector3 waterLevel)
    {
        walking = false;
        swimming = true;
        climbing = false;
        SetScripts();
        swimmingMovement.SetSwimming(waterLevel);
        
        
    }

    public void SwitchToWalking()
    {
            walking = true;
            swimming = false;
            climbing = false;
            SetScripts();
    }

    private void SetScripts()
    {
        swimmingMovement.enabled = swimming;
        landMovement.enabled = walking;
        climbingMovement.enabled = climbing;
    }

    //disable movement when paused
    private void OnPause()
    {
        swimmingMovement.enabled = false;
        landMovement.enabled = false;
        climbingMovement.enabled = false;
        this.enabled = false;
    }

    //reenable movement on resume
    private void OnResume()
    {
        SetScripts();
        this.enabled = true;
    }

}
