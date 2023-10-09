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
        public QuestObj quest;

        private static List<QuestHandler> allHandlers;

        private readonly QuestEvent _questEvent = new QuestEvent();

        private void Awake()
        {
            if (allHandlers == null)
                allHandlers = new List<QuestHandler>();
        }
        
        void Start()
        {
            allHandlers.Add(this);
            new QuestNode(quest);
            _questEvent.AddListener(QuestManager.questManager.getNode(quest.uniqueID).onObjectiveComplete);
        }

        //invokes the event associated with this handler passing it index and toAdd
        //index is the index of the objective being completed
        //toAdd is the amount of progress to add on that objective
        //if you only have one objective and you only want to add the default progress value for the quest, pass no parameters.
        public void invokeMyEvent(int index = 0, float toAdd = 0)
        {
            _questEvent.Invoke(index, toAdd);
        }

        //invokes the event associated with the handler for a quest with the passed ID using the passed parameters
        public static void InvokeFromHandler(string questID, int objectiveIndex = 0, float countToAdd = 0)
        {
            foreach (QuestHandler handler in allHandlers)
            {
                if (handler.quest.uniqueID.Equals(questID))
                {
                    handler.invokeMyEvent(objectiveIndex, countToAdd);
                }
            }
        }
    }
}
