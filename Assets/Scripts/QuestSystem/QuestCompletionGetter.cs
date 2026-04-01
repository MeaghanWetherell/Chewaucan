using System.Collections;
using System.Collections.Generic;
using QuestSystem;
using UnityEngine;

[CreateAssetMenu]
//helper object for checking if certain quests/updates have been achieved
public class QuestCompletionGetter : ScriptableObject
{
    [Tooltip("Ids of quests that must be complete to return true")]
    public List<string> completedQIds;

    [Tooltip("Ids of the quests that need certain updates completed to return true. duplicate the ids of quests which need multiple updates completed")]
    public List<string> updatedQids;

    [Tooltip("names of the update that needs to be completed, mapped by index to the above list")]
    public List<string> updateName;

    public bool isComplete()
    {
        foreach (string id in completedQIds)
        {
            QuestNode node = QuestManager.questManager.GETNode(id);
            if (node == null || !node.isComplete)
                return false;
        }

        for (int i = 0; i < updatedQids.Count; i++)
        {
            string id = updatedQids[i];
            QuestNode node = QuestManager.questManager.GETNode(id);
            if (node == null || !node.isUpdateUnlocked(updateName[i]))
            {
                return false;
            }
        }
        return true;
    }
}
