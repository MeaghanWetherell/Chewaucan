using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoneInteractable : Interactable
{
    public GameObject original;

    public GameObject outlined;
    public override void OnInteractEnable()
    {
        original.SetActive(false);
        outlined.SetActive(true);
        base.OnInteractEnable();
    }

    public override void OnInteractDisable()
    {
        outlined.SetActive(false);
        original.SetActive(true);
        base.OnInteractDisable();
    }

    public override void Listen(int index)
    {
        
    }

    public override void ListenerRemoved() { }
}
