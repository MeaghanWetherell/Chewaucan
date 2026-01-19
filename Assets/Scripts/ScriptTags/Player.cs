using System;
using UnityEngine;

namespace ScriptTags
{
    public class Player : MonoBehaviour
    {
        public static GameObject player;

        public static Player playerA;

        public AudioSource AHHHAudioSource;
        void OnEnable()
        {
            player = this.gameObject;
            playerA = this;
        }

        private void OnDisable()
        {
            player = null;
            playerA = null;
        }

        public void PlayAHHH()
        {
            AHHHAudioSource.Play();
        }
    }
}
