using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Users;

namespace KeyRebinding
{
    public class InstantiateKeyRebinds : MonoBehaviour
    {
        public GameObject keyRebindGroupPrefab;

        public GameObject mapButtonPrefab;

        public Transform mapButtonParent;

        public List<String> invalidForRebinding;

        private List<InputActionMap> maps;

        private void Awake()
        {
            maps = BindingManager.bindingManager.maps;
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

        public void InstantiateRebinds(InputActionMap map)
        {
            PlayerInput playerInput = GameObject.FindWithTag("Player").GetComponent<PlayerInput>();
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
                    RebindKey rebindKey = rebindGroup.GetComponent<RebindKey>();
                    rebindKey.toRebind = action;
                    rebindKey.index = i;
                    rebindKey.keyText.text = action.bindings[i].ToDisplayString();
                    rebindKey.mainText.text = action.name;
                    if (action.bindings[i].isPartOfComposite || action.bindings[i].isComposite)
                    {
                        rebindKey.mainText.text += action.bindings[i].GetNameOfComposite();
                    }
                }
            }
        }
    }
}
