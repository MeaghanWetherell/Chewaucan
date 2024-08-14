using System;
using System.Collections;
using System.Collections.Generic;
using QuestSystem;
using UnityEngine;

public class TimeTravelUIActivator : MonoBehaviour
{
    private void OnEnable()
    {
        StartCoroutine(waitForQuestLoad());
    }

    private IEnumerator waitForQuestLoad()
    {
        while (QuestManager.questManager == null)
            yield return new WaitForSeconds(0);
        QuestNode demoQ = QuestManager.questManager.GETNode("matchdemo");
        if (demoQ == null || !demoQ.isComplete)
        {
            gameObject.SetActive(false);
        }
    }
}
