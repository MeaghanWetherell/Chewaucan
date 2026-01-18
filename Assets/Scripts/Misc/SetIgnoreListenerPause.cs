using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetIgnoreListenerPause : MonoBehaviour
{
    private void Awake()
    {
        AudioSource sound = GetComponent<AudioSource>();
        sound.ignoreListenerPause = true;
    }
}
