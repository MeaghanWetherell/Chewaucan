using UnityEngine;

namespace Misc
{
    public interface IListener
    {
        public void listen(int index);

        public void listenerRemoved();
    }
}
