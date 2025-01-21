using System;
using System.Collections;
using System.Collections.Generic;
using ScriptTags;
using UnityEngine;

public class Climbable : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Player>() != null)
        {
            Quaternion targRot;
            if (transform.parent != null) targRot = transform.parent.rotation;
            else targRot = transform.rotation;
            other.GetComponent<PlayerMovementController>().SwitchToClimbing(targRot, GetComponent<Collider>());
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<Player>() != null)
        {
            other.GetComponent<PlayerMovementController>().SwitchToWalking();
        }
    }
}
