using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrashSave : MonoBehaviour
{
    public static int toTrash;

    public SetPathAndLoad trashButton;

    public void OnEnable()
    {
        if (trashButton != null)
            trashButton.pathNumber = toTrash;
    }

    public void SetToTrash(int trashNo)
    {
        toTrash = trashNo;
    }
}
