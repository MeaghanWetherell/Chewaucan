using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadPersistentObjects : MonoBehaviour
{
    private void Awake()
    {
        if (GameObject.Find("QuestManager") == null || GameObject.Find("MatchLevelManager") == null)
        {
            SceneManager.LoadScene("PersistentObjects", LoadSceneMode.Additive);
        }
    }

    public static void LoadObjs()
    {
        SceneManager.LoadScene("PersistentObjects", LoadSceneMode.Additive);
    }
}
