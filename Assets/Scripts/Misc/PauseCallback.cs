using UnityEngine;
using UnityEngine.Events;

namespace Misc
{
    //define public callbacks for pause and resume
    //subscribe via public UnityEvent members, call by invoking them or using wrapper methods
    public class PauseCallback : MonoBehaviour
    {
        public static PauseCallback pauseManager;
    
        [System.NonSerialized]public UnityEvent pauseCallback = new UnityEvent();

        [System.NonSerialized]public UnityEvent resumeCallback = new UnityEvent();

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
