using System.Collections;
using System.Collections.Generic;
using LoadGUIFolder;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BoneInteractable : Interactable
{
    public GameObject defaultBone; //this is the bone the player sees in the modern map scene

    public GameObject outlinedBone; //set this objects layer to outlinedBone

    public GameObject answerBone; //this is the answer key bone

    public bool isCorrect;

    public AudioSource pickupAudio;

    private bool isLoader = false;
    private Light mainDirectionalLight;
    public override void OnInteractEnable()
    {
        //defaultBone.SetActive(false);
        outlinedBone.SetActive(true);
        base.OnInteractEnable();
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
            isLoader = true;

            // Find and disable the directional light
            if (mainDirectionalLight == null)
            {
                mainDirectionalLight = GameObject.FindGameObjectWithTag("MainLight")?.GetComponent<Light>();
            }

            if (mainDirectionalLight != null)
            {
                mainDirectionalLight.enabled = false;
            }


            pickupAudio.ignoreListenerPause = true;
            pickupAudio?.Play();
        }
        else
        {
            isLoader = false;
        }
    }

    public override void ListenerRemoved() { }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoad;
        SceneManager.sceneUnloaded += OnSceneUnload;
    }
    
    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoad;
        SceneManager.sceneUnloaded -= OnSceneUnload;
    }
    
    private void OnSceneLoad(Scene loaded, LoadSceneMode mode)
    {
        if (isLoader && loaded.name.Equals("BoneComparison"))
        {
            GameObject viewer = GameObject.Find("ViewBone");
        
            GameObject spawnedBone = Instantiate(answerBone, viewer.transform);
            spawnedBone.transform.localScale = answerBone.transform.localScale; // copy scale
            spawnedBone.transform.rotation = answerBone.transform.rotation; //copy rotation of the answer bone
            viewer.GetComponent<BoneChecker>().isCorrect = isCorrect;
            isLoader = false;
        }
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
        }
    }
}
