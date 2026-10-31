using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MainMenuButtonsTextSetter : MonoBehaviour
{
    public enum Menu{Main, Options, Graphics, Controls, Audio, Language, Help, Level, Saves, Map};
    public Menu menuType;
    public int buttonIndex;
    private TextMeshProUGUI textMesh;
    private string text;

    void OnEnable()
    {
        textMesh = GetComponent<TextMeshProUGUI>();
        switch (menuType)
        {
            case Menu.Main:
                text = LanguageManager.GetLanguageObj().mainMenuButtonArray[buttonIndex];
                break;
            case Menu.Options:
                text = LanguageManager.GetLanguageObj().optionsMenuButtonArray[buttonIndex];
                break;
            case Menu.Graphics:
                text = LanguageManager.GetLanguageObj().graphicsMenuText[buttonIndex];
                break;
            case Menu.Controls:
                text = LanguageManager.GetLanguageObj().controlsMenuText[buttonIndex];
                break;
            case Menu.Audio:
                text = LanguageManager.GetLanguageObj().audioMenuText[buttonIndex];
                break;
            case Menu.Language:
                text = LanguageManager.GetLanguageObj().optionsMenuButtonArray[3];
                break;
            case Menu.Help:
                text = LanguageManager.GetLanguageObj().helpMenuText[buttonIndex];
                break;
            case Menu.Level:
                text = LanguageManager.GetLanguageObj().levelSelectText[buttonIndex];
                break;
            case Menu.Saves:
                text = LanguageManager.GetLanguageObj().savesMenuText[buttonIndex];
                break;
            case Menu.Map:
                text = LanguageManager.GetLanguageObj().mapMenuText[buttonIndex];
                break;
        }
        textMesh.text = text;
    }
}
