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

    public void SwitchToClimbing(Quaternion targRot, Collider curCollider)
    {
        climbingMovement.targRot = targRot;
        climbingMovement.curCollider = curCollider;
        walking = false;
        swimming = false;
        climbing = true;
        SetScripts();
    }

    private IEnumerator ClimbingDisabled()
    {
        float rotSpeed = climbingMovement.rotSpeed;
        float x = transform.eulerAngles.x;
        float z = transform.eulerAngles.z;
        while (Mathf.Abs(x)+Mathf.Abs(z)>6f)
        {
            if (x > 3f)
            {
                transform.Rotate(Vector3.left, rotSpeed*0.05f);
            }

            else if (x < -3f)
            {
                transform.Rotate(Vector3.right, rotSpeed*0.05f);
            }

            if (z > 3f)
            {
                transform.Rotate(Vector3.forward, rotSpeed*0.05f);
            }
            else if (z < -3f)
            {
                transform.Rotate(Vector3.back, rotSpeed*0.05f);
            }
            
            x = transform.eulerAngles.x;
            z = transform.eulerAngles.z;
            yield return new WaitForSeconds(0.05f);
        }
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
        if (climbing)
        {
            walking = true;
            swimming = false;
            climbing = false;
            StartCoroutine(ClimbingDisabled());
        }
        else
        {
            walking = true;
            swimming = false;
            climbing = false;
            SetScripts();
        }
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
