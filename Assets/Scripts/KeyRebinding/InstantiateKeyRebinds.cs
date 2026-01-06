using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Users;

namespace KeyRebinding
{
    //script is attached to main scroll > viewport > content in keyrebinding scene
    public class InstantiateKeyRebinds : MonoBehaviour
    {
        public GameObject keyRebindGroupPrefab;

        public GameObject mapButtonPrefab;

        public Transform mapButtonParent;

        [Tooltip("Anything that has a default binding that is on this list will be ineligble for rebinding.")]public List<String> invalidForRebinding;

        public bool useAltMaps;

        private List<InputActionMap> maps;

        private PlayerInput playerInput;

        private void Awake()
        {
            playerInput = GameObject.FindWithTag("Player")?.GetComponent<PlayerInput>();
            if (playerInput == null)
                playerInput = GameObject.Find("SecondaryPInput").GetComponent<PlayerInput>();

            maps = BindingManager.bindingManager.maps;
            if (useAltMaps)
            {
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
                    mapButton.GetComponent<ChangeActionMapButton>().OnClick();
                }
            }
            else
            {
                InstantiateRebinds(maps[0]);
            }
        }

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
                            "<Keyboard>/w" => "Move Forward",
                            "<Keyboard>/s" => "Move Backward",
                            "<Keyboard>/a" => "Strafe Left",
                            "<Keyboard>/d" => "Strafe Right",
                            "<Keyboard>/q" => "Turn Left",
                            "<Keyboard>/e" => "Turn Right",
                            "<Keyboard>/upArrow" => "Move Forward",
                            "<Keyboard>/downArrow" => "Move Backward",
                            "<Keyboard>/leftArrow" => "Strafe Left",
                            "<Keyboard>/rightArrow" => "Strafe Right",
                            _ => action.name
                        };
                    }
                    else
                    {
                        rebindKey.mainText.text = action.name;
                    }
                }
            }
        }
    }
}
