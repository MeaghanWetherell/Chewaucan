using System;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework.Internal;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

//will not work for multibind actions like movement
public class TextUpdateWithRebinds : MonoBehaviour
{
    public TextMeshProUGUI textMesh;

    private static InputActionAsset mainAsset;
    
    // Start is called before the first frame update
    void Start()
    {
        if (mainAsset == null)
            mainAsset = Resources.Load<InputActionAsset>("ActionMap");
        UpdateText();
    }

    private void OnEnable()
    {
        if (mainAsset == null)
            mainAsset = Resources.Load<InputActionAsset>("ActionMap");
        UpdateText();
    }

    void UpdateText()
    {
        string text = textMesh.text;
        string newText = "";
        int charNum = 0;
        while (charNum < text.Length)
        {
            char ch = text[charNum];
            if (ch.Equals('<'))
            {
                int initCharNum = charNum;
                charNum++;
                string replaceAction = "";
                char ch2 = ' ';
                while (charNum < text.Length)
                {
                    ch2 = text[charNum];
                    if (ch2 == '>')
                        break;
                    replaceAction += ch2;
                    charNum++;
                }
                if (ch2 != '>')
                {
                    newText += '>';
                    charNum = initCharNum;
                    continue;
                }
                newText += GetActionText(replaceAction);
            }
            else
            {
                newText += ch;
            }
            charNum++;
        }

        textMesh.text = newText;
    }

    string GetActionText(string action)
    {
        PlayerInput playerInput = GameObject.FindWithTag("Player")?.GetComponent<PlayerInput>();
        if (playerInput == null)
            return "";
        action = action.ToLower();
        foreach (InputActionMap map in mainAsset.actionMaps)
        {
            foreach (InputAction act in map.actions)
            {
                if (act.name.ToLower().Equals(action))
                {
                    for (int i = 0; i < act.bindings.Count; i++)
                    {
                        if (act.bindings[i].groups.Contains(playerInput.currentControlScheme))
                            return act.bindings[i].ToDisplayString();
                    }
                }
            }
        }
        return "";
    }
}
