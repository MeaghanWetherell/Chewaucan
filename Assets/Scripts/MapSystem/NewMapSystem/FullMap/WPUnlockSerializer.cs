using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WPUnlockSerializer : MonoBehaviour
{
    public static WPUnlockSerializer wpUnlockSerializer;

    public bool unlockAllModern = false;
    
    private Dictionary<String, bool> wpUnlocks = new Dictionary<string, bool>();

    [Tooltip("Save file name (not a full path)")]
    public string fileName;

    //whether to open the map on pleistocene or modern
    private bool PL;

    public void Unlock(String name)
    {
        wpUnlocks[name.ToLower()] = true;
    }

    private void Awake()
    {
        if (wpUnlockSerializer != null)
        {
            Destroy(gameObject);
            return;
        }
        SceneManager.sceneLoaded += OnMapLoad;
        wpUnlockSerializer = this;
        DontDestroyOnLoad(this.gameObject);
        SaveHandler.saveHandler.subToSave(Serialize);
        SaveHandler.saveHandler.subToLoad(DeSerialize);
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnMapLoad;
    }

    private void Serialize(string path)
    {
        string completedJson = JsonSerializer.Serialize(wpUnlocks);
        File.WriteAllText(path+"/"+fileName+".json", completedJson);
        string modernUnlocked = JsonSerializer.Serialize(unlockAllModern);
        File.WriteAllText(path+"/"+fileName+"ModernUnlock.json", modernUnlocked);
    }

    private void OnMapLoad(Scene scene, LoadSceneMode mode)
    {
        if (scene.name.Equals("Modern Map"))
            PL = false;
        else if (scene.name.Equals("PleistoceneMap"))
            PL = true;
        if (!scene.name.Equals("FullMapView"))
            return;
        GameObject[] tpObjs = GameObject.FindGameObjectsWithTag("WP");
        foreach (GameObject obj in tpObjs)
        {
            TeleportWaypoint wp = obj.GetComponent<TeleportWaypoint>();
            if (wp != null)
            {
                if (wp.mapType == TeleportWaypoint.MapType.modern)
                {
                    wp.unlocked = unlockAllModern;
                }
                else if(wpUnlocks.ContainsKey(wp.wpName.ToLower()))
                    wp.unlocked = wpUnlocks[wp.wpName.ToLower()];
                else
                {
                    wpUnlocks.Add(wp.wpName.ToLower(), wp.unlocked);
                }
                if (wp.unlocked)
                {
                    wp.GetComponent<SpriteRenderer>().color = Color.white;
                }
                else
                {
                    wp.GetComponent<SpriteRenderer>().color = Color.gray;
                }
            }
        }
        if(PL)
            GameObject.Find("ModernMapView").SetActive(false);
        else
            GameObject.Find("PleistoceneMapView").SetActive(false);
    }

    private void DeSerialize(string path)
    {
        try
        {
            wpUnlocks =
                JsonSerializer.Deserialize<Dictionary<String, bool>>(File.ReadAllText(path + "/" + fileName + ".json"));
            if (wpUnlocks == null) wpUnlocks = new Dictionary<string, bool>();
            
        }
        catch (IOException)
        {
            wpUnlocks = new Dictionary<string, bool>();
            
        }
        try
        {
            unlockAllModern =
                JsonSerializer.Deserialize<bool>(File.ReadAllText(path + "/" + fileName + "ModernUnlock.json"));
        }
        catch (IOException)
        {
            unlockAllModern = false;
        }

    }
}
