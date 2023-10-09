using System;
using QuestSystem;
using UnityEngine;

namespace TestCode
{
    public class WaypointTester : QuestHandler
    {
        public int waypointNumber;
        private void OnTriggerEnter(Collider other)
        {
            invokeMyEvent();
            QuestHandler.InvokeFromHandler("reachallwp", waypointNumber-1);
            Destroy(this.gameObject);
        }
    }
}
