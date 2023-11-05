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

        [System.NonSerialized]public float score;

        [System.NonSerialized]public float scoreRequired;

        [Tooltip("Total score to add per matched bone")]
        public float scorePerBone;

        [Tooltip("Multiplier to score for matches of >3, applied successively. ex. 0.15 for 15% bonus")]
        public float scoreMult;

        private bool isReq;

        private void Awake()
        {
            scoreTracker = this;
            scoreRequired = MatchLevelManager.matchLevelManager.getCurLevel().scoreReq;
            if (scoreRequired < 1)
            {
                isReq = true;
                text.text = "Score: 0";
                return;
            }
            text.text = "Score: 0"+"/"+scoreRequired;
            isReq = false;
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
            if (isReq)
            {
                text.text = "Score: " + score;
                return;
            }
            text.text = "Score: " + score +"/"+scoreRequired;
        }
    }
}
