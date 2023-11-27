using System;
using TMPro;
using UnityEngine;

// ReSharper disable once CheckNamespace
namespace Match3
{
    public class MatchUIManager : MonoBehaviour
    {
        public GameObject uiObj;
    
        public TextMeshProUGUI mainText;

        public TextMeshProUGUI buttonText;

        public TextMeshProUGUI scoreText;

        public static MatchUIManager matchUIManager;

        private void Awake()
        {
            matchUIManager = this;
            uiObj.SetActive(false);
        }

        public void EndGame(String reason)
        {
            uiObj.SetActive(true);
            Timer.timer.enabled = false;
            MatchGrid.matchGrid.enabled = false;
            if(ScoreTracker.scoreTracker.score < ScoreTracker.scoreTracker.scoreRequired)
                Lose(reason);
            else
            {
                Win(reason);
            }
        }

        private void Lose(String reason)
        {
            mainText.text = "You Lose!";
            buttonText.text = "Retry";
            scoreText.text = reason+"\n"+"Score: " + ScoreTracker.scoreTracker.score;
            int num = MatchLevelManager.matchLevelManager.GETCurLevel().levelNum;
            if (num > 0)
            {
                buttonText.gameObject.transform.parent.gameObject.GetComponent<PlayAgainButton>().index = num;
            }
            else
            {
                buttonText.text = "Play Again!";
            }
        }

        private void Win(String reason)
        {
            mainText.text = "You Win!";
            buttonText.text = "Next Level";
            scoreText.text = reason+"\n"+"Score: " + ScoreTracker.scoreTracker.score;
            int num = MatchLevelManager.matchLevelManager.GETCurLevel().levelNum;
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
}
