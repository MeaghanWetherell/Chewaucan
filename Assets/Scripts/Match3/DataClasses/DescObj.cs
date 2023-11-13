using UnityEngine;

namespace Match3
{
    [CreateAssetMenu]
    public class DescObj : ScriptableObject
    {
        [Tooltip("Description of an animal, to be plugged into a field on the mesh datas of multiple bones")]
        public string description;
    }
}
