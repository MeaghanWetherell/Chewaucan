using System.Collections.Generic;
using UnityEngine;

namespace Narration.Triggers
{
    public class PlayRandomlyOnCollisionEnter : MonoBehaviour
    {
        public List<Narration> clips;
        public void OnCollisionEnter(Collision other)
        {
            //Debug.Log("Collided");
            if (!other.gameObject.CompareTag("Player"))
                return;
            Narration clip = clips[Random.Range(0, clips.Count)];
            if (clip.GetPlayability())
            {
                clip.Begin();
            }
        }
    }
}
