using TMPro;
using UnityEngine;

namespace Match3
{
    //tracks the match3 game score and displays it to the UI
    public class ScoreTracker : MonoBehaviour
    {
        public static ScoreTracker scoreTracker;

        [Tooltip("Text on which to display score")]public TextMeshProUGUI text;

        [System.NonSerialized]public float score;

        [System.NonSerialized]public float scoreRequired;

        [Tooltip("Total score to add per matched bone")]
        public float scorePerBone;

        [Tooltip("Multiplier to score for matches of >3, applied successively. ex. 0.15 for 15% bonus")]
        public float scoreMult;

        private bool _isReq;

        private void Awake()
        {
            scoreTracker = this;
            scoreRequired = MatchLevelManager.matchLevelManager.GETCurLevel().scoreReq;
            if (scoreRequired < 1)
            {
                _isReq = true;
                text.text = "Score: 0"+"\n"+GetHighScoreText();
                return;
            }
            text.text = "Score: 0"+"/"+scoreRequired+"\n"+GetHighScoreText();
            _isReq = false;
        }

        //computes score for a certain count, adds it, and displays it
        public void AddScore(int boneCount)
        {
            score += 3 * scorePerBone;
            boneCount -= 3;
            float curMult = scoreMult;
            while (boneCount > 0)
            {
                boneCount--;
                score += scorePerBone * (1 + curMult);
                curMult += scoreMult;
            }
            if (_isReq)
            {
                text.text = "Score: " + score+"\n"+GetHighScoreText();
                return;
            }
            text.text = "Score: " + score +"/"+scoreRequired+"\n"+GetHighScoreText();
        }
        
        //Overload to add a specific score amount
        public void AddScore(float addScore)
        {
            score += addScore;
            if (_isReq)
            {
                text.text = "Score: " + score+"\n"+GetHighScoreText();
                return;
            }
            text.text = "Score: " + score +"/"+scoreRequired+"\n"+GetHighScoreText();
        }

        private string GetHighScoreText()
        {
            return "High Score: " + MatchLevelManager.matchLevelManager.GetHighscore();
        }
    }
}
