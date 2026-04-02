using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenLinkOnButtonPress : MonoBehaviour
{
    [Tooltip("Url to open on press")]
    public string url;
    
    public void OnClick()
    {
        Application.OpenURL(url);
    }
}
