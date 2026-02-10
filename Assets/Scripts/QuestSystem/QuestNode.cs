using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using LoadGUIFolder;
using Misc;
using UnityEngine;
using UnityEngine.Events;

namespace QuestSystem
{
    //object storing data about a particular quest
    [Serializable]
    public class QuestNode : IComparable<QuestNode>
    {
        //Name of the associated quest object
        public string QObjName;
        
        //initialization narration
        [NonSerialized]public Narration.Narration startNarration;
        
        //completion narration
        [NonSerialized]public Narration.Narration completionNarration;
        
        //name of the quest, used as an identifier, should ensure unique names
        [NonSerialized]public string name;

        //unique id of the quest
        [NonSerialized]public string id;

        //short description of the quest
        [NonSerialized]public string shortDescription;

        //long description of the quest
        [NonSerialized]public string longDescription;
        
        //title for each update
        [NonSerialized]public List<string> qUpdateTitles;
        
        //quest updates
        [NonSerialized]public List<string> qUpdates;

        public List<bool> updateUnlocks;

        //completion text
        [NonSerialized]public string compText;

        //list of the objectives to be completed
        [NonSerialized]public List<string> objectives;

        //list of the required count per objective mapped by index
        [NonSerialized]public List<float> requiredCounts;

        //current count per objective mapped by index
        public List<float> counts = new List<float>();

        //default count increment for each objective, mapped by index.
        //initialized to 1 if a value is not received
        [NonSerialized]public List<float> countsPerAction;

        //quest type: Archaeology, Biology, or Geology
        [NonSerialized]public SaveDialProgressData.Dial type;

        [NonSerialized]public UnityEvent<string> OnComplete = new UnityEvent<string>();

        [NonSerialized]public List<string> WPUnlockIDs = new List<string>();

        public bool isComplete = false;

        public bool isPinned = false;

        public static GameObject qUpdatePopUp;

        public bool isMainQuest = false;

        //adds the passed count to the count for the objective at index
        //returns true if the quest is complete, false otherwise
        public bool AddCount(int index, float toAdd = 0.0f)
        {
            if (isComplete)
            {
                return true;
            }

            if (toAdd == 0)
            {
                toAdd = countsPerAction[index];
            }

            counts[index] += toAdd;
            if (counts[index] >= requiredCounts[index])
            {
                counts[index] = requiredCounts[index];
                for (int i = 0; i < requiredCounts.Count; i++)
                {
                    if (counts[i] < requiredCounts[i] )
                    {
                        if(HUDManager.hudManager != null)
                            HUDManager.hudManager.ResetPins();
                        return false;
                    }
                    
                }

                isComplete = true;
                if (isPinned)
                {
                    isPinned = false;
                    QuestManager.questManager.ReportCompletion(this, true);
                }
                else
                {
                    QuestManager.questManager.ReportCompletion(this);
                }
                OnComplete.Invoke(id);
                return true;
            }

            if (isPinned)
            {
                HUDManager.hudManager.ResetPins();
            }

            return false;
        }
        
        //Create a popup for a quest update
        private void CreateUpdatePopUP(int index)
        {
            if (qUpdatePopUp == null)
                qUpdatePopUp = Resources.Load<GameObject>("QuestUpdatePopUp");
            LoadGUIManager.loadGUIManager.InstantiatePopUp(qUpdatePopUp, qUpdateTitles[index], qUpdates[index], null, true);
        }
        
        //Unlocks the update with the passed title
        public bool UnlockUpdate(string title)
        {
            for (int i = 0; i < qUpdateTitles.Count; i++)
            {
                if (qUpdateTitles[i].Trim().Equals(title))
                {
                    return UnlockUpdate(i);
                }
            }
            return false;
        }
        
        //get whether the update with the passed title has been unlocked
        public bool isUpdateUnlocked(string title)
        {
            for (int i = 0; i < qUpdateTitles.Count; i++)
            {
                if (qUpdateTitles[i].Trim().Equals(title))
                {
                    return updateUnlocks[i];
                }
            }
            return false;
        }

        //Unlocks the update at the passed position in the order
        public bool UnlockUpdate(int updateNum)
        {
            if (updateNum < 0)
            {
                return UnlockUpdate();
            }
            if (updateNum >= updateUnlocks.Count)
            {
                Debug.Log("Got invalid unlock number");
                return false;
            }
            if(!updateUnlocks[updateNum])
                CreateUpdatePopUP(updateNum);
            updateUnlocks[updateNum] = true;
            return true;
        }
        
        //get whether the update at the passed index has been unlocked
        public bool isUpdateUnlocked(int updateNum)
        {
            if (updateNum < 0)
            {
                return false;
            }
            if (updateNum >= updateUnlocks.Count)
            {
                Debug.Log("Got invalid unlock number");
                return false;
            }
            return updateUnlocks[updateNum];
        }
        
        //Unlocks the next locked update
        public bool UnlockUpdate()
        {
            for (int i = 0; i < updateUnlocks.Count; i++)
            {
                if (!updateUnlocks[i])
                {
                    CreateUpdatePopUP(i);
                    updateUnlocks[i] = true;
                    return true;
                }
            }
            return false;
        }

