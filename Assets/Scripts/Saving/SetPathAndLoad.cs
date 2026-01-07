using System;
using System.Collections;
using System.Collections.Generic;
using Misc;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem.HID;
using UnityEngine.UI;

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

    public loadType lt;

    public TextMeshProUGUI text;
    
    public int pathNumber;

    private void Start()
    {
        string myPath = SaveHandler.saveHandler.saveSlots[pathNumber];
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
        SaveHandler.saveHandler.NewGame(SaveHandler.saveHandler.saveSlots[pathNumber]);
        SaveHandler.saveHandler.Load();
        SceneLoadWrapper.sceneLoadWrapper.LoadScene("Modern Map");
    }
}
