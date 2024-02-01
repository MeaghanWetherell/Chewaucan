using System;
using Misc;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

namespace KeyRebinding
{
    public class RebindKey : MonoBehaviour
    {
        public TextMeshProUGUI keyText;

        public TextMeshProUGUI mainText;

        [NonSerialized]public int index;
        
        [NonSerialized]public InputAction toRebind;

        private InputActionRebindingExtensions.RebindingOperation rebindOp;

        private void OnEnable()
        {
            BindingManager.bindingManager.bindChange.AddListener(OnRebound);
        }

        private void OnDisable()
        {
            BindingManager.bindingManager.bindChange.RemoveListener(OnRebound);
        }

        private void OnRebound()
        {
            keyText.text = toRebind.bindings[index].ToDisplayString();
        }

        public void StartRebind()
        {
            toRebind.Disable();
            
            rebindOp = toRebind.PerformInteractiveRebinding(index).WithControlsExcluding("<Mouse>/position").WithControlsExcluding("<Mouse>/delta").WithControlsExcluding("<Gamepad>/Start").WithControlsExcluding("<Keyboard>/escape").OnMatchWaitForAnother(0.1f).OnComplete(operation => RebindComplete());

            keyText.text = "Listening for input";
            
            rebindOp.Start();
        }

        private void RebindComplete()
        {
            BindingManager.bindingManager.SetBind(toRebind, index);
            
            toRebind.Enable();
        }
    }
}
