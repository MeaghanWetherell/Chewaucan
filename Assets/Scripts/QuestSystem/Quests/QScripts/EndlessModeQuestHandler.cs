using System;
using System.Collections.Generic;
using Match3.DataClasses;
using UnityEngine;

namespace QuestSystem.Quests.QScripts
{
    public class EndlessModeQuestHandler : MonoBehaviour
    {
        [Tooltip("Quest id and objectives MUST have specific names. See comment in this class for details.")]
        //Quest ids MUST BE IDENTICAL TO THE NAME OF THE ANIMAL THEY CORRESPOND TO PLUS THE WORD 'endless'
        //Objectives must be named exactly after the bone they correspond to
        public List<QuestObj> quests;

        //get the number of bones required for completion of the quest associated with the passed data
        //return -1 if no such quest
        public static int GetCountRequired(MeshDataObj data)
        {
            QuestNode node = QuestManager.questManager.GETNode(data.animal+"endless");
            if (node == null)
                return -1;
            return (int) node.requiredCounts[0];
        }
        
        //progress endless mode bone match quest
        public static void Progress(MeshDataObj data)
        {
            QuestNode node = QuestManager.questManager.GETNode(data.animal+"endless");
            if (node == null || node.isComplete)
                return;
            int index = 0;
            while (!node.objectives[index].Equals(data.boneName))
            {
                index++;
                if (index == node.objectives.Count)
                {
                    Debug.LogError("ERR: bone not included in animal's quest");
                    return;
                }
            }
            node.AddCount(index);
        }

        public void OnClick()
        {
            foreach (QuestObj qObj in quests)
                QuestManager.questManager.CreateQuestNode(qObj);
        }
    }
}
