using System;
using System.Collections;
using System.Collections.Generic;
using Misc;
using ScriptTags;
using UnityEngine;

public class SnakeRotate : MonoBehaviour
{
    [Tooltip("Angular speed, in radians per second")]
    public float rotSpeed;

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Player>() != null)
        {
            StartCoroutine(rotateTowards());
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<Player>())
        {
            StopAllCoroutines();
        }
    }

    private IEnumerator rotateTowards()
    {
        Transform player = Player.player.transform;
        Transform rotTarg = transform.parent;
        while (true)
        {
            if (!PauseCallback.pauseManager.isPaused)
            {
                RaycastHit hit;
                Physics.Raycast(rotTarg.position, rotTarg.forward, out hit);
                Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * hit.distance, Color.yellow); 
                //if this becomes a performance problem use a layer mask instead
                if (hit.collider == null || hit.collider.GetComponent<Player>() == null)
                {
                    Vector3 dir = player.position - rotTarg.position;
                    dir = Vector3.RotateTowards(rotTarg.forward, dir, rotSpeed * 0.02f, 0);
                    rotTarg.rotation = Quaternion.LookRotation(dir);
                    
                }
                
            }
            yield return new WaitForSeconds(0.02f);
        }
    }
}
