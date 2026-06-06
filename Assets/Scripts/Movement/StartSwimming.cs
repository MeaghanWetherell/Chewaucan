using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * This should be attached to the global volume of any body
 * of water that the player can swim in. That volume must have a collider with a ticked trigger.
 * 
 * On 6-6-26 Meaghan added some adjustments provided by Claude Code. 
 * The adjustments are to work with the SwimCameraAdjust script,
 * which is attached to the Player Follow Vcam.
 * These changes are indicated with comments in case they need to be removed.
 */

[RequireComponent(typeof(AudioSource))]
public class StartSwimming : MonoBehaviour
{
    [Tooltip("The WaterUp prefab game object (the actual water not the global volume)")]
    public Transform waterBlock;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            PlayerMovementController mvmtController = other.gameObject.GetComponent<PlayerMovementController>();
            SwimmingMovement swimmingMovement = other.gameObject.GetComponent<SwimmingMovement>();
            CameraLook camLook = other.gameObject.GetComponent<CameraLook>();
            camLook.SetMinDist(10f);
            other.gameObject.transform.position = new Vector3(other.gameObject.transform.position.x,
                waterBlock.position.y, other.gameObject.transform.position.z);
            swimmingMovement.SetWaterSoundSource(this.GetComponent<AudioSource>());
            mvmtController.SwitchToSwimming(waterBlock.position);
                        
            other.gameObject.GetComponent<SwimmingMovement>().swimCameraAdjust.SetSwimming(true); // 6-6-26
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            SwimmingMovement swimmingMovement = other.gameObject.GetComponent<SwimmingMovement>();
            swimmingMovement.SetSwimming(waterBlock.position);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            PlayerMovementController mvmtController = other.gameObject.GetComponent<PlayerMovementController>();
            CameraLook camLook = other.gameObject.GetComponent<CameraLook>();
            camLook.SetMinDist(30f);
            mvmtController.SwitchToWalking();

            other.gameObject.GetComponent<SwimmingMovement>().swimCameraAdjust.SetSwimming(false); //6-6-26
        }
    }
}
