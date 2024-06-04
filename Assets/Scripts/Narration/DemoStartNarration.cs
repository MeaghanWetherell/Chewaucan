using System.Collections;
using System.Collections.Generic;
using Audio;
using Misc;
using ScriptTags;
using UnityEngine;
using UnityEngine.Events;

namespace Narration
{
    public class DemoStartNarration : Narration
    {
        public AudioClip startNarration;
        public FadeFromBlack fader;
        private bool beStopped = true;

        public override void Begin()
        {
            Stop();
            fader.FadeIn(-1);
            StartCoroutine(StayStopped());
            List<UnityAction> onComplete = new List<UnityAction>();
            onComplete.Add(OnComplete);
            SoundManager.soundManager.PlayNarration(startNarration, onComplete);
        }

        private void OnComplete()
        {
            beStopped = false;
            Player.player.GetComponent<LandMovement>().enabled = true;
        }

        private IEnumerator StayStopped()
        {
            while (beStopped)
            {
                Stop();
                yield return new WaitForSeconds(0);
            }
        }

        private void Stop()
        {
            Player.player.GetComponent<LandMovement>().enabled = false;
        }
    }
}
