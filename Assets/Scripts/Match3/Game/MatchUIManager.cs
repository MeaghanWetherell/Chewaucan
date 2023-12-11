using System;
using TMPro;
using UnityEngine;

// ReSharper disable once CheckNamespace
namespace Match3
{
    public class MatchUIManager : MonoBehaviour
    {
        [Tooltip("Reference to the hidden UI object that displays win/loss at game end")]public GameObject uiObj;
    
        [Tooltip("Main text of the uiObj")]public TextMeshProUGUI mainText;

        [Tooltip("Text on the button on the uiObj")]public TextMeshProUGUI buttonText;

        [Tooltip("Text on the uiObj that should display final score")]public TextMeshProUGUI scoreText;

        public static MatchUIManager matchUIManager;

        private void Awake()
        {
            matchUIManager = this;
            uiObj.SetActive(false);
        }

        //ends the match 3 game and displays results, including the passed reason for game end
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

        //loses the game and displays the passed loss reason
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

        //wins the game and displays the passed win reason
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
