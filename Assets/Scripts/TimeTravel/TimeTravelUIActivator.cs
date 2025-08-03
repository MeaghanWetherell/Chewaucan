using System;
using System.Collections;
using System.Collections.Generic;
using QuestSystem;
using UnityEngine;
using UnityEngine.Events;

public class TimeTravelUIActivator : MonoBehaviour
{
    private void Start()
    {
        StartCoroutine(waitForQuestLoad());
    }

    private IEnumerator waitForQuestLoad()
    {
        while (QuestManager.questManager == null)
            yield return new WaitForSeconds(0);
        QuestNode unlockQuest = QuestManager.questManager.GETNode("bonepile");
        if (unlockQuest == null || !unlockQuest.isComplete)
        {
            if (unlockQuest != null)
            {
                QuestManager.questManager.SubToCompletion("bonepile", toSub => { gameObject.SetActive(true); });
            }
            gameObject.SetActive(false);
        }
        
    }
}
