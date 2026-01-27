using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using TMPro;
using UnityEngine;

public class LanguageLearningManager : MonoBehaviour
{
    public static LanguageLearningManager llm;

    public TextAsset script;

    public List<Transform> draggableImageFactoryPanels;

    public List<String> defaultUnlockedWords;

    [NonSerialized]public List<String> unlockedIds;
    
    private List<DraggableImageFactory> factories = new List<DraggableImageFactory>();

    private List<Sentence>[] parsedScript;
    
    private static string saveFileName = "LLWords";

    public TextMeshProUGUI altLangMainText;

    public TextMeshProUGUI englishMainText;
    
    private void Load(string path)
    {
        try
        {
            unlockedIds =
                JsonSerializer.Deserialize<List<String>>(File.ReadAllText(path + "/" + saveFileName + ".json"));
        }
        catch (IOException)
        {
            unlockedIds = defaultUnlockedWords;
        }
    }

    private void Save(string path)
    {
        string svJson = JsonSerializer.Serialize(unlockedIds);
        File.WriteAllText(path+"/" +  saveFileName +".json", svJson);
    }
    
    private void OnDestroy()
    {
        SaveHandler.saveHandler.unsubToSave(Save);
        SaveHandler.saveHandler.unsubToLoad(Load);
        Save(SaveHandler.saveHandler.getSavePath());
    }

    private void SetTexts()
    {
        String text = "";
        List<Sentence> targ = parsedScript[0];
        foreach (Sentence sent in targ)
        {
            text += sent.GetSentenceVal(unlockedIds);
        }
        altLangMainText.text = text;
        text = "";
        targ = parsedScript[1];
        foreach (Sentence sent in targ)
        {
            text += sent.GetSentenceVal(unlockedIds);
        }
        englishMainText.text = text;
    }

    public void Awake()
    {
        parsedScript = LanguageLearningScriptParser.Parse(script.text); 
        Load(SaveHandler.saveHandler.getSavePath());
        SaveHandler.saveHandler.subToSave(Save);
        SaveHandler.saveHandler.subToLoad(Load);
        foreach (Transform draggableImageFactoryPanel in draggableImageFactoryPanels)
        {
            foreach (Transform child in draggableImageFactoryPanel)
            {
                DraggableImageFactory fact = child.GetComponent<DraggableImageFactory>();
                if (fact != null)
                {
                    factories.Add(fact);
                    fact.unlockState = unlockedIds.Contains(fact.wordEnglish.ToLower());
                }
            }
        }
        SetTexts();
    }
}
