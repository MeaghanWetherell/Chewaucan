using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;
using UnityEngine;

public static class GSSaver
{
    public static String filepath = "Chewaucan/GraphicsSettings";

    private static List<Vector2Int> addedResolutions;

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
        SaveHandler.saveHandler.subToSave(Save);
        initted = true;
    }

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
        File.WriteAllText(Application.persistentDataPath+filepath+"AddedResolutions.json", json);
    }
}
