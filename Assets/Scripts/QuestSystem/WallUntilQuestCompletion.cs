using QuestSystem;
using ScriptTags;
using System;
using System.Collections;
using System.Collections.Generic;
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
            // this can fire as early as OnEnable() on a freshly-loaded scene, before
            // Player.player has necessarily been assigned. UnlockUpdate() below can
            // synchronously create a popup that reaches into Player.player - if that's
            // still null (or stale from the previous scene), the popup silently fails
            // to pause/cache the new CameraLook, leaving the camera stuck disabled
            // later when PauseCallback resumes. Wait until the player exists first.
            StartCoroutine(DestroyAndUnlockWhenPlayerReady());
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

    private IEnumerator DestroyAndUnlockWhenPlayerReady()
    {
        yield return new WaitUntil(() => Player.player != null);

        Destroy(gameObject);

        if (QuestManager.questManager.GETNode("MainQuest").isUpdateUnlocked(2))
        {
            //Debug.Log("You have unlocked this already");
        }
        else
        {
            QuestManager.questManager.GETNode("MainQuest").UnlockUpdate(2); //moved on 8-25-2026
            //print(QuestManager.questManager.GETNode("MainQuest").isUpdateUnlocked(2));
            //Debug.Log("You've unlocked it");
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
