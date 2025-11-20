using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Subtitler : MonoBehaviour
{
    public static GameObject subtitler;

    public GameObject subtitlerA;

    private void Awake()
    {
        subtitler = subtitlerA;
    }
}
