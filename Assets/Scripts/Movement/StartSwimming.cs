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
            playerMovement.setSwimming(true, this.transform.position);
            //Debug.Log("IN WATER");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            PlayerMovement playerMovement = other.gameObject.GetComponent<PlayerMovement>();
            playerMovement.setSwimming(false, this.transform.position);
            //Debug.Log("ON LAND");
        }
    }
}
