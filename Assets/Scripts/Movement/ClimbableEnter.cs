using System;
using System.Collections;
using System.Collections.Generic;
using ScriptTags;
using UnityEngine;

public class ClimbableEnter : MonoBehaviour
{
    public BoxCollider exitCollider;
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Player>() != null)
        {
            other.GetComponent<PlayerMovementController>().SwitchToClimbing(exitCollider);
        }
    }
}
