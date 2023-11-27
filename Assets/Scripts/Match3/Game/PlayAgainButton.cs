using UnityEngine;

namespace Match3
{
    public class PlayAgainButton : MonoBehaviour
    {
        public int index;

        public void ONClick()
        {
            MatchLevelManager.matchLevelManager.LoadLevel(index);
        }
    }
}
