using System;
using System.Collections;
using System.Collections.Generic;
using QuestSystem;
using UnityEngine;

public class PlateauQuestManager : MonoBehaviour
{
    public void Start()
    {
        CourseManager.win.AddListener(OnCourseWin);
    }

    private void OnCourseWin(int level)
    {
        QuestManager.questManager.GETNode("MainQuest").UnlockUpdate(2);
        if (level == 0)
        {
            QuestManager.questManager.GETNode("PlateauQuest").AddCount(0);
        }
    }
}
