using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrintClipDuration : MonoBehaviour
{
    public AudioClip clip;
    
    // Start is called before the first frame update
    void Start()
    {
       Debug.Log(clip.length); 
    }

}
