using System;
using System.Collections.Generic;
using System.IO;
using Misc;
using UnityEngine;
using UnityEngine.Events;

namespace QuestSystem
{
    public class QuestNode : IComparable<QuestNode>
    {
        public string name;

        public UnityAction<float> onAction;

        public string shortDescription;

        public string longDescription;

        public string objective;

        public float requiredCount;

        public float count;

        private float countPerAction;

        private bool _isComplete = false;

        public bool isComplete => _isComplete;

        private bool _isPinned = false;

        public bool isPinned => _isPinned;

        public QuestNode(QuestObj data)
        {
            name = data.questName;
            readDescriptionFile(data.descriptionFile);
            objective = data.objective;
            requiredCount = data.countRequired;
            QuestManager.questManager.registerNode(this);
            onAction = new UnityAction<float>(addCount);
        }

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
        
        private void addCount(float toAdd = 0.0f)
        {
            if (_isComplete)
            {
                return;
            }
            if (toAdd == 0)
            {
                toAdd = countPerAction;
            }
            count += toAdd;
            if (count >= requiredCount)
            {
                _isComplete = true;
                if (_isPinned)
                {
                    QuestManager.questManager.reportCompletion(true, this);
                }
                else
                {
                    QuestManager.questManager.reportCompletion();
                }
                _isPinned = false;
            }
            else if (_isPinned)
            {
                HUDManager.hudManager.resetPins();
            }
        }

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
                    break;
                }
                if (fileSplit[i][0] != '#')
                {
                    desc += fileSplit[i] + "\n";
                }
            }

        }

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
