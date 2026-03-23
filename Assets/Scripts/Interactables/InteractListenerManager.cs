using System;
using System.Runtime.Serialization;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Misc
{
    public class InteractListenerManager : MonoBehaviour
    {
        //singleton
        public static InteractListenerManager interactListenerManager;

        [Tooltip("Interact key")]
        public InputActionReference interact;

        //the current active listener. only one allowed at a time
        private IListener _currentListener;

        //priority of the current listener
        private int _listenerPriority;

        //integer to be passed back to the listener when called
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

        //sets the passed listener to current, discarding the old listener, if the old listener is lower or equal priority.
        //pass a message value to display a custom interact message to the hud. call code will be passed back to Listen() when the user presses interact
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

        //deregisters the current listener
        public void DeRegister()
        {
            _currentListener?.ListenerRemoved();
            _currentListener = null;
            HUDManager.hudManager.CloseMessage();
        }

        //deregisters the passed listener from the manager
        public void DeRegister(IListener toDeregister)
        {
            if (_currentListener == null)
                return;
            if (ReferenceEquals(toDeregister, _currentListener))
            {
                _currentListener.ListenerRemoved();
                _currentListener = null;
                HUDManager.hudManager.CloseMessage();
            }
        }

        //sends an update to the listener with its call code
        private void UpdateListener(InputAction.CallbackContext callbackContext)
        {
            _currentListener?.Listen(_listenerCallCode);
        }
    }
}
