using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Narration.Triggers
{
    public class PlayRandomlyOnCollisionEnter : MonoBehaviour, IControllerCollisionEnter
    {
        public List<Narration> clips;

        private static float timeBetweenPlays = 10f;

        private static bool waitForNextPlay;

        private void Start()
        {
            waitForNextPlay = false;
        }

        public void OnControllerCollisionEnter(GameObject collision)
        {
            //Debug.Log("Collided");
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
