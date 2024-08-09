using System.Collections;
using System.Collections.Generic;
using Audio;
using Misc;
using ScriptTags;
using UnityEngine;
using UnityEngine.Events;

namespace Narration
{
    [CreateAssetMenu(menuName = "Narration/DemoStart")]
    public class DemoStartNarration : Narration
    {
        public Narration FQ2;
        
        private FadeFromBlack fader;
        
        private bool beStopped;

        public override void Begin()
        {
            beStopped = true;
            GameObject temp = GameObject.Find("Fader");
            if (temp == null) return;
            fader = temp.GetComponent<FadeFromBlack>();
            if (fader == null) return;
            fader.FadeIn(12f);
            fader.StartCoroutine(StayStopped());
            Stop();
            List<UnityAction<string>> onComplete = new List<UnityAction<string>> {OnComplete};
            base.Begin(onComplete);
        }

        private void OnComplete(string title)
        {
            beStopped = false;
            FQ2.SetPlayability(true);
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
