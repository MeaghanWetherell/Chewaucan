using UnityEngine;

namespace Misc
{
    public interface IListener
    {
        public void Listen(int callCode);

        public void ListenerRemoved();
    }
}
