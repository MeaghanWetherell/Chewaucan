using System.Collections;
using System.Collections.Generic;
using LoadGUIFolder;
using QuestSystem;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BoneInteractable : Interactable
{
    public GameObject defaultBone; //this is the bone the player sees in the modern map scene

    public GameObject outlinedBone; //set this object's layer to outlinedBone

    public GameObject answerBone; //this is the answer key bone

    public bool isCorrect;

    public string boneName;

    public AudioSource pickupAudio;

    public static BoneInteractable currBone;

    public bool sendUpdate = false;

    private bool subbed = false;
    private Light mainDirectionalLight;
    
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

    public override void OnInteractDisable()
    {
        outlinedBone.SetActive(false);
        //defaultBone.SetActive(true);
        base.OnInteractDisable();
    }

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
