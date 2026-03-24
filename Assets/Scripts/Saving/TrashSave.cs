using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TrashSave : MonoBehaviour
{
    public static int toTrash;

    [Tooltip("The button to trash the save")]
    public Button myButton;

    [Tooltip("The slot to trash")]
    public int mySlot;
    

    public void OnEnable()
    {
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
