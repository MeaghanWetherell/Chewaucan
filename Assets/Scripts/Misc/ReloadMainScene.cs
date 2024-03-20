using UnityEngine;
using UnityEngine.Events;

namespace Misc
{
    public class ReloadMainScene : MonoBehaviour
    {
        public void OnClick()
        {
            SceneLoadWrapper.sceneLoadWrapper.LoadScene("Modern Map");
        }
    }
}
