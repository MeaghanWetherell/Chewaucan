using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using Misc;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

public class SaveHandler : MonoBehaviour
{
    public static SaveHandler saveHandler;
    
    //future proofing for in case save files need version conversion
    [Tooltip("Version number for the current version")]
    public string currentVersion;

    [Tooltip("Save location for metadata such as the last used save path")]
    public string metadataSaveLoc;

    [Tooltip("Path at which settings are saved")]
    public string settingsSavePath;

    //the names of each save slot
    [NonSerialized]public List<String> saveSlots;

    //the path to the current active save folder
    private string savePath;

    //time played on the current save
    private float timePlayed;

    //invoked when loading a new save
    private UnityEvent<string> loadRegular = new UnityEvent<string>();

    //invoked when we should save
    private UnityEvent<string> saveRegular = new UnityEvent<string>();
    
    private UnityEvent<string> loadSettings = new UnityEvent<string>();

    private UnityEvent<string> saveSettings = new UnityEvent<string>();

    //invoked on creating a new game
    private UnityEvent<string> newGame = new UnityEvent<string>();

    //flag used when not loading from the main menu to force new subscribers to load to load from file immediately
    private bool loadImmediately;
    
    private Coroutine timeTracker;

    //initialize the save slots and metadata
    private void Awake()
    {
        if (saveHandler != null)
        {
            Destroy(gameObject);
            return;
        }
        saveHandler = this;
        DontDestroyOnLoad(gameObject);
        Directory.CreateDirectory(Application.persistentDataPath+"/"+metadataSaveLoc);
        Directory.CreateDirectory(Application.persistentDataPath+"/"+settingsSavePath);

        if (File.Exists(Application.persistentDataPath + "/" +
                        metadataSaveLoc + "/saveSlots.json"))
        {
            saveSlots = JsonSerializer.Deserialize<List<String>>(File.ReadAllText(Application.persistentDataPath + "/" +
                metadataSaveLoc + "/saveSlots.json"));
        }
        if(saveSlots == null)
        {
            saveSlots = new List<string>();
            saveSlots.Add("Chewaucan/Save 1");
            saveSlots.Add("Chewaucan/Save 2");
            saveSlots.Add("Chewaucan/Save 3");
        }
        if (!SceneManager.GetActiveScene().name.Equals("MainMenu"))
        {
            string lastPath = getLastSavePath();
            if (!checkPath(lastPath))
            {
                Debug.Log("Last Path Not Found!");
            }
            setSavePath(lastPath);
            loadImmediately = true;
        }
    }

    //gets the path to the most recently used save
    public string getLastSavePath()
    {
        try
        {
            StreamReader streamReader =
                new StreamReader(Application.persistentDataPath + "/" + metadataSaveLoc + "/lastPath");
            string line = streamReader.ReadLine();
            line = line.Trim();
            streamReader.Close();
            return line;
        }
        catch (FileNotFoundException)
        {
            return "";
        }
    }

    private void Start()
    {
        if(!loadImmediately)
            loadSettings.Invoke(Application.persistentDataPath+"/"+settingsSavePath);
    }

    //auto-saves on quit
    private void OnApplicationQuit()
    {
        try
        {
            string saveSlotsJson = JsonSerializer.Serialize(saveSlots);
            File.WriteAllText(Application.persistentDataPath + "/" +
                              metadataSaveLoc + "/saveSlots.json", saveSlotsJson);
            Save();
        }
        catch(Exception e)
        {
            Debug.LogError("Failed to save data. Data may be lost and one or more files may have become corrupted");
            Debug.LogError(e.Message);
            Debug.LogError(e.StackTrace);
        }
    }

    //returns whether the current path is valid
    public bool checkPath()
    {
        return checkPath(savePath);
    }
    
    //returns whether the passed path is valid
    public bool checkPath(string path)
    {
        if (path == null || path.Equals("")) return false;
        (string, float) meta = readMetaFile(path);
        return !meta.Item1.Equals("err");
    }

    public void subToLoad(UnityAction<string> load)
    {
        if(loadImmediately) load.Invoke(Application.persistentDataPath+"/"+savePath);
        loadRegular.AddListener(load);
    }