        //builds a quest node from the passed data object
        //registers itself with the quest manager
        //warning: the quest manager will not accept nodes with duplicate ids, therefore,
        //use QuestManager.questManager.getNode([name]) to save references to node objects, 
        //even when you instantiate one
        public QuestNode(QuestObj data)
        {
            if (!QuestManager.questManager.RegisterNode(this))
            {
                return;
            }
            QObjName = data.name;
            updateUnlocks = new List<bool>();
            InitDVs(data);
            foreach (string wp in data.OnStartWPIDs)
            {
                WPUnlockSerializer.wpUnlockSerializer.Unlock(wp);
            }
        }

        //initialize all fields that come from the QObj
        private void InitDVs(QuestObj data)
        {
            name = data.questName;
            id = data.uniqueID.ToLower();
            type = data.type;
            objectives = data.objectives;
            ReadDescriptionFile(data.descriptionFile);
            ReadUpdatesFile(data.questUpdatesFile);
            while (updateUnlocks.Count < qUpdateTitles.Count)
            {
                updateUnlocks.Add(false);
            }
            compText = data.completeFile.ToString();
            requiredCounts = data.countsRequired;
            if (requiredCounts == null)
            {
                requiredCounts = new List<float>();
            }
            while (requiredCounts.Count < objectives.Count)
            {
                requiredCounts.Add(1);
            }
            countsPerAction = data.countsAdded;
            if (countsPerAction == null)
            {
                countsPerAction = new List<float>();
            }
            while (countsPerAction.Count < requiredCounts.Count)
            {
                countsPerAction.Add(1);
            }
            while (counts.Count < requiredCounts.Count)
            {
                counts.Add(0);
            }
            startNarration = data.receivedNarration;
            completionNarration = data.completeNarration;
            List<string> ids = data.OnCompleteWPIDs;
            WPUnlockIDs = new List<string>();
            while (WPUnlockIDs.Count < ids.Count)
            {
                WPUnlockIDs.Add(ids[WPUnlockIDs.Count]);
            }
        }

        public void callOnceInitialized()
        {
            OnComplete = new UnityEvent<string>();
            QuestObj data = Resources.Load<QuestObj>("QObjects/"+QObjName);
            InitDVs(data);
            QuestManager.questManager.RegisterNode(this);
        }

        //change the pinned status of this node
        public void ChangePinned()
        {
            isPinned = !isPinned;
            if (isPinned)
            {
                QuestManager.questManager.AddPin(this);
            }
            else
            {
                QuestManager.questManager.RemovePin(this);
            }
        }

        //read text asset into a string[]
        private string[] FileToStringArr(TextAsset file)
        {
            string fileText = file.ToString();
            string[] fileSplit = fileText.Split('\n');
            for (int i = 0; i < fileSplit.Length; i++)
            {
                fileSplit[i] = fileSplit[i].Trim(new Char[] {'\r'});
            }

            return fileSplit;
        }
        
        //reads in quest updates
        private void ReadUpdatesFile(TextAsset file)
        {
            qUpdates = new List<string>();
            qUpdateTitles = new List<string>();
            if (file == null) return;
            string[] fileSplit = FileToStringArr(file);
            string updateTitle = "";
            string update = "";
            bool titling = true;
            for (int i = 0; i < fileSplit.Length; i++)
            {
                if (fileSplit[i].Equals("--break--"))
                {
                    qUpdates.Add(update);
                    qUpdateTitles.Add(updateTitle);
                    update = "";
                    updateTitle = "";
                    titling = true;
                }
                else if(fileSplit[i].Equals("--title--"))
                {
                    titling = false;
                }
                else if (fileSplit[i][0] != '#')
                {
                    if (titling)
                    {
                        updateTitle += fileSplit[i]+"\n";
                    }
                    else
                    {
                        update += fileSplit[i] + "\n";
                    }
                }
            }
            if (updateTitle != "")
            {
                qUpdateTitles.Add(updateTitle);
                qUpdates.Add(update);
            }
        }

        //reads in description data from text file
        private void ReadDescriptionFile(TextAsset file)
        {
            string[] fileSplit = FileToStringArr(file);
            string desc = "";
            for (int i = 0; i < fileSplit.Length; i++)
            {
                if (fileSplit[i].Equals("--break--"))
                {
                    shortDescription = desc;
                    desc = "";
                    for (int j = i + 1; j < fileSplit.Length; j++)
                    {
                        if (fileSplit[j][0] != '#')
                        {
                            desc += fileSplit[j] + "\n";
                        }
                    }
                    longDescription = desc;
                    return;
                }
                if (fileSplit[i][0] != '#')
                {
                    desc += fileSplit[i] + "\n";
                }
            }
            shortDescription = desc;
            longDescription = "";
        }
        
        //order quest nodes
        //pinned->unpinned->complete
        //for two nodes in the same category, returns alphabetical based on string compare
        //use of string compare is a possible issue if game is made multi-language.
        public int CompareTo(QuestNode other)
        {
            if (name.Equals(other.name))
            {
                return 0;
            }
            if (isMainQuest && !other.isMainQuest)
            {
                return -1;
            }
            if (isPinned && !other.isPinned)
            {
                return -1;
            }
            if (other.isPinned && !isPinned)
            {
                return 1;
            }
            if (!isComplete && other.isComplete)
            {
                return -1;
            }
            if (!other.isComplete && isComplete)
            {
                return 1;
            }
            return String.Compare(name, other.name);
        }
        
    }
}
