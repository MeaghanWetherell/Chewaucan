using System.Collections.Generic;
using UnityEngine;
using Misc;
using UnityEditor.Experimental.GraphView;


namespace QuestSystem
{
    //Singleton manager object handling quest nodes
    public class QuestManager : MonoBehaviour
    {
        public static QuestManager questManager;
        
        //quests the player has received
        private List<QuestNode> quests = new List<QuestNode>();

        //the three quests currently pinned. if we want more than 3 the hud will need refactoring
        //but nothing else will need to be touched here
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

        //if the passed node is new, create it and return it,
        //otherwise return a reference to the existing quest
        public QuestNode createQuestNode(QuestObj obj)
        {
            QuestNode node = getNode(obj.uniqueID);
            if (node != null)
                return node;
            node = new QuestNode(obj);
            return node;
        }

        //use to get specific nodes from the manager. 
        public QuestNode getNode(string ID)
        {
            foreach(QuestNode node in quests)
            {
                if (node.ID == ID)
                {
                    return node;
                }
            }

            return null;
        }
        
        //register a new quest node with the manager. automatically called by new nodes
        //will not add duplicate quests. Make sure to use getNode to save node references,
        //otherwise you could end up with a reference to a duplicate node that is registered with the manager
        public bool registerNode(QuestNode toRegister)
        {
            foreach(QuestNode quest in quests)
            {
                if (quest.name.Equals(toRegister.name))
                {
                    return false;
                }
                if (quest.ID.Equals(toRegister.ID))
                {
                    Debug.LogError("Quests "+quest.name+" and "+toRegister.name+" have same ID!");
                }
            }
            quests.Add(toRegister);
            quests.insertionSort();
            return true;
        }

        //self report quest completion to the manager.
        //nodes handle this, shouldn't be called externally
        public void reportCompletion(bool wasPinned=false, QuestNode pinNode=null)
        {
            quests.insertionSort();
            if (wasPinned)
            {
                RemovePin(pinNode);
                HUDManager.hudManager.resetPins();
            }
        }

        //returns a deep copy of the pin array
        public QuestNode[] getPinNodes()
        {
            QuestNode[] temp = new QuestNode[pins.Length];
            for (int i = 0; i < pins.Length; i++)
            {
                temp[i] = pins[i];
            }
            return temp;
        }

        //adds a quest to the pins
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

        //removes a quest from the pins
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
    }
}
