using UnityEngine;

namespace Misc
{
    //LoadGUI+persistence and singleton
    public class PauseMenu : LoadGUI
    {
        public static PauseMenu pauseMenu;
        private void Awake()
        {
            if (pauseMenu != null)
            {
                Debug.LogError("Loaded persistent objects twice!");
                Destroy(pauseMenu.gameObject);
            }
            pauseMenu = this;
            DontDestroyOnLoad(this.gameObject);
        }
    }
}
