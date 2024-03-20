using System;
using Match3.Game;
using TMPro;
using UnityEngine;

// ReSharper disable once CheckNamespace
namespace Match3
{
    public class MatchUIManager : MonoBehaviour
    {
        [Tooltip("Ref to the sound effect maker")] public MatchSoundEffects matchSound;
        
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

        //loses the game and displays the passed loss reason
        public void Lose(String reason)
        {
            uiObj.SetActive(true);
            matchSound.PlayAw();
            mainText.text = "You Lose!";
            buttonText.text = "Retry";
            scoreText.text = reason+"\n"+"Score: " + ScoreTracker.scoreTracker.score;
            int num = MatchLevelManager.matchLevelManager.GETCurLevel().levelNum;
            if (num < MatchLevelManager.matchLevelManager.levels.Count)
            {
                buttonText.gameObject.transform.parent.gameObject.GetComponent<PlayAgainButton>().index = num-1;
            }
            else
            {
                buttonText.gameObject.transform.parent.gameObject.GetComponent<PlayAgainButton>().index = num-1;
                buttonText.text = "Play Again!";
            }
        }

        //wins the game and displays the passed win reason
        public void Win(String reason)
        {
            uiObj.SetActive(true);
            matchSound.PlayYay();
            mainText.text = "You Win!";
            buttonText.text = "Next Level";
            scoreText.text = reason+"\n"+"Score: " + ScoreTracker.scoreTracker.score;
            int num = MatchLevelManager.matchLevelManager.GETCurLevel().levelNum;
            if (num > 0 && num < MatchLevelManager.matchLevelManager.levels.Count-1)
            {
                buttonText.gameObject.transform.parent.gameObject.GetComponent<PlayAgainButton>().index = num;
                MatchLevelManager.matchLevelManager.levelsComplete[num-1] = true;
            }
            else
            {
                buttonText.text = "Play Again!";
                buttonText.gameObject.transform.parent.gameObject.GetComponent<PlayAgainButton>().index = num-1;
            }
        }
    }
}
