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

    public ClipListPlayer player;

    public Canvas myCanvas;

    public TextMeshProUGUI resText;

    public List<DragTarget> targets;

    public List<DraggableImgData> unlockWords;

    public TextAsset script;

    public TextAsset validWords;

    public GameObject draggableFactoryPrefab;

    public Transform mainDFPanel;

    public List<String> defaultUnlockedWords;

    [NonSerialized]public HashSet<String> unlockedIds;
    
    private List<DraggableImageFactory> factories = new List<DraggableImageFactory>();

    private List<Sentence>[] parsedScript;
    
    private static string saveFileName = "LLWords";

    public Transform TextPanel;

    [NonSerialized]public List<SentenceManager> scriptSentences = new List<SentenceManager>();

    public GameObject sentencePrefab;

    public List<AudioClip> sentenceClips;

    public AudioSource awwdioSource;

    private List<ValidSentence> validSentences;
    
    private void Load(string path)
    {
        try
        {
            unlockedIds =
                JsonSerializer.Deserialize<HashSet<String>>(File.ReadAllText(path + "/" + saveFileName + ".json"));
        }
        catch (IOException)
        {
            unlockedIds = new HashSet<string>();
            foreach (string word in defaultUnlockedWords)
                unlockedIds.Add(word);
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
        int i = 0;
        List<Sentence> targ = parsedScript[0];
        foreach (Sentence sent in targ)
        {
            GameObject sentObj = Instantiate(sentencePrefab, TextPanel);
            SentenceManager sentM = sentObj.GetComponent<SentenceManager>();
            if (i >= sentenceClips.Count)
            {
                sentM.Init(sent, null, unlockedIds);
            }
            else
            {
                sentM.Init(sent, sentenceClips[i], unlockedIds);
                i++;
            }
            scriptSentences.Add(sentM);
            
        }
        targ = parsedScript[1];
        foreach (Sentence sent in targ)
        {
            GameObject sentObj = Instantiate(sentencePrefab, TextPanel);
            SentenceManager sentM = sentObj.GetComponent<SentenceManager>();
            if (i >= sentenceClips.Count)
            {
                sentM.Init(sent, null, unlockedIds);
            }
            else
            {
                sentM.Init(sent, sentenceClips[i], unlockedIds);
                i++;
            }
            scriptSentences.Add(sentM);
        }
    }

    public void AttemptUnlock()
    {
        string[] attemptWords = new string[3];
        foreach (DragTarget target in targets)
        {
            if (target.parent == null)
                return;
        }
        attemptWords[0] = targets[0].parent.wordEnglish.ToLower();
        attemptWords[1] = targets[1].parent.wordEnglish.ToLower();
        attemptWords[2] = targets[2].parent.wordEnglish.ToLower();
        ValidSentence v = CheckAttempt(attemptWords);
        bool ret = v != null;
        if (ret)
        {
            List<AudioClip> clips = GetClipList(v);
            List<DragTarget> targetOrder = GetTargetList(v);
            List<bool> isEnglish = new List<bool> { true, true, true, false, false, false};
            player.PlayClips(clips, targetOrder, isEnglish);
            foreach (string word in attemptWords)
            {
                Unlock(word);
            }
            foreach (DragTarget targ in targets)
            {
                targ.SetText();
            }
            resText.text = "That's a valid sentence!";
        }
        else
        {
            awwdioSource.Play();
            resText.text = "That's not a valid sentence!";
        }
    }

    private void Unlock(string word)
    {
        unlockedIds.Add(word);
        Save(SaveHandler.saveHandler.getSavePath());
        foreach (DraggableImageFactory factory in factories)
        {
            if(factory.wordEnglish.ToLower().Equals(word))
                factory.Unlock();
        }
        foreach (SentenceManager sm in scriptSentences)
        {
            sm.Refresh(unlockedIds);
        }
    }

    private List<DragTarget> GetTargetList(ValidSentence v)
    {
        List<DragTarget> ret = new List<DragTarget>();
        foreach (int i in v.englishOrder)
        {
            ret.Add(targets[i-1]);
        }

        foreach (int j in v.altLangOrder)
        {
            ret.Add(targets[j-1]);
        }
        return ret;
    }

    private List<AudioClip> GetClipList(ValidSentence v)
    {
        List<AudioClip> ret = new List<AudioClip>();
        foreach (int i in v.englishOrder)
        {
            ret.Add(targets[i-1].parent.wordClipEnglish);
        }

        foreach (int j in v.altLangOrder)
        {
            ret.Add(targets[j-1].parent.wordClipAltLang);
        }
        return ret;
    }

    private ValidSentence CheckAttempt(string[] check)
    {
        foreach (ValidSentence s in validSentences)
        {
            if (s.isMatch(check))
                return s;
        }

        return null;
    }

    public void Awake()
    {
        parsedScript = LanguageLearningScriptParser.Parse(script.text);
        validSentences = ValidWordParser.Parse(validWords.text);
        Load(SaveHandler.saveHandler.getSavePath());
        SaveHandler.saveHandler.subToSave(Save);
        SaveHandler.saveHandler.subToLoad(Load);
        int panelInd = 0;
        foreach (DraggableImgData data in unlockWords)
        {
            GameObject newFactory = Instantiate(draggableFactoryPrefab, mainDFPanel.GetChild(panelInd));
            DraggableImageFactory df = newFactory.GetComponent<DraggableImageFactory>();
            df.SetUp(unlockedIds.Contains(data.englishWord.ToLower()), myCanvas, data);
            factories.Add(df);
            panelInd++;
            if (panelInd >= mainDFPanel.childCount)
                panelInd = 0;
        }
        SetTexts();
    }
}
