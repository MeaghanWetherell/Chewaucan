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
            camLook.setMinDist(10f);
            other.gameObject.transform.position = new Vector3(other.gameObject.transform.position.x, 
                waterBlock.position.y, other.gameObject.transform.position.z);
            playerMovement.SetWaterSoundSource(this.GetComponent<AudioSource>());
            //Debug.Log("IN WATER");
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            PlayerMovement playerMovement = other.gameObject.GetComponent<PlayerMovement>();
            playerMovement.setSwimming(true, waterBlock.position);
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
            camLook.setMinDist(30f);
            //Debug.Log("ON LAND");
        }
    }
}
