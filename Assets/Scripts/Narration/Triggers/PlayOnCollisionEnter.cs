using UnityEngine;

namespace Narration.Triggers
{
    public class PlayOnCollisionEnter : MonoBehaviour
    {
        public Narration clip;
        public void OnCollisionEnter(Collision other)
        {
            Debug.Log("Collided with "+other.gameObject.name);
            if (!other.gameObject.CompareTag("Player"))
                return;
            if (clip.GetPlayability())
            {
                clip.Begin();
                clip.SetPlayability(false);
            }
        }
    }
}
