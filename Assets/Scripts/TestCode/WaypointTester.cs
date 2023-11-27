using System;
using QuestSystem;
using UnityEngine;

namespace TestCode
{
    public class WaypointTester : QuestHandler
    {
        public int waypointNumber;

        private void Start()
        {
            StartQuest();
            if (QuestManager.questManager.GETNode("wp" + waypointNumber).isComplete)
            {
                GameObject.Destroy(gameObject);
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            ProgressObjective();
            QuestManager.questManager.GETNode("reachallwp").AddCount(waypointNumber - 1);
            Destroy(this.gameObject);
        }
    }
}
