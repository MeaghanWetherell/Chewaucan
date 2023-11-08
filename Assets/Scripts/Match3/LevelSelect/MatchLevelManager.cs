using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Json;
using System.Text.Json;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Match3
{
    public class MatchLevelManager : MonoBehaviour
    {
        public static MatchLevelManager matchLevelManager;

        public List<LevelData> levels;

        private int curIndex;

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
                StreamReader streamReader = new StreamReader("match3levelscomplete.json");
                string line = streamReader.ReadLine();
                levelsComplete = JsonUtility.FromJson<List<bool>>(line);
                streamReader.Close();
            }
            catch (Exception){ }
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

        public void loadLevel(int index)
        {
            setValidMeshes(levels[index].meshes);
            curIndex = index;
            SceneManager.LoadScene(2);
            //UnityEngine.Random.InitState(160);
        }
        
        public void setValidMeshes(int[] valid)
        {
            MatchObject.validMeshes = new List<int>();
            foreach (int index in valid)
            {
                MatchObject.validMeshes.Add(index);
            }
        }

        public LevelData getCurLevel()
        {
            return levels[curIndex];
        }
    }
}
