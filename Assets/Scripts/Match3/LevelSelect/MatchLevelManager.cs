using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using Match3.DataClasses;
using Misc;
using UnityEngine;
using UnityEngine.Events;

namespace Match3
{
    public class MatchLevelManager : MonoBehaviour
    {
        public static MatchLevelManager matchLevelManager;

        [Tooltip("Match levels json file name")] public String levelsCompleteFileName;

        [Tooltip("Match High Scores json file name")] public String highScoreFileName;

        [Tooltip("All match 3 levels")]public List<LevelData> levels;

        //stores the index of the level data for the level currently being played
        public int curIndex;
        
        private List<float> highScores;

        [System.NonSerialized]
        public List<bool> levelsComplete;
        
        public readonly UnityEvent<int> OnComplete = new UnityEvent<int>();

        private void Awake()
        {
            if (matchLevelManager != null)
            {
                Debug.LogError("Loaded persistent objects twice!");
                Destroy(matchLevelManager.gameObject);
            }
            matchLevelManager = this;
            DontDestroyOnLoad(this.gameObject);
            //MeshDataList allMeshes = Resources.Load<MeshDataList>("meshes/Match3Meshes");
            //foreach (MeshDataObj obj in allMeshes.meshes)
            //{
                //obj.Load();
            //}
            try
            {
                levelsComplete = JsonSerializer.Deserialize<List<bool>>(File.ReadAllText("Saves/"+levelsCompleteFileName+".json"));
            }
            catch (IOException){ }
            levelsComplete ??= new List<bool>();
            // ReSharper disable once PossibleNullReferenceException
            while(levelsComplete.Count < levels.Count)
                levelsComplete.Add(false);
            try
            {
                highScores = JsonSerializer.Deserialize<List<float>>(File.ReadAllText("Saves/"+highScoreFileName+".json"));
            }
            catch (IOException){ highScores = new List<float>();}
            // ReSharper disable once PossibleNullReferenceException
            while(highScores.Count < levels.Count)
                highScores.Add(0);
        }

        private void OnEnable()
        {
            SceneLoadWrapper.sceneLoadWrapper.OnLoadScene.AddListener(SetEndlessHighScore);
        }

        private void OnDisable()
        {
            SceneLoadWrapper.sceneLoadWrapper.OnLoadScene.RemoveListener(SetEndlessHighScore);
            string completedJson = JsonSerializer.Serialize(levelsComplete);
            Directory.CreateDirectory("Saves");
            File.WriteAllText("Saves/"+levelsCompleteFileName+".json", completedJson);
            string scoresJson = JsonSerializer.Serialize(highScores);
            File.WriteAllText("Saves/"+highScoreFileName+".json", scoresJson);
        }

        //ends the match 3 game and displays results, including the passed reason for game end
        public void EndGame(string reason)
        {
            Timer.timer.StopAllCoroutines();
            Timer.timer.enabled = false;
            MatchGrid.matchGrid.gameObject.SetActive(false);
            if (ScoreTracker.scoreTracker.score < ScoreTracker.scoreTracker.scoreRequired)
            {
                MatchUIManager.matchUIManager.Lose(reason);
            }
            else
            {
                OnComplete.Invoke(curIndex);
                MatchUIManager.matchUIManager.Win(reason);
                if (curIndex < highScores.Count-1)
                {
                    if(ScoreTracker.scoreTracker.score > highScores[curIndex])
                        highScores[curIndex] = ScoreTracker.scoreTracker.score;
                }
                else
                {
                    highScores[curIndex] += ScoreTracker.scoreTracker.score;
                }
            }
        }
        
        //get the high score for the current level
        public float GetHighscore()
        {
            return highScores[curIndex];
        }

        public List<float> GetAllHighscores()
        {
            return highScores;
        }

        //load the match 3 level at the passed index
        public void LoadLevel(int index)
        {
            SetValidMeshes(levels[index].meshes);
            MatchObject.ChangeMatchType(levels[index].matchType);
            MatchLine.shouldRotate = levels[index].rotate;
            curIndex = index;
            SceneLoadWrapper.sceneLoadWrapper.LoadScene("Match3");
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
            return levels[curIndex];
        }

        public void SetEndlessHighScore()
        {
            if (ScoreTracker.scoreTracker != null && curIndex == levels.Count - 1)
            {
                highScores[curIndex] += ScoreTracker.scoreTracker.score;
            }
        }
    }
}
