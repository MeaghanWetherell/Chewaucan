using System.IO;
using UnityEngine;
using TMPro;

public class PoplateHelpMenus : MonoBehaviour
{
    public enum HelpMenu{FastTravel, Flies, Goal, Astrolabe};
    public HelpMenu helpMenuType;
    private string textAssetPath;
    public TextMeshProUGUI topText;
    public TextMeshProUGUI leftText;
    public TextMeshProUGUI rightText;
    private int curTextBox = 0;
    private string curLineText = "";

    private void OnEnable()
    {
        switch (helpMenuType)
        {
            case HelpMenu.FastTravel:
                textAssetPath =
                    "Assets/Scripts/Language/Resources/TextFiles/"+LanguageManager.GetLanguageObj().name+"/FastTravelHelp.txt";
                break;
            case HelpMenu.Flies:
                textAssetPath =
                    "Assets/Scripts/Language/Resources/TextFiles/"+LanguageManager.GetLanguageObj().name+"/FliesHelp.txt";
                break;
            case HelpMenu.Goal:
                textAssetPath =
                    "Assets/Scripts/Language/Resources/TextFiles/"+LanguageManager.GetLanguageObj().name+"/GoalMenuHelp.txt";
                break;
            case HelpMenu.Astrolabe:
                textAssetPath =
                    "Assets/Scripts/Language/Resources/TextFiles/"+LanguageManager.GetLanguageObj().name+"/AstrolabeHelp.txt";
                break;
        }

        if (topText != null)
        {
            topText.text = "";
        }
        if (leftText != null)
        {
            leftText.text = "";
        }
        if (rightText != null)
        {
            rightText.text = "";
        }

        curTextBox = 0;
        
        StreamReader streamReader = new StreamReader(textAssetPath);
        while(!streamReader.EndOfStream)
        {
            curLineText = streamReader.ReadLine();
            if(curLineText == "#")
            {
                curTextBox++;
                continue;
            }
            switch (curTextBox)
            {
                case 0:
                    topText.text += curLineText;
                    topText.text += "\n";
                    break;
                case 1:
                    leftText.text += curLineText;
                    leftText.text += "\n";
                    break;
                case 2:
                    rightText.text += curLineText;
                    rightText.text += "\n";
                    break;
            }
        }

        streamReader.Close(); 
    }
}
