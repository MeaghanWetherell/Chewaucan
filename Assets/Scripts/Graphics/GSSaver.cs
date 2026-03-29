using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;
using UnityEngine;

public static class GSSaver
{
    //path at which to save graphics settings
    public static String filepath = "Chewaucan/SavedSettings/GraphicsSettings";

    //list of the non-standard resolutions that have been registered
    private static List<Vector2Int> addedResolutions;

    //whether the saver has been initialized
    private static bool initted;

    public static List<Vector2Int> GetAddedResolutions()
    {
        Init();
        return addedResolutions;
    }

    public static void AddResolution(Vector2Int add)
    {
        Init();
        addedResolutions.Add(add);
    }

    //read in added resolutions from disk, sub to save 
    private static void Init()
    {
        if (initted) return;
        try
        {
            var opts = new JsonSerializerOptions
            {
                IncludeFields = true,
                IgnoreReadOnlyProperties = true,
                NumberHandling = JsonNumberHandling.AllowNamedFloatingPointLiterals
            };
            addedResolutions =
                JsonSerializer.Deserialize<List<Vector2Int>>(File.ReadAllText(Application.persistentDataPath + filepath +
                                                                              "AddedResolutions.json"), opts);
        }
        catch (IOException)
        {
            addedResolutions = new List<Vector2Int>();
        }
        SaveHandler.saveHandler.subSettingToSave(Save);
        initted = true;
    }

    //save to disk
    private static void Save(string path)
    {
        Directory.CreateDirectory(Application.persistentDataPath + filepath);
        var opts = new JsonSerializerOptions
        {
            IncludeFields = true,
            IgnoreReadOnlyProperties = true,
            NumberHandling = JsonNumberHandling.AllowNamedFloatingPointLiterals
        };
        string json = JsonSerializer.Serialize(addedResolutions, opts);
        File.WriteAllText(Application.persistentDataPath+"/"+filepath+"AddedResolutions.json", json);
    }
}
