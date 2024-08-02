using System;
using System.IO;
using System.Text.Json;
using QuestSystem.Quests.QScripts;
using UnityEngine;
// ReSharper disable StringCompareIsCultureSpecific.1

namespace Match3.DataClasses
{
    [CreateAssetMenu]
    public class MeshDataObj : ScriptableObject, IComparable<MeshDataObj>
    {
        [Tooltip("Prefab of the Mesh")] public GameObject meshPrefab;

        [Tooltip("Name of the animal this bone is from")]
        public string animal;

        [Tooltip("Type of bone this is, ex. femur")]
        public string boneType;

        [Tooltip("Name for this bone mesh, ex. 'E. Pikachus Electroraticus Skull'. Must be unique")]
        public string boneName;

        [Tooltip("Description of the animal this bone is from")]
        public TextAsset animalDesc;

        [Tooltip("The 2D sprite representing the model (take snapshot in snapshot scene and convert)")]
        public Sprite flatImage;

        [Tooltip("If this bone is present in an endless mode quest, this must be set to the index of this bone in that quest's objective list.")]
        public int endlessIndex;

        private int numberMatched;

        public void Load()
        {
            try
            {
                numberMatched = JsonSerializer.Deserialize<int>(File.ReadAllText("Saves/" + boneName + ".json"));
            }
            catch (IOException)
            {
                numberMatched = 0;
            }
        }

        public int AddToMatchCount(int num=1)
        {
            numberMatched += num;
            EndlessModeQuestHandler.Progress(this);
            Serialize();
            return numberMatched;
        }

        private void Serialize()
        {
            string json = JsonSerializer.Serialize(numberMatched);
            Directory.CreateDirectory("Saves");
            File.WriteAllText("Saves/"+boneName+".json", json);
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
