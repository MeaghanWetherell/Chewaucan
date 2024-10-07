using System.Collections;
using System.Collections.Generic;
using Misc;
using UnityEngine;

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
