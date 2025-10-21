using System;
using System.Collections;
using Misc;
using TMPro;
using UnityEngine;

namespace Match3
{
    //times a match 3 game
    public class Timer : MonoBehaviour
    {
        [System.NonSerialized]public float timeLeft;

        [Tooltip("Text on which to display remaining time")]public TextMeshProUGUI text;

        public static Timer timer;

        //registers pause state, timer stops on pause
        private bool paused = false;

        private void Awake()
        {
            if (!MatchLevelManager.matchLevelManager.HasCurLevelBeenCompleted())
            {
                Destroy(this.gameObject);
                return;
            }
            timeLeft = MatchLevelManager.matchLevelManager.GETCurLevel().time;
            Timer.timer = this;
            if (timeLeft > 100000)
            {
                timeLeft = 0;
                StartCoroutine(TimeUp());
                return;
            }
            StartCoroutine(Time());
        }

        //subscribe to pause callbacks
        private void OnEnable()
        {
            PauseCallback.pauseManager.SubscribeToPause(OnPause);
            PauseCallback.pauseManager.SubscribeToResume(OnResume);
        }
        
        //unsubscribe to prevent leaks
        private void OnDisable()
        {
            PauseCallback.pauseManager.UnsubToPause(OnPause);
            PauseCallback.pauseManager.UnsubToResume(OnResume);
        }

        //increment time for endless mode
        private IEnumerator TimeUp()
        {
            while (true)
            {
                yield return new WaitForSeconds(0.1f);
                if (paused) continue;
                timeLeft += 0.1f;
                text.text = "Time: " + ((int) timeLeft) + "s";
            }
        }
        
        //decrements time, ends game on time up
        private IEnumerator Time()
        {
            while (timeLeft > 0)
            {
                yield return new WaitForSeconds(0.1f);
                if (paused) continue;
                timeLeft -= 0.1f;
                text.text = "Time Remaining: " + ((int) timeLeft) + "s";
            }
            MatchLevelManager.matchLevelManager.EndGame("Time up!");
        }

        private void OnPause()
        {
            paused = true;
        }

        private void OnResume()
        {
            paused = false;
        }
    }
}
