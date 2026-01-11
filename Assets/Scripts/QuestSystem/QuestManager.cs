using System;
using System.Collections.Generic;
using System.IO;
using LoadGUIFolder;
using UnityEngine;
using Misc;
using ScriptTags;
using UnityEngine.Events;


namespace QuestSystem
{
    //Singleton manager object handling quest nodes
    public class QuestManager : MonoBehaviour
    {
        public static QuestManager questManager;

        public GameObject questUpdatePopUp;
        
        public static bool resetQuests = false;

        public UnityEvent<QuestNode> onQuestCreated;
        
        [Tooltip("List of all quests")] 
        public List<QuestObj> AllQuests;

        //order is archaeology, geology, biology
        public int[] CountsPerQuestType = new int[3];
        
        //quests the player has received
        private List<QuestNode> _quests = new List<QuestNode>();

        //the three quests currently pinned. if we want more than 3 the hud will need refactoring
        //but nothing else will need to be touched here
        private QuestNode[] _pins = new QuestNode[3];
        
        //if the passed node is new, create it and return it,
        //otherwise return a reference to the existing quest
        public QuestNode CreateQuestNode(QuestObj obj)
        {
            QuestNode node = GETNode(obj.uniqueID.ToLower());
            if (node != null)
                return node;
            node = new QuestNode(obj);
            if(node.startNarration != null)
                node.startNarration.Begin();
            if (obj.initFile != null)
            {
                GameObject qNode = Instantiate(questUpdatePopUp);
                LoadGUIManager.loadGUIManager.InstantiatePopUp(qNode, node.name, obj.initFile.text);
            }
            onQuestCreated.Invoke(node);
            return node;
        }

        public bool hasQuests()
        {
            return _quests.Count > 0;
        }

        //get the quest object corresponding to the passed id
        //or null if it does not exist
        public QuestObj getQuest(string qid)
        {
            foreach (QuestObj quest in AllQuests)
            {
                if (quest.uniqueID.Equals(qid))
                {
                    return quest;
                }
            }

            return null;
        }
        
        //if the passed quest is new, create it and return it,
        //otherwise return a reference to the existing quest
        public QuestNode CreateQuestNode(string qid)
        {
            QuestObj obj = getQuest(qid);
            if (obj == null) return null;
            QuestNode node = GETNode(obj.uniqueID.ToLower());
            if (node != null)
                return node;
            node = new QuestNode(obj);
            if(node.startNarration != null)
                node.startNarration.Begin();
            if (obj.initFile != null)
            {
                GameObject qNode = Instantiate(questUpdatePopUp);
                LoadGUIManager.loadGUIManager.InstantiatePopUp(qNode, node.name, obj.initFile.text);
            }
            onQuestCreated.Invoke(node);
            return node;
        }

        //use to get specific nodes from the manager. 
        public QuestNode GETNode(string id)
        {
            id = id.ToLower();
            foreach(QuestNode node in _quests)
            {
                if (node.id.Equals(id))
                {
                    return node;
                }
            }
            return null;
        }
        
        //use to subscribe to completion events for a quest
        public bool SubToCompletion(string id, UnityAction<string> func)
        {
            id = id.ToLower();
            QuestNode node = GETNode(id);
            if (node != null)
            {
                node.OnComplete.AddListener(func);
                return true;
            }
            return false;
        }
        
        //use to unsubscribe to completion events for a quest
        public bool UnsubToCompletion(string id, UnityAction<string> func)
        {
            QuestNode node = GETNode(id);
            if (node != null)
            {
                node.OnComplete.RemoveListener(func);
                return true;
            }
            return false;
        }

        private void Awake()
        {
            if (questManager != null)
            {
                Destroy(gameObject);
                return;
            }
            questManager = this;
            DontDestroyOnLoad(transform.gameObject);
            
            SaveHandler.saveHandler.subToLoad(LoadFromFile);
            SaveHandler.saveHandler.subToSave(SerializeToJson);
        }

        public List<QuestNode> GETQuests()
        {
            return _quests;
        }

        private void LoadFromFile(string path)
        {
            CountsPerQuestType = new int[3];
            foreach (QuestObj quest in AllQuests)
            {
                if(quest.type != SaveDialProgressData.Dial.NONE)
                    CountsPerQuestType[(int) quest.type]++;
            }
            SaveDialProgressData.saveDataPath = path + "/DialProgess.json";
            SaveDialProgressData.archeologyQuestNum = CountsPerQuestType[0];
            SaveDialProgressData.biologyQuestNum = CountsPerQuestType[1];
            SaveDialProgressData.geologyQuestNum = CountsPerQuestType[2];
            _quests = new List<QuestNode>();
            _pins = new QuestNode[3];
            if(!resetQuests)
            {
                string allQuestFiles = "";
                string line = "";
                StreamReader streamReader;
                try
                {
                    streamReader = new StreamReader(path+"/SavedQuests.txt");
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
                        streamReader = new StreamReader(path+"/"+fileName);
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
        }

        //save the quest data
        private void SerializeToJson(string path)
        {
            string allSavedQuests = "";
            foreach (QuestNode quest in _quests)
            {
                string questJson = JsonUtility.ToJson(quest);
                File.WriteAllText(path+"/"+quest.id+".json", questJson);
                if (!allSavedQuests.Equals(""))
                    allSavedQuests += " ";
                allSavedQuests += quest.id + ".json";
            }
            File.WriteAllText(path+"/SavedQuests.txt", allSavedQuests);
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
        public void ReportCompletion(QuestNode node, bool wasPinned=false)
        {
            if(node.completionNarration != null)
                node.completionNarration.Begin();
            _quests.InsertionSort();
            GameObject qNode = Instantiate(questUpdatePopUp);
            LoadGUIManager.loadGUIManager.InstantiatePopUp(qNode, node.name, node.compText);
            SaveDialProgressData.CompleteOneQuest(node.type);
            foreach (string wp in node.WPUnlockIDs)
            {
                WPUnlockSerializer.wpUnlockSerializer.Unlock(wp);
            }
            if (wasPinned)
            {
                RemovePin(node);
                if(HUDManager.hudManager != null)
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
