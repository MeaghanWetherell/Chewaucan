using System;
using System.Collections;
using System.Collections.Generic;
using QuestSystem;
using ScriptTags;
using UnityEngine;

public class StartCourseOnTriggerEnter : MonoBehaviour
{
    private CourseManager manager;

    public Canvas canv;

    private void Awake()
    {
        manager = transform.parent.GetComponent<CourseManager>();
        manager.Stopped.AddListener(Show);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Player>() != null)
        {
            if (manager.active)
            {
                //manager.Reset("You went out of bounds!");
            }
            else
            {
                canv.gameObject.SetActive(false);
                manager.StartCourse();
                QuestManager.questManager.GETNode("PlateauQuest").UnlockUpdate(0);
            }
        }
    }

    private void Show()
    {
        canv.gameObject.SetActive(true);
    }
}
