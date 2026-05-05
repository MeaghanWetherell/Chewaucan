using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrashSaveHelper : MonoBehaviour
{
    public GameObject newGameNameObj;
    
    public void NewGame()
    {
        newGameNameObj.SetActive(true);
        newGameNameObj.GetComponent<NameNewGame>().Initialize();
    }
}
