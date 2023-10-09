using System.Collections.Generic;
using UnityEngine;

namespace QuestSystem
{
    //stores quest data prior to quest initialization
    [CreateAssetMenu]
    public class QuestObj : ScriptableObject
    {
        [Tooltip("Quest name as it should appear in the quest log")]
        public string questName;

        [Tooltip("It doesn't matter what this is, as long it's not the same for any quest")] 
        public string uniqueID;
        
        [Tooltip("File containing a description of the quest. see example.txt for format")]
        public TextAsset descriptionFile;

        [Tooltip("The name of the objectives as it should appear with a count in the quest log. see examples")]
        public List<string> objectives;

        [Tooltip("The number of times the objective must be completed to finish the quest, each entry must correspond by index to objectives")]
        public List<float> countsRequired;

        [Tooltip("OPTIONAL Default amount of progress to add every time the objective is performed, each entry must correspond by index to objectives")]
        public List<float> countsAdded;
    }
}
