using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using QuestSystem;
using UnityEditor.Build.Content;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SaveHandler : MonoBehaviour
{
    public Button continueButton;

    public string currentVersion;
    
    private bool shouldLoad = false;
    
    public void Awake()
    {
        try
        {
            StreamReader streamReader = new StreamReader("Saves/Version");
            string line = streamReader.ReadLine();
            line = line.Trim();
            if(!line.Equals(currentVersion))
                VersionConversion(line);
            streamReader.Close();
        }
        catch (FileNotFoundException)
        {
            continueButton.interactable = false;
        }

    }

    public void NewGame()
    {
        Directory.Delete("Saves", true);
        Directory.CreateDirectory("Saves");
        File.WriteAllText("Saves/Version", currentVersion);
        QuestManager.resetQuests = true;
        LoadPersistentObjects.LoadObjs();
        shouldLoad = true;
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name.Equals("PersistentObjects") && shouldLoad)
        {
            QuestManager.resetQuests = false;
            SceneManager.LoadScene("Modern Map");
        }
    }

    public void VersionConversion(string fileVersion)
    {
        Debug.Log("Version mismatch");
    }
}
