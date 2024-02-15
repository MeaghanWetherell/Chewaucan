using UnityEngine;

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
