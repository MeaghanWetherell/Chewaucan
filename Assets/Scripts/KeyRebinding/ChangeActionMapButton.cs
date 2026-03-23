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
        //static event so that all buttons except the active one get set interactable on click
        private static UnityEvent clicked;

        //binding factory ref
        [NonSerialized] public InstantiateKeyRebinds parent;

        //the action map corresponding to this button
        [NonSerialized] public InputActionMap map;

        [Tooltip("Text of this button")]
        public TextMeshProUGUI text;
        
        [Tooltip("Ref to the button component this corresponds to")]
        public Button button;

        public void Awake()
        {
            clicked ??= new UnityEvent();
            clicked.AddListener(SetInteractable);
        }

        //when clicked set all buttons but this one interactable, then tell the binding button factory to make buttons corresponding to this map
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
