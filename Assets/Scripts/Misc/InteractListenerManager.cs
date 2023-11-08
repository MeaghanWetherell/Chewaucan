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

        private IListener currentListener;

        private int listenerPriority;

        private int listenerIndexToCall;

        private ObjectIDGenerator gen;

        private void OnEnable()
        {
            interact.action.started += updateListener;
        }

        private void OnDisable()
        {
            interact.action.started -= updateListener;
        }

        private void Awake()
        {
            interactListenerManager = this;
            gen = new ObjectIDGenerator();
        }

        public bool changeListener(IListener toChange, int priority = 0, int indexToCall = 0)
        {
            bool temp = true;
            if (currentListener == null || priority >= listenerPriority)
            {
                currentListener?.listenerRemoved();
                currentListener = toChange;
                listenerPriority = priority;
                listenerIndexToCall = indexToCall;
                gen.GetId(toChange, out temp);
                HUDManager.hudManager.sendMessage("Press E to interact");
                return true;
            }
            return false;
        }

        public void deRegister(IListener toDeregister)
        {
            if (currentListener == null)
                return;
            bool temp = true;
            if (gen.GetId(currentListener, out temp) == gen.GetId(toDeregister, out temp))
            {
                currentListener = null;
                HUDManager.hudManager.closeMessage();
            }
        }

        private void updateListener(InputAction.CallbackContext callbackContext)
        {
            currentListener?.listen(listenerIndexToCall);
        }
    }
}
