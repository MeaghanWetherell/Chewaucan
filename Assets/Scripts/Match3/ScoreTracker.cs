using System;
using System.Collections;
using TMPro;
using UnityEngine;

namespace Match3
{
    public class ScoreTracker : MonoBehaviour
    {
        //singleton
        public static ScoreTracker scoreTracker;

        public TextMeshProUGUI text;

        public float score;

        public float scoreRequired;

        [Tooltip("Total score to add per matched bone")]
        public float scorePerBone;

        [Tooltip("Multiplier to score for matches of >3, applied successively. ex. 0.15 for 15% bonus")]
        public float scoreMult;

        private void Awake()
        {
            scoreTracker = this;
            text.text = "Score: 0"+"/"+scoreRequired;
        }

        public void addScore(int boneCount)
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
            text.text = "Score: " + score +"/"+scoreRequired;
            if (score >= scoreRequired)
            {
                StartCoroutine(win());
            }
        }

        private IEnumerator win()
        {
            yield return new WaitForSeconds(0.5f);
            MatchUIManager.matchUIManager.endGame("Score requirement met!");
        }
    }
}
