using ScriptTags;
using UnityEngine;

namespace QuestSystem.Quests.QScripts
{
    public class StartOnTriggerEnter : QuestHandler
    {
        private void OnTriggerEnter(Collider other)
        {
            if(other.GetComponent<Player>() != null)
                StartQuest();
        }
    }
}
