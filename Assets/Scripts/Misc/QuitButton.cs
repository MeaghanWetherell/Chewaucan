using UnityEngine;

namespace Misc
{
    //quit game on click
    public class QuitButton : MonoBehaviour
    {
        public void OnClick()
        {
            Application.Quit();
        }
    }
}
