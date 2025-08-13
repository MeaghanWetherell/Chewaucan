using System.Collections.Generic;
using UnityEngine;

namespace Narration.Triggers
{
    public class PlayRandomlyOnCollisionEnter : MonoBehaviour, IControllerCollisionEnter
    {
        public List<Narration> clips;

        public void OnControllerCollisionEnter(GameObject collision)
        {
            //Debug.Log("Collided");
            if (!collision.gameObject.CompareTag("Player"))
                return;
            Narration clip = clips[Random.Range(0, clips.Count)];
            if (clip.GetPlayability())
            {
                clip.Begin();
            }
        }
    }
}
