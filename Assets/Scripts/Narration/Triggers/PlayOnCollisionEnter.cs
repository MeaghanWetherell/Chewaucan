using UnityEngine;

namespace Narration.Triggers
{
    public class PlayOnCollisionEnter : MonoBehaviour
    {
        public Narration clip;
        protected virtual void OnCollisionEnter(Collision other)
        {
            if (clip.GetPlayability())
            {
                clip.Begin();
                clip.SetPlayability(false);
            }
        }
    }
}
