using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Misc
{
    public class FadeFromBlack : MonoBehaviour
    {
        public Image fader;

        //fade from black over the specified time period. will end up somewhat shorter than specified
        public void FadeIn(float duration)
        {
            if (duration <= 0) duration = 2;
            Color color = Color.black;
            fader.color = color;
            StartCoroutine(Fade(duration, 1));
        }
        
        //fade to black over the specified time period. will end up somewhat shorter than specified
        public void FadeOut(float duration)
        {
            if (duration <= 0) duration = 2;
            Color color = Color.black;
            color.a = 0;
            fader.color = color;
            StartCoroutine(Fade(duration, -1));
        }
        
        //performs fading over time. fademod != 0, positive for fade in, negative for fade out
        private IEnumerator Fade(float duration, float fadeMod)
        {
            float inc = (1 / duration) * 0.1f*fadeMod;
            float cur = 0;
            switch (inc)
            {
                case < 0:
                    cur = 0;
                    break;
                case > 0:
                {
                    cur = 1;
                    break;
                }
            }
            while (duration > 0)
            {
                yield return new WaitForSeconds(0.1f);
                duration -= 0.1f;
                Color color = Color.black;
                cur -= inc;
                color.a = cur;
                fader.color = color;
                inc *= 1.1f;
            }
        }
    }
}
