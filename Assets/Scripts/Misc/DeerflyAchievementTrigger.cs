using System;
using System.Collections;
using System.Collections.Generic;
using QuestSystem;
using UnityEngine;

public class DeerflyAchievementTrigger : MonoBehaviour
{
    public Narration.Narration deerflyNarration;

    public string natLogID = "naturelog";

    public string deerflyUpdateName = "Deerflies";
    
    private void Awake()
    {
        if (DeerflySwarmTrigger.swarmed && !deerflyNarration.HasPlayed())
        {
            DeerflySwarmTrigger.swarmed = false;
            deerflyNarration.Begin();
            SteamAPIManager.UnlockAch("DeerflyAchievement");
            QuestNode natLog = QuestManager.questManager.GETNode(natLogID);
            natLog.UnlockUpdate(deerflyUpdateName);
            natLog.AddCount(0);
        }
    }
}
