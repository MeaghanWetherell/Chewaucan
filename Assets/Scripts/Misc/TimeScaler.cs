using UnityEngine;

namespace Misc
{
    public class TimeScaler : MonoBehaviour
    {
        public float time;
    
        // Start is called before the first frame update
        void Start()
        {
            Time.timeScale = time;
        }
    }
}
