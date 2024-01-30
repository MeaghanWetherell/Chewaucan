using System;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

namespace Audio
{
    public class ChangeVol : MonoBehaviour
    {
        [Tooltip("The index in the vol list this slider should change. In order: master, music, effects")]
        public int index;

        private void OnEnable()
        {
            GetComponent<Slider>().value = SoundManager.soundManager.GetVol(index);
        }

        public void SetLevel(float sliderVal)
        {
            SoundManager.soundManager.SetVol(index, sliderVal);
        }
    }
}
