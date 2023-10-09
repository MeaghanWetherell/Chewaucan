using System;
using System.Collections.Generic;
using System.IO;
using Misc;
using UnityEngine;
using UnityEngine.Events;

namespace QuestSystem
{
    //object storing data about a particular quest
    public class QuestNode : IComparable<QuestNode>
    {
        //name of the quest, used as an identifier, should ensure unique names
        public string name;

        //unique id of the quest
        public string ID;

        //the function that should be called when quest progress is made
        //int: index of the objective completed (0 for single objective quests)
        //float: the amount of progress on the quest that should be gained (send 0 for default value)
        public readonly UnityAction<int, float> onObjectiveComplete;

        //short description of the quest
        public string shortDescription;

        //long description of the quest
        public string longDescription;

        //list of the objectives to be completed
        public List<string> objectives;

        //list of the required count per objective mapped by index
        public List<float> requiredCounts;

        //current count per objective mapped by index
        public List<float> counts = new List<float>();

        //default count increment for each objective, mapped by index.
        //initialized to 1 if a value is not received
        private List<float> countsPerAction;

        private bool _isComplete = false;

        public bool isComplete => _isComplete;

        private bool _isPinned = false;

        public bool isPinned => _isPinned;

        //builds a quest node from the passed data object
        //registers itself with the quest manager
        //warning: the quest manager will not accept nodes with duplicate names, therefore,
        //use QuestManager.questManager.getNode([name]) to save references to node objects
        public QuestNode(QuestObj data)
        {
            name = data.questName;
            ID = data.uniqueID;
            if (!QuestManager.questManager.registerNode(this))
            {
                return;
            }
            readDescriptionFile(data.descriptionFile);
            objectives = data.objectives;
            requiredCounts = data.countsRequired;
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
            onObjectiveComplete = new UnityAction<int, float>(addCount);
        }

        //change the pinned status of this node
        public void changePinned()
        {
            _isPinned = !_isPinned;
            if (_isPinned)
            {
                QuestManager.questManager.AddPin(this);
            }
            else
            {
                QuestManager.questManager.RemovePin(this);
            }
        }
        
        //adds the passed count to the count for the objective at index
        private void addCount(int index, float toAdd = 0.0f)
        {
            if (_isComplete)
            {
                return;
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
                    if (counts[i] != requiredCounts[i])
                    {
                        HUDManager.hudManager.resetPins();
                        return;
                    }
                }
                _isComplete = true;
                if (_isPinned)
                {
                    _isPinned = false;
                    QuestManager.questManager.reportCompletion(true, this);
                }
                else
                {
                    QuestManager.questManager.reportCompletion();
                }
            }
            else if (_isPinned)
            {
                HUDManager.hudManager.resetPins();
            }
        }

        //reads in description data from text file
        private void readDescriptionFile(TextAsset file)
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
            if (_isPinned && !other._isPinned)
            {
                return -1;
            }
            if (other._isPinned && !_isPinned)
            {
                return 1;
            }
            if (!_isComplete && other._isComplete)
            {
                return -1;
            }
            if (!other._isComplete && _isComplete)
            {
                return 1;
            }
            return String.Compare(name, other.name);
        }
        
    }
}
