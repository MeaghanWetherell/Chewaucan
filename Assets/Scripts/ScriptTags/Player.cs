using System;
using UnityEngine;

namespace ScriptTags
{
    public class Player : MonoBehaviour
    {
        public static GameObject player;
        void OnEnable()
        {
            player = this.gameObject;
        }

        private void OnDisable()
        {
            player = null;
        }
    }
}
