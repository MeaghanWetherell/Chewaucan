using QuestSystem;
using UnityEngine;

namespace TestCode
{
    public class QuestTester : QuestHandler
    {
        public KeyCode key;
        void Update()
        {
            if (Input.GetKeyDown(key))
            {
                invokeMyEvent();
            }
        }
    }
}
