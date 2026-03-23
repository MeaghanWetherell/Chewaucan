using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using TMPro;
using UnityEngine;

public class LanguageLearningManager : MonoBehaviour
{
    //singleton
    public static LanguageLearningManager llm;

    public ClipListPlayer player;

    public Canvas myCanvas;

    public TextMeshProUGUI resText;

    [Tooltip("The targets where the player drags words. Should be exactly 3")]
    public List<DragTarget> targets;

    [Tooltip("List of data objects for all the draggable words, including any that start unlocked")]
    public List<DraggableImgData> unlockWords;

    [Tooltip("The full script of the conversation the minigame focuses on. See example script for format.")]
    public TextAsset script;

    [Tooltip("The text asset that holds all valid sentences (in English) the player can make with the draggable words. See example for format")]
    public TextAsset validSentenceDoc;

    [Tooltip("Prefab that the player clicks and drags icons from")]
    public GameObject draggableFactoryPrefab;

    [Tooltip("Transform under which the manager will instantiate draggable factories for each word")]
    public Transform mainDFPanel;

    [Tooltip("Words that will be unlocked by default. These should still have full image data objects appearing in unlockWords.")]
    public List<String> defaultUnlockedWords;

    [Tooltip("Panel that displays the script of the conversation")]
    public Transform TextPanel;

    [Tooltip("Prefab for sentences that display in the convo script panel")]
    public GameObject sentencePrefab;

    [Tooltip("Narration for each sentence as it appears in order, in both English and the alternate language")]
    public List<AudioClip> sentenceClips;

    [Tooltip("Audio source set to play an 'aww' sound")]
    public AudioSource awwdioSource;
    
    //reference to a component attached to each display line of the script that handles display for that line
    [NonSerialized]public List<SentenceManager> scriptSentences = new List<SentenceManager>();
    
    //ids of the words the player has unlocked
    [NonSerialized]public HashSet<String> unlockedIds;

    private List<ValidSentence> validSentences;
    
    //factories that have been created
    private List<DraggableImageFactory> factories = new List<DraggableImageFactory>();

    //internal representation of the script
    private List<Sentence>[] parsedScript;
    
    //filename that the unlocked words save to
    private static string saveFileName = "LLWords";
    
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
        if (unlockedIds == null)
            unlockedIds = new HashSet<string>();
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

    //creates the text display for the conversation script
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

    //checks if the words the player currently has in the drag targets form a valid sentence and displays a result
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

    //registers a word unlocked with this manager, the DraggableImageFactories and the SentenceManagers
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

    //get and return the appropriate drag targets for the order the sentence should appear in
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

    //gets the word clips in the order they should play based on the sentence
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

    //determines whether the words in the check array are a match for any valid sentence
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
        //parse the text files
        parsedScript = LanguageLearningScriptParser.Parse(script.text);
        validSentences = ValidWordParser.Parse(validSentenceDoc.text);
        unlockedIds = new HashSet<string>();
        //load save data
        Load(SaveHandler.saveHandler.getSavePath());
        SaveHandler.saveHandler.subToSave(Save);
        SaveHandler.saveHandler.subToLoad(Load);
        int panelInd = 0;
        //create a draggable factory for each word
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
