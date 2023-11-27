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

        private int _listenerIndexToCall;

        private ObjectIDGenerator _gen;

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
            _gen = new ObjectIDGenerator();
        }

        public bool ChangeListener(IListener toChange, int priority = 0, int indexToCall = 0)
        {
            bool temp = true;
            if (_currentListener == null || priority >= _listenerPriority)
            {
                _currentListener?.ListenerRemoved();
                _currentListener = toChange;
                _listenerPriority = priority;
                _listenerIndexToCall = indexToCall;
                _gen.GetId(toChange, out temp);
                HUDManager.hudManager.DisplayMessageToHUD("Press E to interact");
                return true;
            }
            return false;
        }

        public void DeRegister(IListener toDeregister)
        {
            if (_currentListener == null)
                return;
            bool temp = true;
            if (_gen.GetId(_currentListener, out temp) == _gen.GetId(toDeregister, out temp))
            {
                _currentListener = null;
                HUDManager.hudManager.CloseMessage();
            }
        }

        private void UpdateListener(InputAction.CallbackContext callbackContext)
        {
            _currentListener?.Listen(_listenerIndexToCall);
        }
    }
}
