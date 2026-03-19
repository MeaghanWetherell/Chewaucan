using System;
using System.Collections;
using System.Collections.Generic;
using LoadGUIFolder;
using QuestSystem;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BoneInteractable : Interactable
{
    [Tooltip("This is the bone the player sees normally in the modern map scene")]public GameObject defaultBone; 

    [Tooltip("Reference to the outlined version of the bone")]public GameObject outlinedBone; //set this object's layer to outlinedBone

    [Tooltip("Reference to the answer bone prefab")]public GameObject answerBone; 

    [Tooltip("Whether this is the target bone")]public bool isCorrect;

    [Tooltip("The name of the bone")]public string boneName;

    [Tooltip("Audio source that should play when the bone is picked up")]public AudioSource pickupAudio;

    //singleton ref
    public static BoneInteractable currBone;

    //whether the script should send a quest update when the menu closes. set by the bonechecker script
    [NonSerialized]public bool sendUpdate = false;

    //stores whether this script is currently subscribed as an interact listener
    private bool subbed = false;
    
    //main light in the modern map
    private Light mainDirectionalLight;
    
    //when the bone comparison scene is closed, update the bonepile quest if applicable
    public void UpdateQuest(string guiName)
    {
        if (guiName.Equals("BoneComparison"))
        {
            if(sendUpdate)
                QuestManager.questManager.GETNode("bonepile").UnlockUpdate(boneName);
            LoadGUIManager.loadGUIManager.UnsubtoUnload(UpdateQuest);
            BoneInteractable.currBone = null;
        }
    }
    
    //called when a raycast from the interact raycaster hits this object. outlines the bone if the bonepile is not complete
    public override void OnInteractEnable()
    {
        QuestNode bpile = QuestManager.questManager.GETNode("bonepile");
        //defaultBone.SetActive(false);
        if (bpile is { isComplete: false })
        {
            outlinedBone.SetActive(true);
            base.OnInteractEnable();
        }
    }

    //disables the outline
    public override void OnInteractDisable()
    {
        outlinedBone.SetActive(false);
        //defaultBone.SetActive(true);
        base.OnInteractDisable();
    }

    //called when the player pressed the interact key. loads the bonecomparison menu
    public override void Listen(int index)
    {
        if (LoadGUIManager.loadGUIManager.Load("BoneComparison"))
        {
            BoneInteractable.currBone = this;

            // Find and disable the directional light
            if (mainDirectionalLight == null)
            {
                mainDirectionalLight = GameObject.FindGameObjectWithTag("MainLight")?.GetComponent<Light>();
            }

            if (mainDirectionalLight != null)
            {
                mainDirectionalLight.enabled = false;
            }

            subbed = true;
            SceneManager.sceneUnloaded += OnSceneUnload;
            pickupAudio.ignoreListenerPause = true;
            pickupAudio?.Play();
        }
        else
        {
            BoneInteractable.currBone = null;
        }
    }

    public override void ListenerRemoved() { }
    
    private void OnDisable()
    {
        if(subbed)
            SceneManager.sceneUnloaded -= OnSceneUnload;
    }

    // To turn the main light back on when you unload the scene.
    private void OnSceneUnload(Scene unloaded)
    {
        if (unloaded.name.Equals("BoneComparison"))
        {
            if (mainDirectionalLight != null)
            {
                mainDirectionalLight.enabled = true;
            }

            SceneManager.sceneUnloaded -= OnSceneUnload;
        }
    }
}
