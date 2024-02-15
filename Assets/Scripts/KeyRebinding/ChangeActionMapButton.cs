using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace KeyRebinding
{
    public class ChangeActionMapButton : MonoBehaviour
    {
        private static UnityEvent clicked;

        [NonSerialized] public InstantiateKeyRebinds parent;

        [NonSerialized] public InputActionMap map;

        public TextMeshProUGUI text;
        
        public Button button;

        public void Awake()
        {
            clicked ??= new UnityEvent();
            clicked.AddListener(SetInteractable);
        }

        public void OnClick()
        {
            clicked.Invoke();
            button.interactable = false;
            parent.InstantiateRebinds(map);
        }
        
        private void SetInteractable()
        {
            button.interactable = true;
        }
    }
}
