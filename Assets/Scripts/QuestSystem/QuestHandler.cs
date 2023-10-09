using System.Collections.Generic;
using UnityEngine;

namespace QuestSystem
{
    //Manages completion of quest objectives for a particular quest
    //recommend extending this to set up new quests and invoking their events
    //but as long as a handler for it exists, it can be called on for quest progress
    //questhandlers should be the only way anything external to the quest system itself interfaces with quests.
    public class QuestHandler : MonoBehaviour
    {
        public QuestObj questData;

        private QuestNode quest;

        //initialize the quest associated with this handler
        public void startQuest()
        {
            new QuestNode(questData);
            quest = QuestManager.questManager.getNode(questData.uniqueID);
        }

        //progresses an objective on the quest associated with this handler passing it index and toAdd
        //index is the index of the objective being completed
        //toAdd is the amount of progress to add on that objective
        //if you only have one objective and you only want to add the default progress value for the quest, pass no parameters.
        public bool progressObjective(int index = 0, float toAdd = 0)
        {
            if (quest == null)
                return false;
            return quest.addCount(index, toAdd);
        }

    }
}
