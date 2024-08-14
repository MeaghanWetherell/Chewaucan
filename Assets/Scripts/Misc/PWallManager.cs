using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Misc
{
    public class PWallManager : MonoBehaviour
    {
        public static PWallManager wallManager;
        
        [Tooltip("File name to save to (NOT A FULL PATH, no file extension)")]public String saveFileName;

        private List<String> validWallIds;
        
        private void Awake()
        {
            if (wallManager != null)
            {
                Debug.LogError("Loaded persistent object "+gameObject.name+" twice!");
                Destroy(wallManager.gameObject);
            }
            wallManager = this;
            DontDestroyOnLoad(this.gameObject);
            try
            {
                validWallIds = JsonSerializer.Deserialize<List<String>>(
                    File.ReadAllText("Saves/" + saveFileName + ".json"));
            } catch(IOException){}

            validWallIds ??= new List<string>();
        }
        
        private void OnDisable()
        {
            string bindJson = JsonSerializer.Serialize(validWallIds);
            Directory.CreateDirectory("Saves");
            File.WriteAllText("Saves/" + saveFileName + ".json", bindJson);
        }

        public bool checkID(string id)
        {
            return validWallIds.Contains(id);
        }

        public void AddValidID(string id)
        {
            validWallIds.Add(id);
        }
    }
}
