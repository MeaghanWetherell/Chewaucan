using System;
using System.Collections;
using System.Collections.Generic;
using Match3;
using TMPro;
using UnityEngine;

public class MatchUIManager : MonoBehaviour
{
    public GameObject UIObj;
    
    public TextMeshProUGUI mainText;

    public TextMeshProUGUI buttonText;

    public TextMeshProUGUI scoreText;

    public static MatchUIManager matchUIManager;

    private void Awake()
    {
        matchUIManager = this;
        UIObj.SetActive(false);
    }

    public void endGame(String reason)
    {
        UIObj.SetActive(true);
        Timer.timer.enabled = false;
        MatchGrid.matchGrid.enabled = false;
        if(ScoreTracker.scoreTracker.score < ScoreTracker.scoreTracker.scoreRequired)
            lose(reason);
        else
        {
            win(reason);
        }
    }

    private void lose(String reason)
    {
        mainText.text = "You Lose!";
        buttonText.text = "Retry";
        scoreText.text = reason+"\n"+"Score: " + ScoreTracker.scoreTracker.score;
        int num = MatchLevelManager.matchLevelManager.getCurLevel().levelNum;
        if (num > 0)
        {
            buttonText.gameObject.transform.parent.gameObject.GetComponent<PlayAgainButton>().index = num;
        }
        else
        {
            buttonText.text = "Play Again!";
        }
    }

    private void win(String reason)
    {
        mainText.text = "You Win!";
        buttonText.text = "Next Level";
        scoreText.text = reason+"\n"+"Score: " + ScoreTracker.scoreTracker.score;
        int num = MatchLevelManager.matchLevelManager.getCurLevel().levelNum;
        if (num > 0 && num < MatchLevelManager.matchLevelManager.levels.Count-1)
        {
            buttonText.gameObject.transform.parent.gameObject.GetComponent<PlayAgainButton>().index = num+1;
            MatchLevelManager.matchLevelManager.levelsComplete[num] = true;
        }
        else
        {
            buttonText.text = "Play Again!";
            if(num>0)
                buttonText.gameObject.transform.parent.gameObject.GetComponent<PlayAgainButton>().index = num;
        }
    }
}
