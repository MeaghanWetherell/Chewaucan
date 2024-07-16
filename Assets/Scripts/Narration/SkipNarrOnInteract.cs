using System;
using Audio;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Narration
{
    public class SkipNarrOnInteract : MonoBehaviour
    {
        public InputActionReference interactKey;

        private void OnEnable()
        {
            interactKey.action.performed += StopNarr;
        }

        private void OnDisable()
        {
            interactKey.action.performed -= StopNarr;
        }

        private void StopNarr(InputAction.CallbackContext context)
        {
            SoundManager.soundManager.StopNarration();
        }
    }
}
