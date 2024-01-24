using UnityEngine;

namespace Misc
{
    //resume game on click
    public class ResumeButton : MonoBehaviour
    {
        public void OnClick()
        {
            PauseMenu.pauseMenu.ONOpenTrigger();
        }
    }
}
