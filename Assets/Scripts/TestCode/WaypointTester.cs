using System;
using QuestSystem;
using UnityEngine;

namespace TestCode
{
    public class WaypointTester : MonoBehaviour
    {
        public QuestObj waypointInfo;

        private WaypointReached obj = new WaypointReached();
            private void Start()
        {
            QuestNode quest = new QuestNode(waypointInfo);
            obj.AddListener(quest.onAction);
        }

        private void OnTriggerEnter(Collider other)
        {
            obj.Invoke(1);
            Destroy(this.gameObject);
        }
    }
}
