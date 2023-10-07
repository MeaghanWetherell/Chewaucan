using ScriptTags;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

namespace QuestSystem
{
    public class OpenQuestGUI : MonoBehaviour
    {
        private GameObject HUD;

        public InputActionReference openGUI;

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
