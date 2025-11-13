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

        public override void Begin(bool skippable = true)
        {
            MoveStop();
            List<UnityAction<string>> onComplete = new List<UnityAction<string>> {OnComplete};
            base.Begin(onComplete);
        }

        private void OnComplete(string title)
        {
            Player.player.GetComponent<LandMovement>().enabled = true;
        }

        private void MoveStop()
        {
            Player.player.GetComponent<LandMovement>().enabled = false;
        }
    }
}
