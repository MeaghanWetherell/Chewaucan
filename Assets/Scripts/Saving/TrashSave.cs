using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TrashSave : MonoBehaviour
{
    public static int toTrash;

    public Button myButton;

    public int mySlot;

    public SetPathAndLoad trashButton;

    public void OnEnable()
    {
        if (trashButton != null)
        {
            trashButton.pathNumber = toTrash;
        }

        if (myButton != null)
        {
            if (!SaveHandler.saveHandler.checkPath(SaveHandler.saveHandler.saveSlots[mySlot]))
            {
                myButton.interactable = false;
            }
            else
            {
                myButton.interactable = true;
            }
        }
    }

    public void SetToTrash()
    {
        toTrash = mySlot;
    }
}
