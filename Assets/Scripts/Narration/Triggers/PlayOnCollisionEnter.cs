using UnityEngine;

namespace Narration.Triggers
{
    //plays a narration clip on collision enter
    public class PlayOnCollisionEnter : MonoBehaviour, IControllerCollisionEnter
    {
        [Tooltip("Clip to play")]
        public Narration clip;

        public void OnControllerCollisionEnter(GameObject collision)
        {
            if (!collision.gameObject.CompareTag("Player"))
                return;
            if (clip.GetPlayability())
            {
                clip.Begin();
                clip.SetPlayability(false);
            }
        }
    }
}
