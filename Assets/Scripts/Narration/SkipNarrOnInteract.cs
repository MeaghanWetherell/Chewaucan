using System;
using Audio;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Narration
{
    //when the interact key is pressed, skip the current narration
    public class SkipNarrOnInteract : MonoBehaviour
    {
        public InputActionReference interactKey;

        [Tooltip("Text for the text prompt")]public TextMeshProUGUI text;

        //sets the text prompt letting the player know they can skip the narration
        private void Awake()
        {
            text.text = "Press " + GetInteractKeyName() + " to Skip Narration";
        }

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
        
        private string GetInteractKeyName()
        {
            PlayerInput playerInput = GameObject.FindWithTag("Player").GetComponent<PlayerInput>();
            for (int i = 0; i < interactKey.action.bindings.Count; i++)
            {
                if (!interactKey.action.bindings[i].groups.Contains(playerInput.currentControlScheme))
                    continue;
                return interactKey.action.bindings[i].ToDisplayString();
            }

            return null;
        }
    }
}
