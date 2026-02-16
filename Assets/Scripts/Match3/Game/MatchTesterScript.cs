using UnityEngine;

namespace Match3.Game
{
    public class MatchTesterScript : MonoBehaviour
    {
        [Tooltip("Level to test")] public int level;

        private void Awake()
        {
            MatchLevelManager.matchLevelManager.LoadLevelTesting(level-1);
        }
    }
}
