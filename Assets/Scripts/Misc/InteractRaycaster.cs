using System;
using UnityEngine;

namespace Misc
{
    public class InteractRaycaster : MonoBehaviour
    {
        private void FixedUpdate()
        {
            RaycastHit hit;
            var transform1 = transform;
            Physics.Raycast(transform1.position, transform1.forward, out hit, 5);
            IListener listener = hit.collider.GetComponent<IListener>();
        }
    }
}
