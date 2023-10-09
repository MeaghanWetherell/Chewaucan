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
            startQuest();
        }

        private void OnTriggerEnter(Collider other)
        {
            progressObjective();
            QuestManager.questManager.getNode("reachallwp").addCount(waypointNumber - 1);
            Destroy(this.gameObject);
        }
    }
}
