using UnityEngine;

namespace Match3
{
    public class PlayAgainButton : MonoBehaviour
    {
        public int index;

        public void onClick()
        {
            MatchLevelManager.matchLevelManager.loadLevel(index);
        }
    }
}
