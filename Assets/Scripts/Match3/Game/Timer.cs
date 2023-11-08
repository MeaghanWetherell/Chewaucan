using System.Collections;
using TMPro;
using UnityEngine;

namespace Match3
{
    public class Timer : MonoBehaviour
    {
        [System.NonSerialized]public float timeLeft;

        public TextMeshProUGUI text;

        public static Timer timer;

        private void Awake()
        {
            timeLeft = MatchLevelManager.matchLevelManager.getCurLevel().time;
            Timer.timer = this;
            if (timeLeft > 100000)
            {
                timeLeft = 0;
                StartCoroutine(timeUp());
                return;
            }
            StartCoroutine(time());
        }

        private IEnumerator timeUp()
        {
            while (true)
            {
                yield return new WaitForSeconds(0.1f);
                timeLeft += 0.1f;
                text.text = "Time: " + ((int) timeLeft) + "s";
            }
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
}
