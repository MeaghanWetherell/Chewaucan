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

    public SnakeMove move;

    public bool isMovingSnake;

    public AudioSource rattle;
    /*
    [Tooltip("The max distance at which the snake should look for the player for rotation")]
    public float dist;
    */

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Player>() != null && enabled)
        {
            rattle.Play();
            move.enabled = false;
            transform.parent.GetComponentInChildren<Animator>().SetBool("Rattle", true);
            StartCoroutine(rotateTowards());
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<Player>())
        {
            rattle.Stop();
            transform.parent.GetComponentInChildren<Animator>().SetBool("Rattle", false);
            move.enabled = isMovingSnake;
            StopAllCoroutines();
        }
    }

    private void OnDisable()
    {
        rattle.Stop();
        transform.parent.GetComponentInChildren<Animator>().SetBool("Rattle", false);
        StopAllCoroutines();
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
                //if this becomes a performance problem use a layer mask instead
                if (hit.collider == null || hit.collider.GetComponent<Player>() == null)
                {
                    Vector3 dir = player.position - rotTarg.position;
                    dir = Vector3.RotateTowards(rotTarg.forward, dir, rotSpeed * 0.02f, 0);
                    rotTarg.rotation = Quaternion.LookRotation(new Vector3(dir.x, 0, dir.z));
                    
                }
                
            }
            yield return new WaitForSeconds(0.02f);
        }
    }
}
