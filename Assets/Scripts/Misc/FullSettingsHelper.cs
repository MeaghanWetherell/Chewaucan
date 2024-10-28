using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class FullSettingsHelper : MonoBehaviour
{
    public Canvas toDisable;

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoad;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoad;
    }
    
    private void OnSceneLoad(Scene loaded, LoadSceneMode mode)
    {
        Button button = GameObject.Find("BackButton")?.GetComponent<Button>();
        if (button != null)
        {
            button.onClick.RemoveAllListeners();
            toDisable.gameObject.SetActive(false);
            button.onClick.AddListener(Back);
        }
        
    }

    private void Back()
    {
        SceneManager.LoadScene("SettingsFull");
    }
}
