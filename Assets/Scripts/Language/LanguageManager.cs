using System;
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
    private enum Language {English, French}
    private static Language lang = Language.English;
        
    //current language object
    private static LanguageObject langObj;

    private void Awake()
    {
        lang = (Language) LoadLanguage();
        InstantiateObj();
    }

    public static void InstantiateObj()
    {
        if (lang == 0)
        {
            langObj = Resources.Load<LanguageObject>("Objects/EN");
            
        }
        else
        {
            langObj = Resources.Load<LanguageObject>("Objects/FR");
        }
        Debug.Log("instantiate langobj = " + langObj);
        
    }
    
    public static int LoadLanguage()
    {
        Language curLang = Language.English;
        
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
            curLang = JsonSerializer.Deserialize<Language>(json, opts);
            
        }
        catch (IOException)
        {
            curLang = Language.English;
        }

        lang = curLang;
        InstantiateObj();
        
        return (int) curLang;

    }
    
    public static void SaveLanguage(int newLang)
    {
        lang = (Language) newLang;
        
        var opts = new JsonSerializerOptions
        {
            IncludeFields = true,
            IgnoreReadOnlyProperties = true,
            NumberHandling = JsonNumberHandling.AllowNamedFloatingPointLiterals
        };
        string langStr = JsonSerializer.Serialize(newLang, opts);
        File.WriteAllText(Application.persistentDataPath+"/"+filepath+"LanguageSettings.json", langStr);
        InstantiateObj();
    }

    public static int GetLanguageInt()
    {
        return (int) lang;
    }
    
    public static LanguageObject GetLanguageObj()
    {
        return langObj;
    }

    public static int GetLanguagesNumber()
    {
        return Enum.GetNames(typeof(Language)).Length;
    }
    
}
