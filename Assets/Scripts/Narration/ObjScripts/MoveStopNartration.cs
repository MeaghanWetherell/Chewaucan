using System.Collections;
using System.Collections.Generic;
using Audio;
using Misc;
using ScriptTags;
using UnityEngine;
using UnityEngine.Events;

namespace Narration
{
    [CreateAssetMenu(menuName = "Narration/moveStopping")]
    public class MoveStopNarration : Narration
    {
        private FadeFromBlack fader;
        
        private bool beStopped;

        public override void Begin()
        {
            beStopped = true;
            Stop();
            List<UnityAction<string>> onComplete = new List<UnityAction<string>> {OnComplete};
            base.Begin(onComplete);
        }

        private void OnComplete(string title)
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
