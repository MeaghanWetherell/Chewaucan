using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;
using UnityEngine;
using Language;

public class LanguageManager : MonoBehaviour
{
    //path at which to save language settings
    private static string filepath = "Chewaucan/SavedSettings/LanguageSettings";
    
    //current language set
    //ENGLISH = 0, FRENCH = 1
    private static int lang;
        
    //current language object
    private static LanguageObject langObj;

    private void Awake()
    {
        lang = LoadLanguage();
        InstantiateObj();
    }

    public static void InstantiateObj()
    {
        Debug.Log("instantiate langobj");
        if (lang == 0)
        {
            langObj = Resources.Load<LanguageObject>("Objects/EN");
        }
        else
        {
            langObj = Resources.Load<LanguageObject>("Objects/FR");
        }
        
    }
    
    public static int LoadLanguage()
    {
        int curLang = 0;
        
        string json;

        try
        {
            var opts = new JsonSerializerOptions
            {
                IncludeFields = true,
                IgnoreReadOnlyProperties = true,
                NumberHandling = JsonNumberHandling.AllowNamedFloatingPointLiterals
            };
            json = File.ReadAllText(Application.persistentDataPath + "/" + filepath + "LanguageSettings.json");
            curLang = JsonSerializer.Deserialize<int>(json, opts);
            
        }
        catch (IOException)
        {
            curLang = 0;
        }

        lang = curLang;
        InstantiateObj();
        
        return curLang;

    }
    
    public static void SaveLanguage(int newLang)
    {
        lang = newLang;
        
        var opts = new JsonSerializerOptions
        {
            IncludeFields = true,
            IgnoreReadOnlyProperties = true,
            NumberHandling = JsonNumberHandling.AllowNamedFloatingPointLiterals
        };
        string langStr = JsonSerializer.Serialize(newLang, opts);
        File.WriteAllText(Application.persistentDataPath+"/"+filepath+"LanguageSettings.json", langStr);
    }

    public static int GetLanguage()
    {
        return lang;
    }
    
    public static LanguageObject GetLanguageObj()
    {
        return langObj;
    }
    
}
