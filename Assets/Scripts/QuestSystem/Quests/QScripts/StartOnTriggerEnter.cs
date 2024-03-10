using UnityEngine;

namespace QuestSystem.Quests.QScripts
{
    public class StartOnTriggerEnter : QuestHandler
    {
        private void OnTriggerEnter(Collider other)
        {
            StartQuest();
        }
    }
}
