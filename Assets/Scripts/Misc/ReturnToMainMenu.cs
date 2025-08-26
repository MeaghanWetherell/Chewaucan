using System.Collections;
using System.Collections.Generic;
using Audio;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ReturnToMainMenu : MonoBehaviour
{
    public void OnClick()
    {
        SoundManager.soundManager.ClearSoundManager();
        PlayerPositionManager.playerPositionManager.Reset();
        SaveHandler.saveHandler.Save();
        SceneManager.LoadScene(0);
    }
}
