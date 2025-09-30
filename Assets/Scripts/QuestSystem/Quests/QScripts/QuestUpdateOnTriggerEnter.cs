using System;
using System.Collections;
using System.Collections.Generic;
using ScriptTags;
using UnityEngine;

public class QuestUpdateOnTriggerEnter : MonoBehaviour
{
    [Tooltip("ID of the quest to update")]
    public string qid;

    [Tooltip("Index of the update to provide")]
    public int updateNum = 0;

    private void OnTriggerEnter(Collider other)
    {
        if(other.GetComponent<Player>() != null)
            QuestSystem.QuestManager.questManager.GETNode(qid).UnlockUpdate(updateNum);
    }
}
