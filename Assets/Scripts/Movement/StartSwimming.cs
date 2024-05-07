using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * This should be attached to the global volume of any body
 * of water that the player can swim in.
 * The
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
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            SwimmingMovement swimmingMovement = other.gameObject.GetComponent<SwimmingMovement>();
            swimmingMovement.SetSwimming(true, waterBlock.position);
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
        }
    }
}
