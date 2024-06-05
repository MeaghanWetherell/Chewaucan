using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using UnityEngine;

namespace Narration
{
    public class NarrationManager : MonoBehaviour
    {
        public static NarrationManager narrationManager;

        public bool reset;

        private Dictionary<string, bool> shouldRun;

        private List<string> hasRun;

        public bool ShouldPlay(Narration narration)
        {
            return ShouldPlay(narration.clipID);
        }

        public bool ShouldPlay(string id)
        {
            return shouldRun.ContainsKey(id) && shouldRun[id];
        }

        public void Played(string id)
        {
            hasRun.Add(id);
        }

        public void SetPlayability(Narration narration, bool set)
        {
            SetPlayability(narration.clipID, set);
        }

        public void SetPlayability(string id, bool set)
        {
            shouldRun[id] = set;
        }
        
        private void Awake()
        {
            if (narrationManager != null)
            {
                Debug.LogError("Loaded persistent objects twice!");
                Destroy(narrationManager.gameObject);
            }
            narrationManager = this;
            DontDestroyOnLoad(transform.gameObject);
            if(!reset)
                ReadFromJson();
            else
            {
                shouldRun = new Dictionary<string, bool>();
                hasRun = new List<string>();
            }
        }

        private void ReadFromJson()
        {
            try
            {
                shouldRun = JsonSerializer.Deserialize<Dictionary<string, bool>>(File.ReadAllText("Saves/shouldRun.json"));
            } catch(IOException){}
            shouldRun ??= new Dictionary<string, bool>();
            try
            {
                hasRun = JsonSerializer.Deserialize<List<string>>(File.ReadAllText("Saves/hasRun.json"));
            } catch(IOException){}
            hasRun ??= new List<string>();
        }

        private void OnDisable()
        {
            SerializeToJSON();
        }

        private void SerializeToJSON()
        {
            string shouldRunSave = JsonSerializer.Serialize(shouldRun);
            string hasRunSave = JsonSerializer.Serialize(hasRun);
            Directory.CreateDirectory("Saves");
            File.WriteAllText("Saves/shouldRun.json", shouldRunSave);
            File.WriteAllText("Saves/hasRun.json", hasRunSave);
        }
    }
}
