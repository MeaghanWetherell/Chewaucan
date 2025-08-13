using UnityEngine;

namespace Narration.Triggers
{
    public class PlayOnCollisionEnter : MonoBehaviour, IControllerCollisionEnter
    {
        public Narration clip;

        public void OnControllerCollisionEnter(GameObject collision)
        {
            //Debug.Log("Collided with "+other.gameObject.name);
            if (!collision.gameObject.CompareTag("Player"))
                return;
            //Debug.Log(clip.name+" playability "+clip.GetPlayability());
            if (clip.GetPlayability())
            {
                clip.Begin();
                clip.SetPlayability(false);
            }
        }
    }
}
