using System;
using System.Collections;
using System.Collections.Generic;
using Misc;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem.HID;
using UnityEngine.UI;

//handles loading a new game
public class SetPathAndLoad : MonoBehaviour
{
    public enum loadType
    {
        myPath,
        lastUsed,
        newGame,
        continueIfPossible
    }
    
    public Button myButton;

    [Tooltip("The type of loading this button should. myPath for the path number set below, lastUsed for the last used save path, newGame for a newGame, continueIfPossible to continue from last path if it exists and otherwise start a new game")]
    public loadType lt;

    [Tooltip("The main text for the save slot name")]
    public TextMeshProUGUI text;
    
    [Tooltip("Save slot to load if set to myPath")]
    public int pathNumber;

    [Tooltip("Reference to an instance of the prefab pop-up asking the player to name a new save")]
    public GameObject newGameNameObj;

    //initialize the button based on the inspector settings
    private void Start()
    {
        string myPath = SaveHandler.saveHandler.saveSlots[pathNumber];
        switch (lt)
        {
            case loadType.myPath:
                if (!SaveHandler.saveHandler.checkPath(myPath))
                    myButton.interactable = false;
                else
                {
                    text.text = "Load " + myPath.Split("/")[^1];
                }
                myButton.onClick.AddListener(LoadMyPath);
                break;
            case loadType.lastUsed:
                myPath = SaveHandler.saveHandler.getLastSavePath();
                if (!SaveHandler.saveHandler.checkPath(myPath))
                    myButton.interactable = false;
                else
                {
                    text.text = "Continue with " + myPath.Split("/")[^1];
                }
                myButton.onClick.AddListener(LoadLastUsed);
                break;
            case loadType.newGame:
                myButton.onClick.AddListener(NewGame);
                break;
            case loadType.continueIfPossible:
                if (!SaveHandler.saveHandler.checkPath(myPath))
                {
                    myButton.onClick.AddListener(NewGame);
                    text.text = "New Game";
                }
                else
                {
                    myButton.onClick.AddListener(LoadMyPath);
                    text.text = "Load " + myPath.Split("/")[^1];
                }
                break;
        }
        
    }

    public void SetPath(int pathNo)
    {
        pathNumber = pathNo;
    }

    private void LoadMyPath()
    {
        SaveHandler.saveHandler.setSavePath(SaveHandler.saveHandler.saveSlots[pathNumber]);
        SaveHandler.saveHandler.Load();
        if(PlayerPositionManager.playerPositionManager.loadModernMap)
            SceneLoadWrapper.sceneLoadWrapper.LoadScene("Modern Map");
        else
        {
            SceneLoadWrapper.sceneLoadWrapper.LoadScene("PleistoceneMap");
        }
    }

    private void LoadLastUsed()
    {
        SaveHandler.saveHandler.setSavePath(SaveHandler.saveHandler.getLastSavePath());
        SaveHandler.saveHandler.Load();
        if(PlayerPositionManager.playerPositionManager.loadModernMap)
            SceneLoadWrapper.sceneLoadWrapper.LoadScene("Modern Map");
        else
        {
            SceneLoadWrapper.sceneLoadWrapper.LoadScene("PleistoceneMap");
        }
    }

    private void NewGame()
    {
        newGameNameObj.SetActive(true);
        newGameNameObj.GetComponent<NameNewGame>().Initialize(pathNumber);
    }
}
