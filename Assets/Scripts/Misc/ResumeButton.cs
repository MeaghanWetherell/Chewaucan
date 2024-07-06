using LoadGUIFolder;
using UnityEngine;

namespace Misc
{
    //resume game on click
    public class ResumeButton : MonoBehaviour
    {
        public void OnClick()
        {
            GameObject.Find("PauseManager").GetComponent<LoadGUI>().ONOpenTrigger();
        }
    }
}
