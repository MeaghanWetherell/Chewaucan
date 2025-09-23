using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ScriptTags;

public class MammothRunOnTriggerEnter : MonoBehaviour
{
    public Rigidbody mammothBody;

    private bool run = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Player>() != null)
        {
            Run();
        }
    }

    private void Run()
    {
        run = true;
    }

    private void FixedUpdate()
    {
        if (run && mammothBody.velocity.x < 150)
        {
            mammothBody.velocity += new Vector3(100, 0, 0) * Time.fixedDeltaTime;
            //Debug.Log("More Move");
        }

        //Debug.Log(mammothBody.velocity);
    }
}
