using UnityEngine;

namespace Match3
{
    //Loads a passed level
    public class PlayAgainButton : MonoBehaviour
    {
        public int index;

        public void ONClick()
        {
            MatchLevelManager.matchLevelManager.LoadLevel(index);
        }
    }
}
