using System;
using QuestSystem;
using UnityEngine;

namespace TestCode
{
    public class QuestTester : QuestHandler
    {
        public KeyCode key;

        private void Start()
        {
            startQuest();
        }

        void Update()
        {
            if (Input.GetKeyDown(key))
            {
                progressObjective();
            }
        }
    }
}
