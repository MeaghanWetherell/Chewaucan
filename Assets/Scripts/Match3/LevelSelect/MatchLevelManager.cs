using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using Misc;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Match3
{
    public class MatchLevelManager : MonoBehaviour
    {
        public static MatchLevelManager matchLevelManager;

        [Tooltip("All match 3 levels")]public List<LevelData> levels;

        //stores the index of the level data for the level currently being played
        private int _curIndex;

        [System.NonSerialized]
        public List<bool> levelsComplete;
        private void Awake()
        {
            if (matchLevelManager != null)
            {
                Debug.LogError("Loaded persistent objects twice!");
                Destroy(matchLevelManager.gameObject);
            }
            matchLevelManager = this;
            DontDestroyOnLoad(this.gameObject);
            try
            {
                levelsComplete = JsonSerializer.Deserialize<List<bool>>(File.ReadAllText("Saves/match3levelscomplete.json"));
            }
#pragma warning disable 168
            catch (IOException e){ levelsComplete = new List<bool>();}
#pragma warning restore 168
            // ReSharper disable once PossibleNullReferenceException
            while(levelsComplete.Count < levels.Count)
                levelsComplete.Add(false);
        }

        private void OnDisable()
        {
            string completedJson = JsonSerializer.Serialize(levelsComplete);
            Directory.CreateDirectory("Saves");
            File.WriteAllText("Saves/match3levelscomplete.json", completedJson);
        }

        //load the match 3 level at the passed index
        public void LoadLevel(int index)
        {
            SetValidMeshes(levels[index].meshes);
            MatchObject.compareByGroup = levels[index].matchType;
            MatchLine.shouldRotate = levels[index].rotate;
            _curIndex = index;
            MainSceneDataSaver.mainSceneDataSaver.PrepareForUnload();
            SceneManager.LoadScene(2);
            //UnityEngine.Random.InitState(170);
        }
        
        //sets up the match object valid mesh list
        public void SetValidMeshes(int[] valid)
        {
            MatchObject.validMeshes = new List<int>();
            foreach (int index in valid)
            {
                MatchObject.validMeshes.Add(index);
            }
        }

        public LevelData GETCurLevel()
        {
            return levels[_curIndex];
        }
    }
}