using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadSceneOnClick : MonoBehaviour
{
    public string toLoad;

    public void OnClick()
    {
        SceneManager.LoadScene(toLoad);
    }

    public void AdditiveOnClick()
    {
        SceneManager.LoadScene(toLoad, LoadSceneMode.Additive);
    }
}
