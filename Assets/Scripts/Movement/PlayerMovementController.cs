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

    private bool walking;
    private bool swimming;
    
    // Start is called before the first frame update
    void Start()
    {
        landMovement = GetComponent<LandMovement>();
        swimmingMovement = GetComponent<SwimmingMovement>();

        landMovement.enabled = true;
        swimmingMovement.enabled = false;
        walking = true;
        swimming = false;

        PauseCallback.pauseManager.SubscribeToPause(OnPause);
        PauseCallback.pauseManager.SubscribeToResume(OnResume);
    }

    private void OnDestroy()
    {
        PauseCallback.pauseManager.UnsubToPause(OnPause);
        PauseCallback.pauseManager.UnsubToResume(OnResume);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SwitchToSwimming(Vector3 waterLevel)
    {
        landMovement.enabled = false;
        swimmingMovement.SetSwimming(waterLevel);
        swimmingMovement.enabled = true;
        walking = false;
        swimming = true;
    }

    public void SwitchToWalking()
    {
        swimmingMovement.enabled = false;
        landMovement.enabled = true;
        walking = true;
        swimming = false;
    }

    //disable movement when paused
    private void OnPause()
    {
        swimmingMovement.enabled = false;
        landMovement.enabled = false;
        this.enabled = false;
    }

    //reenable movement on resume
    private void OnResume()
    {
        swimmingMovement.enabled = swimming;
        landMovement.enabled = walking;
        this.enabled = true;
    }

}
