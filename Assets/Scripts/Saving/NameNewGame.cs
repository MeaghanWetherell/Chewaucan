using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using LoadGUIFolder;
using Misc;
using ScriptTags;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class NameNewGame : MonoBehaviour
{
    [Tooltip("Title text field")]
    public TextMeshProUGUI headerText;

    public TMP_InputField inputField;

    [Tooltip("Placeholder text field from the TMP input component")]
    public TextMeshProUGUI inputPlacehodler;
    
    private int pathNo;

    public List<string> reservedNames;

    //initializes the pop-up for the save slot specified by pathNumber
    public void Initialize(int pathNumber)
    {
        pathNo = pathNumber;
        headerText.text = "Enter name for save in slot " + (pathNumber+1) + " or leave blank to use existing name " +
                          SaveHandler.saveHandler.saveSlots[pathNumber].Split("/")[^1];
        inputPlacehodler.text = "Enter save name...";
        inputField.text = SaveHandler.saveHandler.saveSlots[pathNumber].Split("/")[^1];
    }
    
    public void Initialize()
    {
        pathNo = TrashSave.toTrash;
        headerText.text = "Enter name for save in slot " + (pathNo+1) + " or leave blank to use existing name " +
                          SaveHandler.saveHandler.saveSlots[pathNo].Split("/")[^1];
        inputPlacehodler.text = "Enter save name...";
        inputField.text = SaveHandler.saveHandler.saveSlots[pathNo].Split("/")[^1];
    }

    public void OnSelect()
    {
        if(Player.player != null)
            Player.player.GetComponent<PlayerInput>().enabled = false;
    }

    public void OnDeselect()
    {
        if (Player.player != null)
            Player.player.GetComponent<PlayerInput>().enabled = true;
    }

    //attempts to create a new save with the name in the input field, with validation.
    public void OnClick()
    {
        string inText = inputField.text.Trim();
        if (reservedNames.Contains(inText.ToLower()))
        {
            LoadGUIManager.loadGUIManager.InstantiatePopUp("Invalid Name", "File name cannot be reserved name "+inText);
            return;
        }
        if (inText[^1].Equals('.'))
        {
            LoadGUIManager.loadGUIManager.InstantiatePopUp("Invalid Name", "File name cannot end with .");
            return;
        }
        for (int i = 0; i < SaveHandler.saveHandler.saveSlots.Count; i++)
        {
            if (i != pathNo && inText.Equals(SaveHandler.saveHandler.saveSlots[i].Split("/")[^1]))
            {
                LoadGUIManager.loadGUIManager.InstantiatePopUp("Invalid Name", "A save with that name exists in a different slot!");
                return;  
            }
        }
        if (!inText.Equals(""))
        {
            if(Directory.Exists(Application.persistentDataPath+"/"+SaveHandler.saveHandler.saveSlots[pathNo]))
                Directory.Delete(Application.persistentDataPath+"/"+SaveHandler.saveHandler.saveSlots[pathNo], true);
            SaveHandler.saveHandler.saveSlots[pathNo] = "Chewaucan/"+inText;
        }
        SaveHandler.saveHandler.NewGame(SaveHandler.saveHandler.saveSlots[pathNo]);
        SaveHandler.saveHandler.Load();
        SceneLoadWrapper.sceneLoadWrapper.LoadScene("Modern Map");
    }
}
