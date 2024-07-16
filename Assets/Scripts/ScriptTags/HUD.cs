using System;
using UnityEngine;

namespace ScriptTags
{
    //use to find a canvas to place stuff on
    public class HUD : MonoBehaviour
    {
        public static HUD hud;
        private void Awake()
        {
            hud = this;
        }
    }
}
