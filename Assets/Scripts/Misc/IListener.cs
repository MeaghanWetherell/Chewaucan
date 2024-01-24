using UnityEngine;

namespace Misc
{
    public interface IListener
    {
        public void Listen(int index);

        public void ListenerRemoved();
    }
}
