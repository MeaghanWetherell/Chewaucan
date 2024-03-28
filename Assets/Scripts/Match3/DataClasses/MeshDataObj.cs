using System;
using QuestSystem.Quests.QScripts;
using UnityEngine;
// ReSharper disable StringCompareIsCultureSpecific.1

namespace Match3.DataClasses
{
    [CreateAssetMenu]
    public class MeshDataObj : ScriptableObject, IComparable<MeshDataObj>
    {
        [Tooltip("Mesh for this model")] 
        public Mesh mesh;

        [Tooltip("Material that should be applied to this model")]
        public Material material;

        [Tooltip("Name of the animal this bone is from")]
        public string animal;

        [Tooltip("Type of bone this is, ex. femur")]
        public string boneType;

        [Tooltip("Name for this bone mesh, ex. 'E. Pikachus Electroraticus Skull'")]
        public string boneName;

        [Tooltip("Description of the animal this bone is from")]
        public TextAsset animalDesc;

        [Tooltip("The 2D sprite representing the model (take snapshot in snapshot scene and convert)")]
        public Sprite flatImage;

        private int numberMatched;

        public int AddToMatchCount(int num=1)
        {
            numberMatched += num;
            EndlessModeQuestHandler.Progress(this);
            return numberMatched;
        }

        public int GetMatchCount()
        {
            return numberMatched;
        }

        public int CompareTo(MeshDataObj other)
        {
            if (!animal.Equals(other.animal))
                return String.Compare(animal, other.animal);
            return String.Compare(boneName, other.boneName);
        }
    }
}
