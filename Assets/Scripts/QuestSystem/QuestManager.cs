using System.Collections.Generic;
using UnityEngine;
using Misc;


namespace QuestSystem
{
    public class QuestManager : MonoBehaviour
    {
        public static QuestManager questManager;
        
        private List<QuestNode> quests = new List<QuestNode>();

        private QuestNode[] pins = new QuestNode[3];

        private void Awake()
        {
            questManager = this;
            DontDestroyOnLoad(transform.gameObject);
        }

        public List<QuestNode> getQuests()
        {
            return quests;
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

        public void reportCompletion(bool wasPinned=false, QuestNode pinNode=null)
        {
            quests.insertionSort();
            if (wasPinned)
            {
                RemovePin(pinNode);
                HUDManager.hudManager.resetPins();
            }
        }

        public QuestNode[] getPinNodes()
        {
            QuestNode[] temp = new QuestNode[pins.Length];
            for (int i = 0; i < pins.Length; i++)
            {
                temp[i] = pins[i];
            }
            return temp;
        }

        public void AddPin(QuestNode node)
        {
            if (node.isComplete)
                return;
            for (int i = 0; i < pins.Length; i++)
            {
                if (pins[i] == null)
                {
                    pins[i] = node;
                    return;
                }
            }
            pins[0].changePinned();
            pins[^1] = node;
        }

        public void RemovePin(QuestNode pin)
        {
            for (int i = 0; i < pins.Length; i++)
            {
                if (pins[i] == null)
                {
                    break;
                }
                if (pins[i].CompareTo(pin) == 0)
                {
                    for (int j = i; j < pins.Length-1; j++)
                    {
                        pins[j] = pins[j+1];
                    }
                    pins[^1] = null;
                    return;
                }
            }
            Debug.LogError("Attempted to remove unpinned quest from pins");
        }

        private void RemovePin()
        {
            for (int j = 0; j < pins.Length-1; j++)
            {
                pins[j] = pins[j+1];
            }
            pins[^1] = null;
        }
    }
}
