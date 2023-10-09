using UnityEngine;

namespace ScriptTags
{
    public class Player : MonoBehaviour
    {
        public static GameObject player;
        void Awake()
        {
            player = this.gameObject;
        }
    }
}
