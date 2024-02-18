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
    
        private UnityEvent pauseCallback = new UnityEvent();

        private UnityEvent resumeCallback = new UnityEvent();

        private void Awake()
        {
            if (pauseManager != null)
            {
                Debug.LogError("Loaded persistent objects twice!");
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
            pauseCallback.Invoke();
        }

        //wrapper around UnityEvent.Invoke
        public void Resume()
        {
            resumeCallback.Invoke();
        }
    }
}
