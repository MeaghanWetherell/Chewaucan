using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartSwimming : MonoBehaviour
{
    

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            PlayerMovement playerMovement = other.gameObject.GetComponent<PlayerMovement>();
            CameraLook camLook = other.gameObject.GetComponent<CameraLook>();
            playerMovement.setSwimming(true, this.transform.position);
            camLook.minViewDist = 10f;
            //Debug.Log("IN WATER");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            PlayerMovement playerMovement = other.gameObject.GetComponent<PlayerMovement>();
            CameraLook camLook = other.gameObject.GetComponent<CameraLook>();
            playerMovement.setSwimming(false, this.transform.position);
            camLook.minViewDist = 25f;
            //Debug.Log("ON LAND");
        }
    }
}
