using System;
using System.Text.RegularExpressions;
using KeyRebinding;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Match3
{
    public class HelpMenuTextSetter : MonoBehaviour
    {
        public TextMeshProUGUI maintext;
        public TextMeshProUGUI controlsText;

        private void Awake()
        {
            string addStr = "";
            if (MatchUIManager.matchUIManager != null)
            {
                LevelData data = MatchLevelManager.matchLevelManager.GETCurLevel();
                switch (data.matchType)
                {
                    case MatchObject.MatchType.identical:
                        addStr = "match identical bones!";
                        break;
                    case MatchObject.MatchType.sameBone:
                        addStr = "match bones of the same type (e.g. skull to skull, femur to femur)";
                        break;
                    case MatchObject.MatchType.sameSpecies:
                        addStr = "match bones from the same species!";
                        break;
                }
                maintext.text += "\nCurrently: " + addStr;
            }
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
            addStr += ": pan the secondary camera";
            controlsText.text += addStr;
        }
    }
}
