using System.IO;
using KeyRebinding;
using Match3;
using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;

public class PoplateHelpMenus : MonoBehaviour
{
    public enum HelpMenu{FastTravelHelp, FliesHelp, GoalMenuHelp, AstrolabeHelp, Match3Help};
    public HelpMenu helpMenuType;
    private string textAssetPath;
    public TextMeshProUGUI topText;
    public TextMeshProUGUI leftText;
    public TextMeshProUGUI rightText;
    private int curTextBox = 0;
    private string curLineText = "";

    private void OnEnable()
    {
        textAssetPath =
                            "Assets/Scripts/Language/Resources/TextFiles/"+LanguageManager.GetLanguageObj().name+"/"+helpMenuType+".txt";

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
                    if (topText != null)
                    {
                        topText.text += curLineText;
                        topText.text += "\n";
                    }
                    break;
                case 1:
                    if (leftText != null)
                    {
                        leftText.text += curLineText;
                        leftText.text += "\n";
                    }

                    break;
                case 2:
                    if (rightText != null)
                    {
                        rightText.text += curLineText;
                        rightText.text += "\n";
                    }

                    break;
            }
        }

        streamReader.Close();

        if (helpMenuType == HelpMenu.Match3Help)
        {
            string addStr = "";
            if (MatchUIManager.matchUIManager != null)
            {
                LevelData data = MatchLevelManager.matchLevelManager.GETCurLevel();
                switch (data.matchType)
                {
                    case MatchObject.MatchType.identical:
                        addStr = LanguageManager.GetLanguageObj().matchIdentical; //"match identical bones!"
                        break;
                    case MatchObject.MatchType.sameBone:
                        addStr = LanguageManager.GetLanguageObj().matchType; //"match bones of the same type (e.g. skull to skull, femur to femur)"
                        break;
                    case MatchObject.MatchType.sameSpecies:
                        addStr = LanguageManager.GetLanguageObj().matchSpecies; //"match bones from the same species!"
                        break;
                }
                leftText.text += "\n"+ LanguageManager.GetLanguageObj().currently + ": " + addStr;
            }
            
            if (GameObject.FindWithTag("Player") != null)
            {
                PlayerInput playerInput = GameObject.FindWithTag("Player").GetComponent<PlayerInput>();
                addStr = "\n";
                bool first = true;
                foreach (InputActionMap map in BindingManager.bindingManager.maps)
                {
                    if (map.name.Equals("Player"))
                    {
                        foreach (InputAction action in map)
                        {
                            for (int i = 0; i < action.bindings.Count; i++)
                            {
                                if (!action.bindings[i].groups.Contains(playerInput.currentControlScheme))
                                    continue;
                                if (action.name.Equals("Move"))
                                {
                                    if (!first)
                                    {
                                        addStr += ", ";
                                    }
                                    else {first = false;}
                                    if (action.bindings[i].hasOverrides)
                                    {
                                        addStr += action.bindings[i].overridePath.Split("/")[1];
                                    }
                                    else
                                    {
                                        addStr += action.bindings[i].path.Split("/")[1];
                                    }
                                }
                            }
                        }
                    }
                }

                addStr += ": " + LanguageManager.GetLanguageObj().panCamera; //"pan the secondary camera"
                rightText.text += addStr;
            }
            
        }
    }
}
