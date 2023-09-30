using System.Collections.Generic;
using UnityEngine;
using Misc;


namespace QuestSystem
{
    public class QuestManager : MonoBehaviour
    {
        public static QuestManager questManager;
        
        private List<QuestNode> quests = new List<QuestNode>();

        private void Awake()
        {
            questManager = this;
        }
        
        public void registerNode(QuestNode toRegister)
        {
            foreach(QuestNode quest in quests)
            {
                if (quest.name.Equals(toRegister.name))
                {
                    return;
                }
            }
            quests.Add(toRegister);
            quests.insertionSort();
        }

        public void reportCompletion()
        {
            quests.insertionSort();
        }
        
        
    }
}
