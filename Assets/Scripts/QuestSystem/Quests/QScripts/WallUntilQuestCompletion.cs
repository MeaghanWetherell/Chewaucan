using System;
using System.Collections;
using System.Collections.Generic;
using QuestSystem;
using UnityEngine;

//destroys this object if the associated quest is complete
public class WallUntilQuestCompletion : MonoBehaviour
{
    public string questID;
    
    private void Start()
    {
        QuestNode quest = QuestManager.questManager.GETNode(questID);
        if (quest.isComplete)
        {
            Destroy(gameObject);
        }
        else
        {
            quest.OnComplete.AddListener(QuestComplete);
        }
    }

    private void QuestComplete(string id)
    {
        Destroy(gameObject);
    }
}
