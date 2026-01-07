using ScriptTags;
using UnityEngine;

namespace Interactables
{
    public class InteractRaycaster : MonoBehaviour
    {
        public PlayerMovementController moveContoller;
        
        private Interactable lastInter;
        private void FixedUpdate()
        {
            if (!moveContoller.isActiveAndEnabled)
            {
                return;
            }
            var transform1 = transform;
            Physics.Raycast(transform1.position, transform1.forward, out var hit, 5);
            if (hit.collider == null)
            {
                if(lastInter != null)lastInter.OnInteractDisable();
                lastInter = null;
                return;
            }
            Interactable listener = hit.collider.GetComponent<Interactable>();
            if (listener != null)
            {
                if (lastInter == null || !ReferenceEquals(lastInter, listener))
                {
                    if(lastInter != null)lastInter.OnInteractDisable();
                    listener.OnInteractEnable();
                    lastInter = listener;
                }
            }
            else if(lastInter != null)
            {
                lastInter.OnInteractDisable();
                lastInter = null;
            }
        }
    }
}
