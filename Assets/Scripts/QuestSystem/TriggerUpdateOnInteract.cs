using System;
using System.Collections;
using System.Collections.Generic;
using QuestSystem;
using UnityEngine;
using UnityEngine.Playables;

public class TriggerUpdateOnInteract : Interactable
{
    [Tooltip("This is the object the player sees normally in the modern map scene")]public GameObject defaultObject; 

    [Tooltip("Reference to the outlined version of the object")]public GameObject outlinedObject; //set this object's layer to outlinedBone

    [Tooltip("The name of the update that this object should unlock")]
    public string updateName;

    [Tooltip("id of the quest this interactable should update")]
    public string qid;

    [Tooltip("The cutscene to play when interacting with this object. Will only run a cutscene OR play narration, prioritizing the cutscene if both are set. If neither are set only sends a quest update.")]
    public PlayableDirector director;

    [Tooltip("The narration to play when interacting with this object. Will only run a cutscene OR play narration, prioritizing the cutscene if both are set. If neither are set only sends a quest update.")]
    public Narration.Narration narr;

    [Tooltip("Whether to display an update pop-up on interact")]
    public bool createPopUp;

    [Tooltip("Whether interacting with this progresses the first objective of the associated quest")] 
    public bool progressObjective;

    [Tooltip("The trigger instance that must be interacted with before this one is enabled")]
    public TriggerUpdateOnInteract waitTrigger;

    public void Start()
    {
        QuestNode quest = QuestManager.questManager.GETNode(qid);
        if (quest == null || quest.isUpdateUnlocked(updateName))
        {
            enabled = false;
        }
    }

    //called when a raycast from the interact raycaster hits this object. outlines the object if its update hasn't been obtained
    public override void OnInteractEnable()
    {
        if (waitTrigger == null || !waitTrigger.enabled)
        {
            outlinedObject.SetActive(true);
            base.OnInteractEnable(); 
        }
    }
    
    //disables the outline
    public override void OnInteractDisable()
    {
        outlinedObject.SetActive(false);
        base.OnInteractDisable();
    }
    
    //called when the player pressed the interact key. 
    public override void Listen(int index)
    {
        QuestNode quest = QuestManager.questManager.GETNode(qid);
        if(director != null)
            director.Play();
        else if (narr != null)
        {
            narr.Begin();
            quest.UnlockUpdate(updateName, createPopUp);
            if (progressObjective)
                quest.AddCount(0);
        }
        OnInteractDisable();
        Destroy(this);
    }

    //call this via signal when an attached cutscene ends
    public void SendQuestChanges()
    {
        QuestNode quest = QuestManager.questManager.GETNode(qid);
        quest.UnlockUpdate(updateName, createPopUp);
        if (progressObjective)
            quest.AddCount(0);
    }

    public override void ListenerRemoved() { }
}
