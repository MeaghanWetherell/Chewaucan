using System.Collections.Generic;
using Audio;
using Misc;
using UnityEngine;
using UnityEngine.Events;

namespace QuestSystem
{
    //Manages completion of quest objectives for a particular quest
    //recommend extending this to set up new quests and invoking their events
    //but as long as a handler for it exists, it can be called on for quest progress
    //questhandlers should be the only way anything external to the quest system itself interfaces with quests.
    public class QuestHandler : MonoBehaviour
    {
        public QuestObj questData;

        private QuestNode _quest;

        //initialize the quest associated with this handler
        public void StartQuest()
        {
            _quest = QuestManager.questManager.CreateQuestNode(questData);
        }

        //progresses an objective on the quest associated with this handler passing it index and toAdd
        //index is the index of the objective being completed
        //toAdd is the amount of progress to add on that objective
        //if you only have one objective and you only want to add the default progress value for the quest, pass no parameters.
        public bool ProgressObjective(int index = 0, float toAdd = 0)
        {
            if (_quest == null)
                return false;
            return _quest.AddCount(index, toAdd);
        }

    }
}
