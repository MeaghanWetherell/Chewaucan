using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using UnityEngine;

namespace Narration
{
    //manages which narration clips are able to be run based on the player's progress
    public class NarrationManager : MonoBehaviour
    {
        //singleton
        public static NarrationManager narrationManager;

        [Tooltip("Reset flag. Check to reset which narrations can play")]public bool reset;

        //maps the name of a Narration scriptable object with whether it is allowed to run
        private Dictionary<string, bool> shouldRun;

        //stores narration clips that have already played
        public List<string> hasRun;

        //whether the Narration with the passed name is valid to play
        public bool ShouldPlay(string id)
        {
            return shouldRun.ContainsKey(id) && shouldRun[id];
        }

        //whether the Narration with the passed name has been played
        public void Played(string id)
        {
            if(!hasRun.Contains(id))
                hasRun.Add(id);
        }

        //set whether Narration with the passed name should play
        public void SetPlayability(string id, bool set)
        {
            shouldRun[id] = set;
        }

        //initialize singleton and load save
        private void Awake()
        {
            if(!reset)
                ReadFromJson();
            else
            {
                shouldRun = new Dictionary<string, bool>();
                hasRun = new List<string>();
            }
            if (narrationManager != null)
            {
                Destroy(narrationManager.gameObject);
            }
            narrationManager = this;
            DontDestroyOnLoad(transform.gameObject);
        }

        //read in save from disk
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

        //serialize on exit
        private void OnDisable()
        {
            SerializeToJSON();
        }

        //save the narrations that should and have run
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
