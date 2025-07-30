using System;
using System.Collections;
using System.IO;
using Misc;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class SaveHandler : MonoBehaviour
{
    public static SaveHandler saveHandler;
    
    public string currentVersion;

    public string lastPathSaveLoc;

    public string settingsSavePath;

    private string savePath;

    private float timePlayed;

    private UnityEvent<string> loadRegular = new UnityEvent<string>();

    private UnityEvent<string> saveRegular = new UnityEvent<string>();
    
    private UnityEvent<string> loadSettings = new UnityEvent<string>();

    private UnityEvent<string> saveSettings = new UnityEvent<string>();

    private bool loadImmediately;

    private Coroutine timeTracker;

    private void Awake()
    {
        if (saveHandler != null)
        {
            Destroy(gameObject);
            return;
        }
        saveHandler = this;
        DontDestroyOnLoad(gameObject);
        Directory.CreateDirectory(Application.persistentDataPath+"/"+lastPathSaveLoc);
        Directory.CreateDirectory(Application.persistentDataPath+"/"+settingsSavePath);
        try
        {
            StreamReader streamReader = new StreamReader(Application.persistentDataPath+"/"+lastPathSaveLoc + "/lastPath");
            string line = streamReader.ReadLine();
            line = line.Trim();
            streamReader.Close();
            setSavePath(line);
        }
        catch (FileNotFoundException)
        { }
        if (!SceneManager.GetActiveScene().name.Equals("MainMenu") && savePath != null && !savePath.Equals(""))
        {
            loadImmediately = true;
        }
    }

    private void Start()
    {
        loadSettings.Invoke(Application.persistentDataPath+"/"+settingsSavePath);
        if (!SceneManager.GetActiveScene().name.Equals("MainMenu") && savePath != null && !savePath.Equals(""))
        {
            Load();
        }
    }

    private void OnApplicationQuit()
    {
        try
        {
            Save();
        }
        catch(Exception e)
        {
            Debug.LogError("Failed to save data. Data may be lost and one or more files may have become corrupted");
            Debug.LogError(e.Message);
            Debug.LogError(e.StackTrace);
        }
    }

    public bool checkPath()
    {
        return checkPath(savePath);
    }
    
    public bool checkPath(string path)
    {
        if (path == null || path.Equals("")) return false;
        (string, float) meta = readMetaFile(path);
        return !meta.Item1.Equals("err");
    }

    public void subToLoad(UnityAction<string> load)
    {
        //if(loadImmediately) load.Invoke(Application.persistentDataPath+"/"+savePath);
        loadRegular.AddListener(load);
    }

    public void subToSave(UnityAction<string> save)
    {
        saveRegular.AddListener(save);
    }
    
    public void subSettingToLoad(UnityAction<string> load)
    {
        //if(loadImmediately) load.Invoke(Application.persistentDataPath+"/"+settingsSavePath);
        loadSettings.AddListener(load);
    }

    public void subSettingToSave(UnityAction<string> save)
    {
        saveSettings.AddListener(save);
    }

    public void setSavePath(string path)
    {
        if (checkPath())
        {
            Save();
        }
        savePath = path;
        (string, float) tup = readMetaFile(path);
        if (tup.Item1.Equals("err"))
        {
            Directory.CreateDirectory(Application.persistentDataPath+"/"+path);
            writeMetaFile(path, currentVersion, 0.0f);
        }
        else
        {
            if(!tup.Item1.Equals(currentVersion))
                VersionConversion(tup.Item1);
            timePlayed = tup.Item2;
        }
        File.WriteAllText(Application.persistentDataPath+"/"+lastPathSaveLoc+"/lastPath", path);
    }

    private void writeMetaFile(string path, string version, float time)
    {
        File.WriteAllText(Application.persistentDataPath+"/"+path+"/meta", version+"\n"+time);
    }

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
            File.WriteAllText(Application.persistentDataPath + "/" + lastPathSaveLoc + "/lastPath", path);
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
        return savePath;
    }

    public void NewGame(string path)
    {
        try
        {
            Directory.Delete(Application.persistentDataPath+"/"+path, true);
        }
        catch(DirectoryNotFoundException) {}
        setSavePath(path);
    }

    public void VersionConversion(string fileVersion)
    {
        Debug.Log("Version mismatch");
    }

    public void Load()
    {
        StartTimer();
        loadRegular.Invoke(Application.persistentDataPath+"/"+savePath);
        loadSettings.Invoke(Application.persistentDataPath+"/"+settingsSavePath);
    }

    public void Save()
    {
        StopTimer();
        writeMetaFile(savePath, currentVersion, timePlayed);
        if (savePath != null && !savePath.Equals(""))
        {
            saveRegular.Invoke(Application.persistentDataPath+"/"+savePath);
            File.WriteAllText(Application.persistentDataPath+"/"+lastPathSaveLoc + "/lastPath", savePath);
        }
        saveSettings.Invoke(Application.persistentDataPath+"/"+settingsSavePath);
    }
}
