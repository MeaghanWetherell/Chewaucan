using UnityEngine;

namespace Misc
{
    public class FadeInOnStart : MonoBehaviour
    {
        public FadeFromBlack fader;
        
        // Start is called before the first frame update
        void Start()
        {
            fader.FadeIn(0);
        }
    }
}
