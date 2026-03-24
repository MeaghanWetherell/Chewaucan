using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Narration.Triggers
{
    public class PlayRandomlyOnCollisionEnter : MonoBehaviour, IControllerCollisionEnter
    {
        [Tooltip("Picks a random one of this clips to play on collision enter")]
        public List<Narration> clips;
        
        [Tooltip("Minimum time before other *instances of this script* will play again")]
        public float timeBetweenPlays = 10f;

        //Whether the script is able to play (if false) or not (if true)
        private static bool waitForNextPlay;

        private void Start()
        {
            waitForNextPlay = false;
        }

        public void OnControllerCollisionEnter(GameObject collision)
        {
            if (!collision.gameObject.CompareTag("Player"))
                return;
            if (waitForNextPlay) return;
            StartCoroutine(WaitForNextPlay());
            Narration clip = clips[Random.Range(0, clips.Count)];
            if (clip.GetPlayability())
            {
                clip.Begin();
            }
        }

        private IEnumerator WaitForNextPlay()
        {
            waitForNextPlay = true;
            yield return new WaitForSeconds(timeBetweenPlays);
            waitForNextPlay = false;
        }
    }
}
