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
    
    private Dictionary<String, bool> wpUnlocks = new Dictionary<string, bool>();

    [Tooltip("Save file name (not a full path)")]
    public string fileName;

    //whether to open the map on pleistocene or modern
    private bool PL;

    public void Unlock(String name)
    {
        if (wpUnlocks.ContainsKey(name))
            wpUnlocks[name] = true;
        else
        {
            wpUnlocks.Add(name, true);
        }
    }

    private void Awake()
    {
        SceneManager.sceneLoaded += OnMapLoad;
        DeSerialize();
        if (wpUnlockSerializer != null)
        {
            Destroy(wpUnlockSerializer.gameObject);
        }
        wpUnlockSerializer = this;
        DontDestroyOnLoad(this.gameObject);
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnMapLoad;
        Serialize();
    }

    private void Serialize()
    {
        string completedJson = JsonSerializer.Serialize(wpUnlocks);
        Directory.CreateDirectory("Saves");
        File.WriteAllText("Saves/"+fileName+".json", completedJson);
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
                if(wpUnlocks.ContainsKey(wp.wpName))
                    wp.unlocked = wpUnlocks[wp.wpName];
                else
                {
                    wpUnlocks.Add(wp.wpName, wp.unlocked);
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

    private void DeSerialize()
    {
        
        try
        {
            wpUnlocks = 
                JsonSerializer.Deserialize<Dictionary<String, bool>>(File.ReadAllText("Saves/" + fileName + ".json"));
            if (wpUnlocks == null) wpUnlocks = new Dictionary<string, bool>();
            
        }
        catch (IOException)
        {
            return;
        }
    }
}
