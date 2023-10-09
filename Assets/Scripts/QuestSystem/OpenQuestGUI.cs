using ScriptTags;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

namespace QuestSystem
{
    //handles opening and closing the quest gui
    public class OpenQuestGUI : MonoBehaviour
    {
        //store a ref to the HUD object
        private GameObject HUD;

        //action to open gui
        public InputActionReference openGUI;

        //whether the gui is currently open
        private bool guiOpen = false;

        private void Awake()
        {
            HUD = GameObject.Find("HUD");
        }

        private void OnEnable()
        {
            openGUI.action.performed += OnKPressed;
        }

        private void OnDisable()
        {
            openGUI.action.performed -= OnKPressed;
        }

        //if the quest gui is open, reenable player movement and close it, otherwise do the opposite
        private void OnKPressed(InputAction.CallbackContext callbackContext)
        {
            if (guiOpen)
            {
                Player.player.GetComponent<PlayerMovement>().enabled = true;
                Player.player.GetComponent<CameraLook>().enabled = true;
                SceneManager.UnloadSceneAsync("QuestGUI");
                HUD.SetActive(true);
            }
            else
            {
                Player.player.GetComponent<PlayerMovement>().enabled = false;
                Player.player.GetComponent<CameraLook>().enabled = false;
                HUD.SetActive(false);
                SceneManager.LoadScene("QuestGUI", LoadSceneMode.Additive);
            }
            guiOpen = !guiOpen;
        }
    }
}
