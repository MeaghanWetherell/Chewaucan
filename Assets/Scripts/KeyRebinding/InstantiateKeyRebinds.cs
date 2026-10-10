using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Users;
using UnityEngine.UI;

namespace KeyRebinding
{
    //script is attached to main scroll > viewport > content in keyrebinding scene
    public class InstantiateKeyRebinds : MonoBehaviour
    {
        public GameObject keyRebindGroupPrefab;

        public GameObject mapButtonPrefab;
    
        [Tooltip("Parent object under which to instantiate map buttons. only needed if useAltMaps is enabled")]
        public Transform mapButtonParent;

        [Tooltip("Anything that has a default binding that is on this list will be ineligble for rebinding.")]public List<String> invalidForRebinding;

        [Tooltip("Whether to add buttons for maps other than the main map")]
        public bool useAltMaps;

        //all maps
        private List<InputActionMap> maps;

        private PlayerInput playerInput;

        private void Awake()
        {
            //find the player and get the player input component
            playerInput = GameObject.FindWithTag("Player")?.GetComponent<PlayerInput>();
            if (playerInput == null)
                playerInput = GameObject.Find("SecondaryPInput").GetComponent<PlayerInput>();

            maps = BindingManager.bindingManager.maps;
            if (useAltMaps)
            {
                //instantiate map buttons
                GameObject mapButton = null;
                foreach (InputActionMap map in maps)
                {
                    mapButton = Instantiate(mapButtonPrefab, mapButtonParent);
                    ChangeActionMapButton clickScript = mapButton.GetComponent<ChangeActionMapButton>();
                    clickScript.parent = this;
                    clickScript.text.text = map.name;
                    clickScript.map = map;
                }
                if (mapButton != null)
                {
                    //instantiate rebind buttons for the most recent map
                    mapButton.GetComponent<ChangeActionMapButton>().OnClick();
                }
            }
            else
            {
                //instantiate rebind buttons for the main map
                InstantiateRebinds(maps[0]);
            }
        }

        //instantiate rebind buttons for all actions in the passed action map, as children of this object's transform
        public void InstantiateRebinds(InputActionMap map)
        {
            foreach (Transform child in transform)
            {
                Destroy(child.gameObject);
            }
            foreach (InputAction action in map)
            {
                for (int i = 0; i < action.bindings.Count; i++)
                {
                    if (!action.bindings[i].groups.Contains(playerInput.currentControlScheme))
                        continue;
                    if (invalidForRebinding.Contains(action.bindings[i].ToDisplayString()))
                    {
                        continue;
                    }
                    GameObject rebindGroup = Instantiate(keyRebindGroupPrefab, transform);
                    RebindWithGlyphs rebindKey = rebindGroup.GetComponent<RebindWithGlyphs>();
                    rebindKey.actionToRebind = action;
                    rebindKey.index = i;
                    rebindKey.UpdateGlyph();
                    if (map.name.Equals("Player"))
                    {
                        rebindKey.mainText.text = action.bindings[i].path switch
                        {
                            "<Keyboard>/w" => LanguageManager.GetLanguageObj().controlsMenuText[0], //"Move Forward"
                            "<Keyboard>/s" => LanguageManager.GetLanguageObj().controlsMenuText[1], //"Move Backward",
                            "<Keyboard>/a" => LanguageManager.GetLanguageObj().controlsMenuText[2], //"Strafe Left",
                            "<Keyboard>/d" => LanguageManager.GetLanguageObj().controlsMenuText[3], //"Strafe Right",
                            "<Keyboard>/q" => LanguageManager.GetLanguageObj().controlsMenuText[9], //"Turn Left",
                            "<Keyboard>/e" => LanguageManager.GetLanguageObj().controlsMenuText[8], //"Turn Right",
                            "<Keyboard>/upArrow" => LanguageManager.GetLanguageObj().controlsMenuText[0], //"Move Forward",
                            "<Keyboard>/downArrow" => LanguageManager.GetLanguageObj().controlsMenuText[1], //"Move Backward",
                            "<Keyboard>/leftArrow" => LanguageManager.GetLanguageObj().controlsMenuText[2], //"Strafe Left",
                            "<Keyboard>/rightArrow" => LanguageManager.GetLanguageObj().controlsMenuText[3], //"Strafe Right",
                            "<Keyboard>/space" => LanguageManager.GetLanguageObj().controlsMenuText[4], //"Jump",
                            "<Keyboard>/k" => LanguageManager.GetLanguageObj().controlsMenuText[5], //"Quest Menu",
                            "<Keyboard>/0" => LanguageManager.GetLanguageObj().controlsMenuText[6], //"Hide Map",
                            "<Keyboard>/t" => LanguageManager.GetLanguageObj().controlsMenuText[7], //"Astrolabe",
                            "<Keyboard>/leftShift" => LanguageManager.GetLanguageObj().controlsMenuText[8], //"Sprint",
                            "<Keyboard>/m" => LanguageManager.GetLanguageObj().controlsMenuText[11], //"Open Map",
                            "<Keyboard>/tab" => LanguageManager.GetLanguageObj().controlsMenuText[12], //"Interact",
                            "<Keyboard>/period" => LanguageManager.GetLanguageObj().controlsMenuText[13], //"Skip",
                            "<Keyboard>/x" => LanguageManager.GetLanguageObj().controlsMenuText[14], //"Flip Bone",
                            "<Keyboard>/y" => LanguageManager.GetLanguageObj().controlsMenuText[15], //"Open Bone UI",
                            "<Keyboard>/c" => LanguageManager.GetLanguageObj().controlsMenuText[16], //"Stop Climbing",
                            "<Keyboard>/l" => LanguageManager.GetLanguageObj().controlsMenuText[17], //"End Game",
                            _ => action.name
                        };
                    }
                    else
                    {
                        rebindKey.mainText.text = action.name;
                    }
                }
            }
            Canvas.ForceUpdateCanvases();
            LayoutRebuilder.ForceRebuildLayoutImmediate((RectTransform)transform);
        }
    }
}
