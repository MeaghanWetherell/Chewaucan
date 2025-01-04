using System;
using System.Collections;
using System.Collections.Generic;
using ScriptTags;
using UnityEngine;

public class CourseWall : MonoBehaviour
{
    public CourseManager manager;

    private bool inited = false;

    private void Start()
    {
        if (!inited)
        {
            manager.Started.AddListener(OnStart);
            manager.Stopped.AddListener(OnStop);
            inited = true;
            transform.parent.gameObject.SetActive(false);
        }
    }

    private void OnStart()
    {
        transform.parent.gameObject.SetActive(true);
    }

    private void OnStop()
    {
        transform.parent.gameObject.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Player>() != null)
        {
            manager.Reset();
        }
    }
}
