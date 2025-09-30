using System;
using System.Collections;
using System.Collections.Generic;
using ScriptTags;
using UnityEngine;

public class StartCourseOnTriggerEnter : MonoBehaviour
{
    private CourseManager manager;

    private void Awake()
    {
        manager = transform.parent.GetComponent<CourseManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Player>() != null)
        {
            if (manager.active)
            {
                manager.Reset("You went out of bounds!");
            }
            else
            {
                manager.StartCourse();
            }
        }
    }
}
