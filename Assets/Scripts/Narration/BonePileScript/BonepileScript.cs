using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using KeyRebinding;
using LoadGUIFolder;
using Narration;
using QuestSystem;
using ScriptTags;
using TimeTravel;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.Playables;
using UnityEngine.Serialization;
using UnityEngine.UI;

//kind of cludgy script to make the bone pile narrations and pop ups and everything happen at the right times
//essentially just full of bespoke code for each narration that runs when it completes to do specific things that aren't handled generically by anything
public class BonepileScript : MonoBehaviour
{
    [Tooltip("Parent object containing all the bone pile walls")]
    public GameObject bpileWalls;

    [Tooltip("Quest object for the bone pile quest")]
    public QuestObj bpileQ;
    
    [Tooltip("The playable director component for cutscene 1")]
    public PlayableDirector cutscene1;
    
    public Narration.Narration BP1;

    public Narration.Narration BP2;

    public Narration.Narration BP3;

    public Narration.Narration BP4;

    public Narration.Narration BP10;

    public Narration.Narration BP11;

    public Narration.Narration BP12;

    public Narration.Narration BP51;

    public Narration.Narration BP52;

    //the mastodon bone hud image
    private Image mastoBoneUIImage;

    [Tooltip("All the bones in the bonepile the player can interact with")]
    public List<BoneInteractable> allBoneInteractables;

    [Tooltip("The location to return the player to in the modern map after going to the pleistocene")]
    public Vector3 BPilePlayerPosition;

    public static BonepileScript scriptSingleton;
    
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
        if (!BP3.HasPlayed())
        {
            BP3.addToOnComplete(new List<UnityAction<string>> { s => { 
            
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
            cutscene1.Play();
            if (mastoBoneUIImage == null)
                mastoBoneUIImage = GameObject.Find("MastoBoneUI")?.GetComponent<Image>();
            if(mastoBoneUIImage != null)
                mastoBoneUIImage.gameObject.SetActive(false);
        }
        BP10.addToOnComplete(new List<UnityAction<string>>{ str => {
                BP11.SetPlayability(true);}
        });
        if (!BP11.HasPlayed())
        {
            BP11.addToOnComplete(new List<UnityAction<string>>{
                str =>
                {
                    LoadGUIManager.loadGUIManager.InstantiatePopUp("Back to the Present!", "Open your astrolabe and return to the present!");
                    AstrolabeQueueManager.queueManager.EnqueueDestination(BPilePlayerPosition, 1);
                    MapUIController.canOpenMap = true;
                    BP12.SetPlayability(true);
                }, str => { WPUnlockSerializer.wpUnlockSerializer.unlockAllModern = true;}});
        }
        string questKey = BindingManager.bindingManager.GetBind("Quest Menu");
        string mapKey = BindingManager.bindingManager.GetBind("Open Map");
        BP12.addToOnComplete(new List<UnityAction<string>>
        {
            str =>
            {
                QuestManager.questManager.CreateQuestNode("match31");
                LoadGUIManager.loadGUIManager.InstantiatePopUp("The tutorial is done!", "You can now explore the map for more clues. When you think you know enough, press "+questKey+" to open the quest menu where you can choose to tell that kid their birthday story and end the game. If you’d like to fast travel, press "+mapKey+" to open the map.");
            }
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

    //flash the outlines of all the interactable bones
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
