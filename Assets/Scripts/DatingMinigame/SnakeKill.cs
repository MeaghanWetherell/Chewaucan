using System;
using System.Collections;
using System.Collections.Generic;
using ScriptTags;
using UnityEngine;

public class SnakeKill : MonoBehaviour
{
    public CourseManager manager;

    public AudioSource bite;
    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Player>() != null)
        {
            StartCoroutine(LoseCoroutine());
        }
    }

    private IEnumerator LoseCoroutine()
    {
        bite.Play();
        yield return new WaitForSeconds(0.2f);
        manager.Reset("You were bitten by a rattlesnake! Watch out!");
    }
}
