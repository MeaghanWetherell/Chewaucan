using UnityEngine;

namespace QuestSystem
{
    [CreateAssetMenu]
    public class QuestObj : ScriptableObject
    {
        [Tooltip("Quest name as it should appear in the quest log")]
        public string questName;
        
        [Tooltip("File containing a description of the quest. see example.txt for format")]
        public TextAsset descriptionFile;

        [Tooltip("The name of an objective as it should appear with a count in the quest log. see examples")]
        public string objective;

        [Tooltip("The number of times the objective must be completed to finish the quest (floats allowed, use for % progress)")]
        public float countRequired;

        [Tooltip("Default amount of progress to add every time the objective is performed (floats allowed, use for % progress)")]
        public float countAdded;
    }
}
