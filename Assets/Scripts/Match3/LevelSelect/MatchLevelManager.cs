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

        public List<LevelData> levels;

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
                levelsComplete = JsonSerializer.Deserialize<List<bool>>(File.ReadAllText("match3levelscomplete.json"));
            }
            catch (IOException e){ Debug.LogError(e.StackTrace);}
            if(levelsComplete == null)
                levelsComplete = new List<bool>();
            while(levelsComplete.Count < levels.Count)
                levelsComplete.Add(false);
        }

        private void OnDisable()
        {
            string completedJson = JsonSerializer.Serialize(levelsComplete);
            File.WriteAllText("match3levelscomplete.json", completedJson);
        }

        public void LoadLevel(int index)
        {
            SetValidMeshes(levels[index].meshes);
            if (levels[index].matchType)
            {
                MatchObject.compareByGroup = true;
            }
            else
            {
                MatchObject.compareByGroup = false;
            }
            _curIndex = index;
            MainSceneDataSaver.mainSceneDataSaver.PrepareForUnload();
            SceneManager.LoadScene(2);
            //UnityEngine.Random.InitState(160);
        }
        
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
