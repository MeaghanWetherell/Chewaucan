using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using LoadGUIFolder;
using Narration;
using QuestSystem;
using ScriptTags;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Playables;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class BonepileScript : MonoBehaviour
{
    public GameObject bpileWalls;

    public QuestObj bpileQ;
    
    public GameObject cutscene1;
    
    public Narration.Narration BP1;

    public Narration.Narration BP2;

    public Narration.Narration BP3;

    public Narration.Narration BP4;

    public Narration.Narration BP10;

    public Narration.Narration BP11;

    public Narration.Narration BP12;

    public Narration.Narration BP51;

    public Narration.Narration BP52;

    private Image mastoBoneUIImage;

    public List<BoneInteractable> allBoneInteractables;

    public Vector3 BPilePlayerPosition;

    private static BonepileScript scriptSingleton;
    
    private void Start()
    {
        QuestNode bpile = QuestManager.questManager.GETNode("bonepile");
        if (bpile != null && bpile.isComplete && !BP10.HasPlayed())
        {
            BP10.SetPlayability(true);
            //Player.player.GetComponent<LandMovement>().enabled = true;
        }

        if (!BP2.HasPlayed())
        {
            cutscene1.SetActive(true);
            foreach (Transform wall in bpileWalls.transform)
            {
                wall.gameObject.SetActive(false);
            }
        }
        BP1.addToOnComplete(new List<UnityAction<string>>{
            s =>
            {
                if (mastoBoneUIImage == null)
                    mastoBoneUIImage = GameObject.Find("MastoBoneUI")?.GetComponent<Image>();
                if(mastoBoneUIImage != null)
                    mastoBoneUIImage.gameObject.SetActive(true);
            }});
        BP3.addToOnComplete(new List<UnityAction<string>> {
            s =>
            {
                foreach (Transform wall in bpileWalls.transform)
                {
                    wall.gameObject.SetActive(true);
                }
            }});
        if (!BP4.HasPlayed())
        {
            BP4.addToOnComplete(new List<UnityAction<string>> { s => { 
            
                QuestManager.questManager.CreateQuestNode(bpileQ); 
                if (!QuestManager.questManager.SubToCompletion("bonepile", toSub =>
                    {
                        //Debug.Log("Setting BP10 Playable");
                        BP10.SetPlayability(true);
                    }))
                {
                    Debug.Log("sub failed");
                }} });
        }
        else if(bpile != null && !bpile.isComplete)
        {
            QuestManager.questManager.SubToCompletion("bonepile", toSub =>
            {
                //Debug.Log("Setting BP10 Playable");
                BP10.SetPlayability(true);
            });
        }
        
        
            

        if (scriptSingleton != null)
        {
            Destroy(scriptSingleton.gameObject);
        }
        DontDestroyOnLoad(gameObject);
        scriptSingleton = this;
        
        if (!BP2.HasPlayed())
        {
            if (mastoBoneUIImage == null)
                mastoBoneUIImage = GameObject.Find("MastoBoneUI")?.GetComponent<Image>();
            if(mastoBoneUIImage != null)
                mastoBoneUIImage.gameObject.SetActive(false);
        }
        BP10.addToOnComplete(new List<UnityAction<string>>{ str => {
                //Debug.Log("Ran BP10 OnComp");
                BP11.SetPlayability(true);},
            str => { WPUnlockSerializer.wpUnlockSerializer.unlockAllModern = true;}
        });
        if (!BP11.HasPlayed())
        {
            BP11.addToOnComplete(new List<UnityAction<string>>{
                str =>
                {
                    //Debug.Log("Ran BP11 OnComp");
                    LoadGUIManager.loadGUIManager.InstantiatePopUp("Back to the Present!", "Open your astrolabe and return to the present!");
                    v3Wrapper toSerialize = new v3Wrapper(BPilePlayerPosition);
                    string json = JsonSerializer.Serialize(toSerialize);
                    string savePath = SaveHandler.saveHandler.getSavePath();
                    File.WriteAllText(savePath+"/astrolabeteleposition1.json", json);
                    BP12.SetPlayability(true);
                }});
        }
        BP12.addToOnComplete(new List<UnityAction<string>>
        {
            str => QuestManager.questManager.CreateQuestNode("match31")
        });
    }
    
    //deprecated
    public void StartBP2()
    {
        foreach (Transform wall in bpileWalls.transform)
        {
            wall.gameObject.SetActive(true);
        }
        BP2.SetPlayability(true);
        BP2.Begin();
        LoadGUIManager.loadGUIManager.Load("MastodonBoneViewer");
        AudioListener.pause = false;
        BP2.addToOnComplete(new List<UnityAction<string>>{StartBP3});
        if (mastoBoneUIImage == null)
            mastoBoneUIImage = GameObject.Find("MastoBoneUI")?.GetComponent<Image>();
        if(mastoBoneUIImage != null)
            mastoBoneUIImage.gameObject.SetActive(true);
    }

    public void StartBP3(string none)
    {
        LoadGUIManager.loadGUIManager.CloseOpenGUI();
        BP2.SetPlayability(false);
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
        BP51.SetPlayability(true);
        BP52.SetPlayability(true);
        LoadGUIManager.loadGUIManager.InstantiatePopUp("The Bone Pile", "Interact with bones with Tab. Find the bone that matches yours!");
    }

    private IEnumerator FlashOutlines()
    {
        foreach (BoneInteractable bint in allBoneInteractables)
        {
            if (bint != null)
                bint.outlinedBone.SetActive(true);
        }
        yield return new WaitForSeconds(5);
        foreach (BoneInteractable bint in allBoneInteractables)
        {
            if (bint != null)
                bint.outlinedBone.SetActive(false);
        }
    }
}
