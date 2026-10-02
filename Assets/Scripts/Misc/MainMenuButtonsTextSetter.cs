using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MainMenuButtonsTextSetter : MonoBehaviour
{
    public int buttonIndex;
    private  TextMeshProUGUI textMesh;

    void OnEnable()
    {
        textMesh = GetComponent<TextMeshProUGUI>();
        textMesh.text = LanguageManager.GetLanguageObj().mainMenuButtonArray[buttonIndex];
    }
}
