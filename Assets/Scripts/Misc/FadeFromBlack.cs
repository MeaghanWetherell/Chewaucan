using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Misc
{
    public class FadeFromBlack : MonoBehaviour
    {
        //fades will look shorter than expected because low alpha is barely noticeable. a slightly non-linear fade partially accounts for this
        public Image fader;

        //fade from black over the specified time period. 
        public void FadeIn(float duration)
        {
            if (duration <= 0) duration = 2;
            Color color = Color.black;
            fader.color = color;
            StartCoroutine(Fade(duration, 1));
        }
        
        //fade to black over the specified time period. 
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
            float inc;
            float cur = 0;
            switch (fadeMod)
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
            Color color;
            inc = (fadeMod)/duration;
            while (duration > 0)
            {
                yield return new WaitForSeconds(0.05f);
                duration -= 0.05f;
                color = Color.black;
                cur -= inc*0.05f;
                switch (cur)
                {
                  case > 1:
                      break;
                  case < 0:
                      break;
                }
                color.a = cur;
                fader.color = color;
            }
            switch (fadeMod)
            {
                case < 0:
                    cur = 1;
                    break;
                case > 0:
                {
                    cur = 0;
                    break;
                }
            }
            color = Color.black;
            color.a = cur;
            fader.color = color;
        }
    }
}
