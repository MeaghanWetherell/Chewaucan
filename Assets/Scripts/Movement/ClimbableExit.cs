using System;
using System.Collections;
using System.Collections.Generic;
using ScriptTags;
using UnityEngine;

public class ClimbableExit : MonoBehaviour
{

    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<Player>() != null)
        {
            other.GetComponent<PlayerMovementController>().SwitchToWalking();
        }
    }
}
