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
                Destroy(gameObject);
                return;
            }
            validWallIds ??= new List<string>();
            wallManager = this;
            DontDestroyOnLoad(this.gameObject);
            SaveHandler.saveHandler.subToSave(Save);
            SaveHandler.saveHandler.subToLoad(Load);
        }

        private void Load(string path)
        {
            try
            {
                validWallIds = JsonSerializer.Deserialize<List<String>>(
                    File.ReadAllText(path+"/" + saveFileName + ".json"));
            } catch(IOException){}
            validWallIds ??= new List<string>();
        }
        
        private void Save(string path)
        {
            string bindJson = JsonSerializer.Serialize(validWallIds);
            File.WriteAllText(path+"/" + saveFileName + ".json", bindJson);
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
