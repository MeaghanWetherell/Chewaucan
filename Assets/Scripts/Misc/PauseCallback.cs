using System;
using UnityEngine;
using UnityEngine.Events;

namespace Misc
{
    //define public callbacks for pause and resume
    //subscribe via public UnityEvent members, call by invoking them or using wrapper methods
    public class PauseCallback : MonoBehaviour
    {
        public static PauseCallback pauseManager;

        public bool isPaused = false;
    
        private UnityEvent pauseCallback = new UnityEvent();

        private UnityEvent resumeCallback = new UnityEvent();

        private void Awake()
        {
            if (pauseManager != null)
            {
                Destroy(pauseManager.gameObject);
            }
            pauseManager = this;
            DontDestroyOnLoad(this.gameObject);
        }

        //sub to pause event
        public void SubscribeToPause(UnityAction func)
        {
            pauseCallback.AddListener(func);
        }

        //unsub to pause event
        public void UnsubToPause(UnityAction func)
        {
            pauseCallback.RemoveListener(func);
        }
        
        //sub to resume event
        public void SubscribeToResume(UnityAction func)
        {
            resumeCallback.AddListener(func);
        }

        //unsub to resume event
        public void UnsubToResume(UnityAction func)
        {
            resumeCallback.RemoveListener(func);
        }

        //wrapper around UnityEvent.Invoke
        public void Pause()
        {
            if (!isPaused)
            {
                pauseCallback.Invoke();
                AudioListener.pause = true;
                isPaused = true;
            }
        }

        //wrapper around UnityEvent.Invoke
        public void Resume()
        {
            if (isPaused)
            {
                resumeCallback.Invoke();
                AudioListener.pause = false;
                isPaused = false;
            }
        }

        private void OnApplicationPause(bool pauseStatus)
        {
            if (pauseStatus)
            {
                Pause();
            }
            else
            {
                Resume();
            }
        }
    }
}
