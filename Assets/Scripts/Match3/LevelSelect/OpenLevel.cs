using UnityEngine;

namespace Match3
{
    public class OpenLevel : MonoBehaviour
    {
        [System.NonSerialized]
        public int levelIndex;

        public void onClick()
        {
            MatchLevelManager.matchLevelManager.loadLevel(levelIndex);
        }
    }
}
