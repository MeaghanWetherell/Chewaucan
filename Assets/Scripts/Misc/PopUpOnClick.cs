
using UnityEngine;

namespace Misc
{
    public class PopUpOnClick : MonoBehaviour
    {
        public void OnClick()
        {
            Destroy(gameObject);
            if (GameObject.Find("PopUp") == null)
            {
                PauseCallback.pauseManager.Resume();
                GameObject hud = GameObject.Find("HUD");
                if(hud != null)
                    hud.SetActive(true);
            }
        }
    }
}
