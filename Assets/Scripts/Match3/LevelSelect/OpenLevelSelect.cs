using LoadGUIFolder;
using Misc;
using ScriptTags;
using UnityEngine;

namespace Match3
{
    //allows the user to press the interact key to open the match3 level select while within this object's trigger collider
    public class OpenLevelSelect : LoadGUI, IListener
    {
        public static OpenLevelSelect openLevelSelect;
        
        public bool shouldLoadBone = false;
        
        private void OnTriggerEnter(Collider other)
        {
            openLevelSelect = this;
            if (other.GetComponent<Player>() != null)
            {
                InteractListenerManager.interactListenerManager.ChangeListener(this, 1);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.GetComponent<Player>() != null)
            {
                InteractListenerManager.interactListenerManager.DeRegister(this);
            }
        }

        public override void ONOpenTrigger()
        {
            base.ONOpenTrigger();
            if (BoneSceneManager.boneSceneManager.wasLoaded)
            {
                BoneSceneManager.boneSceneManager.wasLoaded = false;
                shouldLoadBone = true;
            }
        }

        public void Listen(int index)
        {
            ONOpenTrigger();
        }

        public void ListenerRemoved(){}
    }
}
