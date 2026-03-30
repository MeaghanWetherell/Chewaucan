using ScriptTags;
using UnityEngine;

namespace Interactables
{
    public class InteractRaycaster : MonoBehaviour
    {
        [Tooltip("Ref to the move controller")]
        public PlayerMovementController moveContoller;
        
        //reference to the most recent interactable we hit
        private Interactable lastInter;
        private void FixedUpdate()
        {
            //do not cast a ray if movement is locked
            if (!moveContoller.isActiveAndEnabled)
            {
                return;
            }
            //cast the ray
            var transform1 = transform;
            Physics.Raycast(transform1.position, transform1.forward, out var hit, 5);
            //if we don't hit anything, remove the old interactable
            if (hit.collider == null)
            {
                if(lastInter != null)lastInter.OnInteractDisable();
                lastInter = null;
                return;
            }
            //if we hit an interactable, set it as the current interactable
            Interactable listener = hit.collider.GetComponent<Interactable>();
            if (listener != null)
            {
                Debug.Log("Hit an interactable");
                if (lastInter == null || !ReferenceEquals(lastInter, listener))
                {
                    if(lastInter != null)lastInter.OnInteractDisable();
                    listener.OnInteractEnable();
                    lastInter = listener;
                }
            }
            //if we hit a non-interactable, remove the old interactable
            else if(lastInter != null)
            {
                lastInter.OnInteractDisable();
                lastInter = null;
            }
        }
    }
}
