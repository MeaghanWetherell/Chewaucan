using System;
using UnityEngine;

namespace Misc
{
    public class Wall : MonoBehaviour
    {
        [Tooltip("this corresponds to groups of walls in the Pliestocene, facilitating removal based on quest progress")]public String myId;

        private void Awake()
        {
            if(!PWallManager.wallManager.checkID(myId)) Destroy(this.gameObject);
        }
    }
}
