using System;
using System.Collections;
using System.Collections.Generic;
using QuestSystem;
using ScriptTags;
using UnityEngine;
using UnityEngine.Playables;

public class StartCutsceneOnTriggerEnter : MonoBehaviour
{
    public PlayableDirector cutscene;

    [Tooltip("Quest to start when this cutscene finishes")]
    public QuestObj startQuest;

    [Tooltip("Id of the quest to update when this cutscene finishes")]
    public string questToUpdate;

    [Tooltip("Name of the update to send")]
    public string updateName;


    private void Awake()
    {
        if(!checkPlayable())
            Destroy(gameObject);
    }

    private bool checkPlayable()
    {
        if (startQuest != null && QuestManager.questManager.GETNode(startQuest.uniqueID) != null)
            return false;
        if (questToUpdate != null)
        {
            QuestNode node = QuestManager.questManager.GETNode(questToUpdate);
            if (node != null && node.isUpdateUnlocked(updateName))
            {
                return false;
            }
        }
        return true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Player>() != null && checkPlayable())
        {
            if (questToUpdate != null && !questToUpdate.Trim().Equals(""))
            {
                QuestNode node = QuestManager.questManager.GETNode(questToUpdate);
                if (node == null) return;
            }
            cutscene.Play();
        }
    }

    public void OnCutsceneEnd()
    {
        if (startQuest != null)
            QuestManager.questManager.CreateQuestNode(startQuest);
        if (questToUpdate != null)
        {
            QuestNode node = QuestManager.questManager.GETNode(questToUpdate);
            if (node != null)
                node.UnlockUpdate(updateName);
        }
        Destroy(gameObject);
    }
}
