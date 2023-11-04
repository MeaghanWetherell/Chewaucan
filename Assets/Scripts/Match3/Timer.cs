using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Timer : MonoBehaviour
{
    public float timeLeft;

    public TextMeshProUGUI text;

    private void Awake()
    {
        StartCoroutine(time());
    }

    private IEnumerator time()
    {
        while (timeLeft > 0)
        {
            yield return new WaitForSeconds(0.1f);
            timeLeft -= 0.1f;
            text.text = "Time Remaining: " + ((int) timeLeft) + "s";
        }
        MatchUIManager.matchUIManager.endGame("Time up!");
    }
}
