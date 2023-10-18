using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartSwimming : MonoBehaviour
{
    public Transform waterBlock;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            PlayerMovement playerMovement = other.gameObject.GetComponent<PlayerMovement>();
            CameraLook camLook = other.gameObject.GetComponent<CameraLook>();
            playerMovement.setSwimming(true, waterBlock.position);
            camLook.setMinDist(50f);
            //Debug.Log("IN WATER");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            PlayerMovement playerMovement = other.gameObject.GetComponent<PlayerMovement>();
            CameraLook camLook = other.gameObject.GetComponent<CameraLook>();
            playerMovement.setSwimming(false, waterBlock.position);
            camLook.setMinDist(25f);
            //Debug.Log("ON LAND");
        }
    }
}
