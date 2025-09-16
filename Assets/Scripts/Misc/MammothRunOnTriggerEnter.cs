using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ScriptTags;

public class MammothRunOnTriggerEnter : MonoBehaviour
{
    public Rigidbody mammothBody;

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Player>() != null)
        {
            Run();
        }
    }

    private void Run()
    {
        mammothBody.velocity = new Vector3(5000, 0, 0);
    }
}
