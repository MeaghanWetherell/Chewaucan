using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using UnityEngine;
using UnityEngine.Serialization;

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

        [Tooltip("Scriptable object holding all narration that should have their on complete lists reset on load")] public FullNarrList resetNarrList;

        //return whether the manager has registered if the passed narration should be playable or not
        public bool HasDataOnNarr(string id)
        {
            return shouldRun.ContainsKey(id);
        }

        //whether the Narration with the passed name is valid to play
        public bool ShouldPlay(string id)
        {
            return shouldRun.ContainsKey(id) && shouldRun[id];
        }

        //registers the passed narration as played
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
            if (narrationManager != null)
            {
                Destroy(gameObject);
                return;
            }
            shouldRun = new Dictionary<string, bool>();
            hasRun = new List<string>();
            narrationManager = this;
            DontDestroyOnLoad(transform.gameObject);
            SaveHandler.saveHandler.subToLoad(ReadFromJson);
            SaveHandler.saveHandler.subToSave(SerializeToJson);
        }

        //read in save from disk
        private void ReadFromJson(string path)
        {
            if (reset) return;
            resetNarrList.Clear();
            try
            {
                shouldRun = JsonSerializer.Deserialize<Dictionary<string, bool>>(File.ReadAllText(path+"/shouldRun.json"));
            } catch(IOException){shouldRun = new Dictionary<string, bool>();}
            
            try
            {
                hasRun = JsonSerializer.Deserialize<List<string>>(File.ReadAllText(path+"/hasRun.json"));
            } catch(IOException){hasRun = new List<string>();}
            
        }

        //save the narrations that should and have run
        private void SerializeToJson(string path)
        {
            string shouldRunSave = JsonSerializer.Serialize(shouldRun);
            string hasRunSave = JsonSerializer.Serialize(hasRun);
            File.WriteAllText(path+"/shouldRun.json", shouldRunSave);
            File.WriteAllText(path+"/hasRun.json", hasRunSave);
        }
    }
}
