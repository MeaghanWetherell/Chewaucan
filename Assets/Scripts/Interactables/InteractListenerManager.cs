using System;
using System.Runtime.Serialization;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Misc
{
    public class InteractListenerManager : MonoBehaviour
    {
        public static InteractListenerManager interactListenerManager;

        public InputActionReference interact;

        private IListener _currentListener;

        private int _listenerPriority;

        private int _listenerCallCode;

        private void OnEnable()
        {
            interact.action.started += UpdateListener;
        }

        private void OnDisable()
        {
            interact.action.started -= UpdateListener;
        }

        private void Awake()
        {
            interactListenerManager = this;
        }

        public bool ChangeListener(IListener toChange, int priority = 0, int callCode = 0, string msg = "")
        {
            if (_currentListener == null || priority >= _listenerPriority)
            {
                _currentListener?.ListenerRemoved();
                _currentListener = toChange;
                _listenerPriority = priority;
                _listenerCallCode = callCode;
                String key = interact.action.bindings[0].ToDisplayString();
                if (msg.Equals(""))
                {
                    msg = "Press " + key + " to interact";
                }
                HUDManager.hudManager.DisplayMessageToHUD(msg);
                return true;
            }
            return false;
        }

        public void DeRegister()
        {
            _currentListener?.ListenerRemoved();
            _currentListener = null;
            HUDManager.hudManager.CloseMessage();
        }

        public void DeRegister(IListener toDeregister)
        {
            if (_currentListener == null)
                return;
            _currentListener.ListenerRemoved();
            if (ReferenceEquals(toDeregister, _currentListener))
            {
                _currentListener = null;
                HUDManager.hudManager.CloseMessage();
            }
        }

        private void UpdateListener(InputAction.CallbackContext callbackContext)
        {
            _currentListener?.Listen(_listenerCallCode);
        }
    }
}
