using System;
using System.Collections;
using System.Collections.Generic;
using QuestSystem;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadPersistentObjects : MonoBehaviour
{
    private void Awake()
    {
        LoadObjs();
    }

    public static void LoadObjs()
    {
        if (QuestManager.questManager != null) return;
        int count = SceneManager.sceneCount;
        for (int i = 0; i < count; i++)
        {
            if (SceneManager.GetSceneAt(i).name.Equals("PersistentObjects"))
            {
                return;
            }
        }
        SceneManager.LoadScene("PersistentObjects", LoadSceneMode.Additive);
    }
}
