using System;
using System.IO;
using UnityEngine;
using TMPro;

public class PopulateWithTextFile : MonoBehaviour
{
    public enum TextContents{Credits};
    public TextContents textContents;
    private string textAssetPath;
    private TextMeshProUGUI textMesh;
    private string text = "";

    private void OnEnable()
    {
        textMesh = GetComponent<TextMeshProUGUI>();
        switch (textContents)
        {
            case TextContents.Credits:
                textAssetPath =
                    "Assets/Scripts/Language/Resources/TextFiles/"+LanguageManager.GetLanguageObj().name+"/Credits.txt";
                break;
        }
        
        StreamReader streamReader = new StreamReader(textAssetPath);
        while(!streamReader.EndOfStream)
        {
            text += streamReader.ReadLine();
            text += "\n";
        }

        streamReader.Close(); 
        
        textMesh.text = text;
    }


}
