using System;
using System.Collections;
using System.Collections.Generic;
using QuestSystem;
using QuestSystem.Quests.QScripts;
using UnityEngine;

//progress quests when the player wins the plateau game
public class PlateauQuestManager : MonoBehaviour
{
    public void Start()
    {
        CourseManager.win.AddListener(OnCourseWin);
    }

    private void OnCourseWin(int level)
    {
       // QuestManager.questManager.GETNode("MainQuest").UnlockUpdate(2);
        if (level == 0)
        {
            Debug.Log("This should turn on the astrolabe rn.");
            QuestManager.questManager.GETNode("PlateauQuest").AddCount(0);
            SubAstrolabeTeleport.subAstrolabeTeleport.CheckForNewDest();
            QuestManager.questManager.GETNode("MainQuest").UnlockUpdate(1, false); //update main quest, no popup
        }
    }
}
