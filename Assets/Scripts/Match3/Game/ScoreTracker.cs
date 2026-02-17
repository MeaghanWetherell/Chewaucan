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
            score = 0;
            scoreRequired = MatchLevelManager.matchLevelManager.GETCurLevel().scoreReq;
            if (scoreRequired < 1 || MatchLevelManager.matchLevelManager.HasCurLevelBeenCompleted())
            {
                _isReq = true;
                SetScoreText(true);
            }
            else
            {
                SetScoreText();
                _isReq = false;
            }
        }

        public void SetScoreText(bool hs=false)
        {
            text.text = "Score\n<style=\"Title\"><style=\"M3\">"+ score+"</style></style>\n";
            if (hs)
            {
                text.text += GetHighScoreText();
            }
            else
            {
                text.text += "out of\n<style=\"H3\"><style=\"M3\">" + scoreRequired + "</style></style>";
            }
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
                SetScoreText(true);
                return;
            }
            SetScoreText();
            if (score >= scoreRequired)
            {
                MatchLevelManager.matchLevelManager.EndGame("You reached the required score!");
            }
        }
        
        //Overload to add a specific score amount
        public void AddScore(float addScore)
        {
            score += addScore;
            if (_isReq)
            {
                SetScoreText(true);
                return;
            }
            SetScoreText();
            if (score >= scoreRequired)
            {
                MatchLevelManager.matchLevelManager.EndGame("You reached the required score!");
            }
        }

        private string GetHighScoreText()
        {
            return "High Score\n<style=\"H2\"><style=\"M3\">" + MatchLevelManager.matchLevelManager.GetHighscore()+"</style></style>";
        }
    }
}
