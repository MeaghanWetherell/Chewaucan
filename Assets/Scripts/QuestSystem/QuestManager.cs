using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Misc;
using UnityEditor.Experimental.GraphView;


namespace QuestSystem
{
    //Singleton manager object handling quest nodes
    public class QuestManager : MonoBehaviour
    {
        public static QuestManager questManager;

        [Tooltip("Testing flag. Enable this to disable loading saved quest data.")]
        public bool resetQuests;
        
        //quests the player has received
        private List<QuestNode> _quests = new List<QuestNode>();

        //the three quests currently pinned. if we want more than 3 the hud will need refactoring
        //but nothing else will need to be touched here
        private QuestNode[] _pins = new QuestNode[3];
        
        //if the passed node is new, create it and return it,
        //otherwise return a reference to the existing quest
        public QuestNode CreateQuestNode(QuestObj obj)
        {
            QuestNode node = GETNode(obj.uniqueID);
            if (node != null)
                return node;
            node = new QuestNode(obj);
            return node;
        }

        //use to get specific nodes from the manager. 
        public QuestNode GETNode(string id)
        {
            foreach(QuestNode node in _quests)
            {
                if (node.id.Equals(id))
                {
                    return node;
                }
            }

            return null;
        }

        private void Awake()
        {
            if (questManager != null)
            {
                Debug.LogError("Loaded persistent objects twice!");
                Destroy(questManager.gameObject);
            }
            questManager = this;
            DontDestroyOnLoad(transform.gameObject);
            if (!resetQuests)
            {
                LoadFromFile();
            }
        }

        public List<QuestNode> GETQuests()
        {
            return _quests;
        }

        private void LoadFromFile()
        {
            string allQuestFiles = "";
            string line = "";
            StreamReader streamReader;
            try
            {
                streamReader = new StreamReader("Saves/SavedQuests.txt");
                line = streamReader.ReadLine();
                if (line != null)
                {
                    allQuestFiles += line;
                }

                line = streamReader.ReadLine();
                while (line != null)
                {
                    allQuestFiles += " " + line;
                    line = streamReader.ReadLine();
                }

                streamReader.Close();
            }
            catch (Exception)
            { return; }
            if (allQuestFiles.Equals(""))
                return;
            string[] files = allQuestFiles.Split(" ");
            foreach (string fileName in files)
            {
                try
                {
                    streamReader = new StreamReader("Saves/"+fileName);
                    line = streamReader.ReadLine();
                    streamReader.Close();
                }
                catch (Exception)
                { Debug.LogError("Error in quest json deserializer");}
                QuestNode quest = JsonUtility.FromJson<QuestNode>(line);
                quest.callOnceInitialized();
                if (quest.isPinned)
                {
                    AddPin(quest);
                }
            }
        }

        private void OnDisable()
        {
            SerializeToJson();
        }

        //save the quest data
        private void SerializeToJson()
        {
            string allSavedQuests = "";
            Directory.CreateDirectory("Saves");
            foreach (QuestNode quest in _quests)
            {
                string questJson = JsonUtility.ToJson(quest);
                File.WriteAllText("Saves/"+quest.id+".json", questJson);
                if (!allSavedQuests.Equals(""))
                    allSavedQuests += " ";
                allSavedQuests += quest.id + ".json";
            }
            File.WriteAllText("Saves/SavedQuests.txt", allSavedQuests);
        }

        //register a new quest node with the manager. automatically called by new nodes
        //will not add duplicate quests. Make sure to use getNode to save node references,
        //otherwise you could end up with a reference to a duplicate node that is registered with the manager
        public bool RegisterNode(QuestNode toRegister)
        {
            foreach(QuestNode quest in _quests)
            {
                if (quest.name.Equals(toRegister.name))
                {
                    return false;
                }
                if (quest.id.Equals(toRegister.id))
                {
                    Debug.LogError("Quests "+quest.name+" and "+toRegister.name+" have same ID!");
                }
            }
            _quests.Add(toRegister);
            _quests.InsertionSort();
            return true;
        }

        //self report quest completion to the manager.
        //nodes handle this, shouldn't be called externally
        public void ReportCompletion(bool wasPinned=false, QuestNode pinNode=null)
        {
            _quests.InsertionSort();
            if (wasPinned)
            {
                RemovePin(pinNode);
                HUDManager.hudManager.ResetPins();
            }
        }

        //returns a deep copy of the pin array
        public QuestNode[] GETPinNodes()
        {
            QuestNode[] temp = new QuestNode[_pins.Length];
            for (int i = 0; i < _pins.Length; i++)
            {
                temp[i] = _pins[i];
            }
            return temp;
        }

        //adds a quest to the pins
        public void AddPin(QuestNode node)
        {
            if (node.isComplete)
                return;
            for (int i = 0; i < _pins.Length; i++)
            {
                if (_pins[i] == null)
                {
                    _pins[i] = node;
                    return;
                }
            }
            _pins[0].ChangePinned();
            _pins[^1] = node;
        }

        //removes a quest from the pins
        public void RemovePin(QuestNode pin)
        {
            for (int i = 0; i < _pins.Length; i++)
            {
                if (_pins[i] == null)
                {
                    break;
                }
                if (_pins[i].CompareTo(pin) == 0)
                {
                    for (int j = i; j < _pins.Length-1; j++)
                    {
                        _pins[j] = _pins[j+1];
                    }
                    _pins[^1] = null;
                    return;
                }
            }
            Debug.LogError("Attempted to remove unpinned quest from pins");
        }
    }
}
