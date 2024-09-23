using System;
using System.Collections;
using DigitalRuby.Tween;
using UnityEngine;
using UnityEngine.UI;

namespace Misc
{
    public class FadeFromBlack : MonoBehaviour
    {
        public Image fader;

        //fade from black over the specified time period. 
        public void FadeIn(float duration)
        {
            if (duration <= 0) duration = 2;
            Color color = Color.black;
            fader.color = color;
            Fade(duration, true);
        }
        
        //fade to black over the specified time period. 
        public void FadeOut(float duration)
        {
            if (duration <= 0) duration = 2;
            Color color = Color.black;
            color.a = 0;
            fader.color = color;
            Fade(duration, false);
        }
        
        //performs fading over time.
        private void Fade(float duration, bool fadeIn)
        {
            System.Action<ITween<Color>> updateColor = (t) =>
                {
                    try
                    {
                        fader.color = t.CurrentValue;
                    }
                    catch (Exception)
                    {
                        Debug.Log("Suppressing Tween err");
                    }
                };
                Color initial = Color.black;
                Color final = Color.black;
                if (fadeIn)
                {
                    final.a = 0;
                    gameObject.Tween("Fade", initial, final, duration, TweenScaleFunctions.CubicEaseIn, updateColor);
                }
                else
                {
                    initial.a = 0;
                    gameObject.Tween("Fade", initial, final, duration, TweenScaleFunctions.CubicEaseOut, updateColor);
                }

        }
    }
}
