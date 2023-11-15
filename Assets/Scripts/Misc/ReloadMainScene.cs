using UnityEngine;

namespace Misc
{
    public class ReloadMainScene : MonoBehaviour
    {
        public void OnClick()
        {
            MainSceneDataSaver.mainSceneDataSaver.reloadMainScene();
        }
    }
}
