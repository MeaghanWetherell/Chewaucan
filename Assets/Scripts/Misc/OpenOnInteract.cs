using System.Collections;
using System.Collections.Generic;
using LoadGUIFolder;
using Match3;
using Misc;
using ScriptTags;
using UnityEngine;

public class OpenOnInteract : LoadGUI, IListener
{
    private void OnTriggerEnter(Collider other)
    {
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

    public void Listen(int index)
    {
        ONOpenTrigger();
    }

    public void ListenerRemoved(){}
}
