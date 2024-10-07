using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Misc;
using UnityEngine;
using UnityEngine.Events;

namespace QuestSystem
{
    //object storing data about a particular quest
    [Serializable]
    public class QuestNode : IComparable<QuestNode>
    {
        //initialization narration
        public Narration.Narration startNarration;
        
        //name of the init narration obj
        private string startNarrName;
        
        //completion narration
        public Narration.Narration completionNarration;
        
        //name of the comp narration obj
        private string compNarrName;
        
        //name of the quest, used as an identifier, should ensure unique names
        public string name;

        //unique id of the quest
        public string id;

        //short description of the quest
        public string shortDescription;

        //long description of the quest
        public string longDescription;

        //completion text
        public string compText;

        //list of the objectives to be completed
        public List<string> objectives;

        //list of the required count per objective mapped by index
        public List<float> requiredCounts;

        //current count per objective mapped by index
        public List<float> counts = new List<float>();

        //default count increment for each objective, mapped by index.
        //initialized to 1 if a value is not received
        public List<float> countsPerAction;

        //quest type: Archaeology, Biology, or Geology
        public SaveDialProgressData.Dial type;

        public UnityEvent<string> OnComplete = new UnityEvent<string>();

        public bool isComplete = false;

        public bool isPinned = false;

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
                    // ReSharper disable once CompareOfFloatsByEqualityOperator
                    if (counts[i] != requiredCounts[i] )
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
            name = data.name;
            id = data.uniqueID.ToLower();
            type = data.type;
            objectives = data.objectives;
            ReadDescriptionFile(data.descriptionFile);
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
            for (int i = 0; i < requiredCounts.Count; i++)
            {
                counts.Add(0);
            }

            startNarration = data.receivedNarration;
            startNarrName = startNarration.name;

            completionNarration = data.completeNarration;
            compNarrName = completionNarration.name;
        }

        public void callOnceInitialized()
        {
            if (!QuestManager.questManager.RegisterNode(this))
            {
                return;
            }

            startNarration = Resources.Load<Narration.Narration>(startNarrName);
            completionNarration = Resources.Load<Narration.Narration>(compNarrName);
            if (countsPerAction == null)
            {
                countsPerAction = new List<float>();
            }
            while (countsPerAction.Count < requiredCounts.Count)
            {
                countsPerAction.Add(1);
            }
            for (int i = 0; i < requiredCounts.Count; i++)
            {
                counts.Add(0);
            }
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

        //reads in description data from text file
        private void ReadDescriptionFile(TextAsset file)
        {
            string fileText = file.ToString();
            string[] fileSplit = fileText.Split('\n');
            for (int i = 0; i < fileSplit.Length; i++)
            {
                fileSplit[i] = fileSplit[i].Trim(new Char[] {'\r'});
            }
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
