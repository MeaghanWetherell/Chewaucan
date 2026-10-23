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
    
    //event that invokes when language is updated
    //private static event Action UpdateLangText;
    public delegate void UpdateLangText();
    public static UpdateLangText updateLangText;

    public static void InstantiateObj()
    {
        lang = (Language) LoadLanguage();
        switch (lang)
        {
            case Language.English:
                langObj = Resources.Load<LanguageObject>("Objects/EN");
                break;
            case Language.French:
                langObj = Resources.Load<LanguageObject>("Objects/FR");
                break;
        }
        Debug.Log("instantiate langobj = " + langObj);
        updateLangText?.Invoke();

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

        lang = (Language) curLang;
        
        return curLang;

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
        if (langObj != null)
        {
            return langObj;
        }
        else
        {
            return Resources.Load<LanguageObject>("Objects/EN");
        }
    }

    public static int GetLanguagesNumber()
    {
        return Enum.GetNames(typeof(Language)).Length;
    }
    
    public static LanguageObject GetEnglishObj()
    {
        return Resources.Load<LanguageObject>("Objects/EN");
    }
    
}
