using System;
using System.Collections;
using System.Collections.Generic;
using LoadGUIFolder;
using Misc;
using ScriptTags;
using UnityEngine;

public class DeerflySwarmTrigger : MonoBehaviour
{
    public static bool swarmed;

    public GameObject swarmPrefab;

    public string popupTitle;

    public string popupMsg;

    private void Awake()
    {
        swarmed = false;
    }
    

    private void OnTriggerEnter(Collider other)
    {
        if (!swarmed && other.gameObject.GetComponent<Player>() != null)
        {
            swarmed = true;
            Instantiate(swarmPrefab, other.gameObject.transform);
            LoadGUIManager.loadGUIManager.InstantiatePopUp(popupTitle, popupMsg);
        }
    }
}
