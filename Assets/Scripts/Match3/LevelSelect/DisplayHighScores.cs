using System;
using System.Collections;
using System.Collections.Generic;
using Match3;
using TMPro;
using UnityEngine;

public class DisplayHighScores : MonoBehaviour
{
    public void OnEnable()
    {
        List<float> highscores = MatchLevelManager.matchLevelManager.GetAllHighscores();
        TextMeshProUGUI text = transform.GetComponentInChildren<TextMeshProUGUI>();
        text.text = LanguageManager.GetLanguageObj().highScores+":\n";
        for (int i = 0; i < highscores.Count-1; i++)
        {
            text.text += LanguageManager.GetLanguageObj().level + " " + (i + 1) + ": " + highscores[i] + "\n";
        }
        text.text += LanguageManager.GetLanguageObj().endless + ": " + highscores[^1];
    }
}
