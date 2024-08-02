using UnityEngine;

namespace Misc
{
    public class FadeInOnStart : MonoBehaviour
    {
        public FadeFromBlack fader;

        public float duration;
        
        // Start is called before the first frame update
        void Start()
        {
            fader.FadeIn(duration);
        }
    }
}
