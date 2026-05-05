using System.Collections;
using System.Collections.Generic;
using Misc;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadSceneOnClick : MonoBehaviour
{
    public string toLoad;

    public void OnClick()
    {
        if (SceneLoadWrapper.sceneLoadWrapper != null)
        {
            SceneLoadWrapper.sceneLoadWrapper.LoadScene(toLoad);
        }
        else
        {
            SceneManager.LoadScene(toLoad);
        }
        
    }

    public void AdditiveOnClick()
    {
        SceneManager.LoadScene(toLoad, LoadSceneMode.Additive);
    }
}
