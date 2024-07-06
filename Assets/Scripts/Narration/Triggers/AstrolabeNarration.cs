using System;
using System.Collections;
using System.Collections.Generic;
using Audio;
using LoadGUIFolder;
using Misc;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace Narration
{
    public class AstrolabeNarration : MonoBehaviour
    {
        public Narration astroClip;

        public Narration pleistoClip;

        public InputActionReference astroOpened;

        private void Start()
        {
            if (!astroClip.GetPlayability()) return;
            LoadGUIManager.loadGUIManager.InstantiatePopUp("Open the Astrolabe!", "Press T to open the astrolabe.");
        }

        private void OnEnable()
        {
            astroOpened.action.started += Play;
        }

        private void OnDisable()
        {
            astroOpened.action.started -= Play;
        }

        private void Play(InputAction.CallbackContext callbackContext)
        {
            if (!astroClip.GetPlayability()) return;
            astroClip.SetPlayability(false);
            StartCoroutine(OverrideAudioPause());
            astroClip.Begin();
            pleistoClip.SetPlayability(true);
        }

        private IEnumerator OverrideAudioPause()
        {
            for (int i = 0; i < 100; i++)
            {
                AudioListener.pause = false;
                yield return new WaitForSeconds(0.1f);
            }
        }
    }
}
