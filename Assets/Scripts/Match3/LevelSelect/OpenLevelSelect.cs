using System;
using Misc;
using ScriptTags;
using UnityEngine;

namespace Match3
{
    public class OpenLevelSelect : LoadGUI, IListener
    {
        private void OnTriggerEnter(Collider other)
        {
            if (other.GetComponent<Player>() != null)
            {
                InteractListenerManager.interactListenerManager.changeListener(this, 1);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.GetComponent<Player>() != null)
            {
                InteractListenerManager.interactListenerManager.deRegister(this);
            }
        }

        public void listen(int index)
        {
            onOpenTrigger();
        }

        public void listenerRemoved(){}
    }
}
