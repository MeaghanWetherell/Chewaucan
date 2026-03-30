using System;
using System.Collections;
using System.Collections.Generic;
using QuestSystem;
using UnityEngine;

//destroys this object if the associated quests are complete
public class WallUntilQuestCompletion : MonoBehaviour
{
    [Tooltip("Ids of the quests that need to be completed to destroy this wall")]
    public List<string> compIds;
    
    private void OnEnable()
    {
        
        if (CheckQuestsComplete())
        {
            Destroy(gameObject);
        }
        else
        {
            QuestManager.questManager.onQuestCreated.AddListener(OnQuestCreated);
            foreach (string id in compIds)
            {
                QuestNode node = QuestManager.questManager.GETNode(id);
                if (node != null)
                {
                    node.OnComplete.AddListener(QuestComplete);
                }
            }
        }
    }

    private void OnDisable()
    {
        QuestManager.questManager.onQuestCreated.RemoveListener(OnQuestCreated);
        foreach (string id in compIds)
        {
            QuestNode node = QuestManager.questManager.GETNode(id);
            if (node != null)
            {
                node.OnComplete.RemoveListener(QuestComplete);
            }
        }
    }

    private void OnQuestCreated(QuestNode node)
    {
        foreach (string id in compIds)
        {
            if (id.Equals(node.id))
            {
                node.OnComplete.AddListener(QuestComplete);
            }
        }
    }

    private bool CheckQuestsComplete()
    {
        foreach (string id in compIds)
        {
            QuestNode node = QuestManager.questManager.GETNode(id);
            if (node == null || !node.isComplete)
            {
                return false;
            }
        }

        return true;
    }

    private void QuestComplete(string id)
    {
        if(CheckQuestsComplete())
            Destroy(gameObject);
    }
}
