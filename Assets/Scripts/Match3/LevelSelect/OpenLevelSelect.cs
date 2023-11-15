using System;
using Misc;
using ScriptTags;
using UnityEngine;

namespace Match3
{
    public class OpenLevelSelect : LoadGUI, IListener
    {
        public static OpenLevelSelect openLevelSelect;
        
        public bool shouldLoadBone = false;
        
        private void OnTriggerEnter(Collider other)
        {
            openLevelSelect = this;
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

        public override void onOpenTrigger()
        {
            base.onOpenTrigger();
            if (BoneSceneManager.boneSceneManager.wasLoaded)
            {
                BoneSceneManager.boneSceneManager.wasLoaded = false;
                shouldLoadBone = true;
            }
        }

        public void listen(int index)
        {
            onOpenTrigger();
        }

        public void listenerRemoved(){}
    }
}
