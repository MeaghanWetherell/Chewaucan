using System.Collections;
using System.Collections.Generic;
using LoadGUIFolder;
using Misc;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class YNPopUpManager : PopUpManager
{
    [Tooltip("Runs when the user presses confirm. OnClose WILL NOT run in this case")]
    public UnityEvent<string> onConfirm;

    [Tooltip("Text on the decline button")]
    public TextMeshProUGUI decButtText;

    [Tooltip("Text on the confirm button")]
    public TextMeshProUGUI confButtText;

    public override void SetText(string inTitle, string mainText)
    {
        SetText(inTitle, mainText, "Decline", "Confirm");
    }

    public void SetText(string inTitle, string mainText, string decText, string confText)
    {
        base.SetText(inTitle, mainText);
        decButtText.text = decText;
        confButtText.text = confText;
    }

    public void OnConfirm()
    {
        onConfirm.Invoke(title);
        LoadGUIManager.loadGUIManager.ClosePopUp(index);
    }
}
