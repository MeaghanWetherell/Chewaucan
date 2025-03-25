using System;
using System.Collections;
using System.Collections.Generic;
using Misc;
using UnityEngine;
using UnityEngine.InputSystem.HID;
using UnityEngine.UI;

public class SetPathAndLoad : MonoBehaviour
{
    public enum loadType
    {
        myPath,
        lastUsed,
        newGame
    }
    
    public Button myButton;

    public loadType lt;
    
    public string myPath;

    private void Start()
    {
        switch (lt)
        {
            case loadType.myPath:
                if (!SaveHandler.saveHandler.checkPath(myPath))
                    myButton.interactable = false;
                myButton.onClick.AddListener(LoadMyPath);
                break;
            case loadType.lastUsed:
                if (!SaveHandler.saveHandler.checkPath())
                    myButton.interactable = false;
                myButton.onClick.AddListener(LoadLastUsed);
                break;
            case loadType.newGame:
                myButton.onClick.AddListener(NewGame);
                break;
        }
        
    }

    private void LoadMyPath()
    {
        SaveHandler.saveHandler.setSavePath(myPath);
        SaveHandler.saveHandler.Load();
        SceneLoadWrapper.sceneLoadWrapper.LoadScene("Modern Map");
    }

    private void LoadLastUsed()
    {
        SaveHandler.saveHandler.Load();
        SceneLoadWrapper.sceneLoadWrapper.LoadScene("Modern Map");
    }

    private void NewGame()
    {
        SaveHandler.saveHandler.NewGame(myPath);
        SaveHandler.saveHandler.Load();
        SceneLoadWrapper.sceneLoadWrapper.LoadScene("Modern Map");
    }
}
