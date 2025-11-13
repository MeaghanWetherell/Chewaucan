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

        public override void Begin(bool skippable = true)
        {
            beStopped = true;
            MoveStop();
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
                MoveStop();
                yield return new WaitForSeconds(0);
            }
            
        }

        private void MoveStop()
        {
            Player.player.GetComponent<LandMovement>().enabled = false;
        }
    }
}
