using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestUpdateOnTriggerEnter : MonoBehaviour
{
    [Tooltip("ID of the quest to update")]
    public string qid;

    [Tooltip("Index of the update to provide, by default (-1) unlocks the next update in order")]
    public int updateNum = -1;

    private void OnTriggerEnter(Collider other)
    {
        QuestSystem.QuestManager.questManager.GETNode(qid).UnlockUpdate();
    }
}