    public void unsubToLoad(UnityAction<string> load)
    {
        loadRegular.RemoveListener(load);
    }

    public void unsubToSave(UnityAction<string> save)
    {
        saveRegular.RemoveListener(save);
    }

    public void subToSave(UnityAction<string> save)
    {
        saveRegular.AddListener(save);
    }
    
    public void subSettingToLoad(UnityAction<string> load)
    {
        if(loadImmediately) load.Invoke(Application.persistentDataPath+"/"+settingsSavePath);
        loadSettings.AddListener(load);
    }

    public void subSettingToSave(UnityAction<string> save)
    {
        saveSettings.AddListener(save);
    }

    public void subToNewGame(UnityAction<string> ng)
    {
        newGame.AddListener(ng);
    }

    public void unsubToNewGame(UnityAction<string> ng)
    {
        newGame.RemoveListener(ng);
    }

    //sets the current save path
    public void setSavePath(string path)
    {
        if (checkPath())
        {
            Save();
        }
        savePath = path;
    }

    private void writeMetaFile(string path, string version, float time)
    {
        File.WriteAllText(Application.persistentDataPath+"/"+path+"/meta", version+"\n"+time);
    }

    //reads the meta data for a save slot: version number and time played for the slot
    private (string, float) readMetaFile(string path)
    {
        (string, float) ret = new ValueTuple<string, float>();
        try
        {
            StreamReader streamReader = new StreamReader(Application.persistentDataPath + "/" + path + "/meta");
            string line = streamReader.ReadLine();
            ret.Item1 = line.Trim();
            line = streamReader.ReadLine();
            line = line.Trim();
            ret.Item2 = float.Parse(line);
            streamReader.Close();
            return ret;
        }
        catch (FileNotFoundException)
        {
            ret.Item1 = "err";
            return ret;
        }
        catch (DirectoryNotFoundException)
        {
            ret.Item1 = "err";
            return ret;
        }
    }

    public void StartTimer()
    {
        if (timeTracker == null)
            timeTracker = StartCoroutine(timer());
    }

    public void StopTimer()
    {
        if (timeTracker != null)
        {
            StopCoroutine(timeTracker);
            timeTracker = null;
        }
    }

    private IEnumerator timer()
    {
        while (true)
        {
            if (!PauseCallback.pauseManager.isPaused)
            {
                timePlayed += 0.2f;
            }
            yield return new WaitForSeconds(0.2f);
        }
    }
    
    public string getSavePath()
    {
        return Application.persistentDataPath+"/"+savePath;
    }

    //creates a new game at the passed path, deleting the old save if it exists
    public void NewGame(string path)
    {
        try
        {
            Directory.Delete(Application.persistentDataPath+"/"+path, true);
        }
        catch(DirectoryNotFoundException) {}
        setSavePath(path);
        timePlayed = 0;
        StartTimer();
        newGame.Invoke(path);
    }

    //currently does nothing. future proofing for in case save files need version conversion
    public void VersionConversion(string fileVersion)
    {
        Debug.Log("Version mismatch");
    }

    //invoke load for the currently set save file
    public void Load()
    {
        (string, float) tup = readMetaFile(savePath);
        if (tup.Item1.Equals("err"))
        {
            Directory.CreateDirectory(Application.persistentDataPath+"/"+savePath);
            writeMetaFile(savePath, currentVersion, 0.0f);
        }
        else
        {
            if(!tup.Item1.Equals(currentVersion))
                VersionConversion(tup.Item1);
            timePlayed = tup.Item2;
        }
        StartTimer();
        loadRegular.Invoke(Application.persistentDataPath+"/"+savePath);
        loadSettings.Invoke(Application.persistentDataPath+"/"+settingsSavePath);
    }

    //save everything to the current save file
    public void Save()
    {
        StopTimer();
        if (savePath != null && !savePath.Equals(""))
        {
            writeMetaFile(savePath, currentVersion, timePlayed);
            saveRegular.Invoke(Application.persistentDataPath+"/"+savePath);
            File.WriteAllText(Application.persistentDataPath+"/"+metadataSaveLoc + "/lastPath", savePath);
        }
        saveSettings.Invoke(Application.persistentDataPath+"/"+settingsSavePath);
    }
}
