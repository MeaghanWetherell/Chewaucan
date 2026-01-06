using System;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

namespace Audio
{
    public class ChangeVol : MonoBehaviour
    {
        [Tooltip("The index in the vol list this slider should change. In order: master, narration, music, effects")]
        public int index;

        private void OnEnable()
        {
            float vol = SoundManager.soundManager.GetVol(index);
            Slider slider = GetComponent<Slider>();
            slider.value = vol;
        }

        public void SetLevel(float sliderVal)
        {
            SoundManager.soundManager.SetVol(index, sliderVal);
        }
    }
}
