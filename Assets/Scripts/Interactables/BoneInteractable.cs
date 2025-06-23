using System.Collections;
using System.Collections.Generic;
using LoadGUIFolder;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BoneInteractable : Interactable
{
    public GameObject original;

    public GameObject outlined;

    public GameObject myPrefab;

    public bool isCorrect;

    public AudioSource pickupAudio;

    private bool isLoader = false;
    public override void OnInteractEnable()
    {
        //original.SetActive(false);
        outlined.SetActive(true);
        base.OnInteractEnable();
    }

    public override void OnInteractDisable()
    {
        outlined.SetActive(false);
        //original.SetActive(true);
        base.OnInteractDisable();
    }

    public override void Listen(int index)
    {
        if (LoadGUIManager.loadGUIManager.Load("BoneComparison"))
        {
            isLoader = true;
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
    }
    
    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoad;
    }
    
    private void OnSceneLoad(Scene loaded, LoadSceneMode mode)
    {
        if (isLoader && loaded.name.Equals("BoneComparison"))
        {
            GameObject viewer = GameObject.Find("ViewBone");
            Instantiate(myPrefab, viewer.transform);
            viewer.GetComponent<BoneChecker>().isCorrect = isCorrect;
            isLoader = false;
        }
    }
}
