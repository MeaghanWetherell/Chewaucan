using UnityEngine;

namespace Match3
{
    [CreateAssetMenu]
    public class LevelData : ScriptableObject
    {
        [Tooltip("Number in order, ex. 1 for level 1")]
        public int levelNum;

        [Tooltip("Indexes of meshes from the mesh list to use; see mesh data")]
        public int[] meshes;

        [Tooltip("Score required to pass the level")]
        public float scoreReq;

        [Tooltip("Time to pass the level")] 
        public float time;

        [Tooltip("Whether meshes should be rotated")]
        public bool rotate;

        [Tooltip("The type of matching that should be performed in this level")]
        public MatchObject.MatchType matchType;
    }
}
