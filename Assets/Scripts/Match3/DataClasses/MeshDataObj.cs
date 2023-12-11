using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace Match3
{
    [CreateAssetMenu]
    public class MeshDataObj : ScriptableObject, IComparable<MeshDataObj>
    {
        [Tooltip("Mesh for this model")]
        public Mesh mesh;

        [Tooltip("Material that should be applied to this model")]
        public Material material;

        [FormerlySerializedAs("type")] [Tooltip("Group of this model for matching by type, ex. Pikachu if it's a Pikachu bone")]
        public string @group;

        [Tooltip("Name for this bone mesh, ex. 'E. Pikachus Electroraticus Skull'")]
        public string boneName;

        [Tooltip("Description of the animal this bone is from")] 
        public DescObj animalDesc;

        [Tooltip("The 2D sprite representing the model (take snapshot in snapshot scene and convert)")]
        public Sprite flatImage;

        public int CompareTo(MeshDataObj other)
        {
            return String.Compare(boneName, other.boneName);
        }
    }
}
