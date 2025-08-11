using System;
using System.Collections;
using System.Collections.Generic;
using LoadGUIFolder;
using Narration;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class BonepileScript : MonoBehaviour
{
    public Narration.Narration BP1;

    public Narration.Narration BP2;

    public Narration.Narration BP3;

    public Narration.Narration BP4;

    public Image mastoBoneUIImage;

    public List<BoneInteractable> allBoneInteractables;
    
    private void Start()
    {
        BP1.addToOnComplete(new List<UnityAction<string>>{StartBP2});
        if (!NarrationManager.narrationManager.hasRun.Contains("BP2"))
        {
            mastoBoneUIImage.gameObject.SetActive(false);
        }
    }

    public void StartBP2(string none)
    {
        BP2.SetPlayability(true);
        BP2.Begin();
        LoadGUIManager.loadGUIManager.Load("MastodonBoneViewer");
        AudioListener.pause = false;
        BP2.addToOnComplete(new List<UnityAction<string>>{StartBP3});
        mastoBoneUIImage.gameObject.SetActive(true);
    }

    public void StartBP3(string none)
    {
        LoadGUIManager.loadGUIManager.CloseOpenGUI();
        BP3.SetPlayability(true);
        BP3.Begin();
        LoadGUIManager.loadGUIManager.InstantiatePopUp("Movement", "Use WASD to move");
        BP3.addToOnComplete(new List<UnityAction<string>>{StartBP4});
    }

    public void StartBP4(string none)
    {
        BP4.SetPlayability(true);
        BP4.Begin();
        StartCoroutine(FlashOutlines());
        BP4.addToOnComplete(new List<UnityAction<string>>{ShowPopUp});
    }

    public void ShowPopUp(string none)
    {
        LoadGUIManager.loadGUIManager.InstantiatePopUp("The Bone Pile", "Interact with bones with Tab. Find the bone that matches yours!");
    }

    private IEnumerator FlashOutlines()
    {
        foreach (BoneInteractable bint in allBoneInteractables)
        {
            bint.outlinedBone.SetActive(true);
        }
        yield return new WaitForSeconds(5);
        foreach (BoneInteractable bint in allBoneInteractables)
        {
            bint.outlinedBone.SetActive(false);
        }
    }
}
