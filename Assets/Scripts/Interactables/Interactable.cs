using System.Collections;
using System.Collections.Generic;
using Misc;
using UnityEngine;

//abstract class implementing IListener interface for interactable objects.
//Automatically searched for by the InteractListenerManager
public abstract class Interactable : MonoBehaviour, IListener
{

    public virtual void OnInteractEnable()
    {
        InteractListenerManager.interactListenerManager.ChangeListener(this);
    }

    public virtual void OnInteractDisable()
    {
        InteractListenerManager.interactListenerManager.DeRegister(this);
    }

    public abstract void Listen(int index);

    public abstract void ListenerRemoved();
}
