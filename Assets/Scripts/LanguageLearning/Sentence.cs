using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sentence
{
    //the phrases that make up the sentence
    private List<Phrase> sentence = new List<Phrase>();

    //characters that are reserved for internal functions
    private static List<char> reservedChars = new List<char> { '<', '>', '|' };
    
    //characters that end a sentence
    private static List<char> sentenceEndChars => LanguageLearningScriptParser.sentenceEndChars;

    //whether the sentence is in English
    private bool _isEnglish;

    //creates and returns a tuple holding 1. the string representation of this sentence as it should
    //appear based on the passed set of unlocked words and 2. whether the whole sentence is unlocked
    public (String,bool) GetSentenceVal(HashSet<String> unlockedIds)
    {
        String ret = "";
        bool allUnlocked = true;
        foreach (Phrase p in sentence)
        {
            if (!unlockedIds.Contains(p.id) && !p.id.Equals("freewordid") && !p.id.Equals(""))
            {
                allUnlocked = false;
                break;
            }
        }
        foreach (Phrase p in sentence)
        {
            ret += p.GetVal(unlockedIds.Contains(p.id) || allUnlocked);
        }
        return (ret, allUnlocked || !_isEnglish);
    }

    //parse the passed sentence into the sentence object. should be called once per Sentence to initialize
    public void ParseSentence(String sent, bool isEnglish)
    {
        _isEnglish = isEnglish;
        //mode represents whether we are currently recording a phrase or an id for a phrase
        int mode = 0;
        String phrase = "";
        String id = "";
        char[] sentenceSplit = sent.ToCharArray();
        for (int i = 0; i < sentenceSplit.Length; i++)
        {
            if (mode == 0)
            {
                //if the character is an escape character, record the next character without looking at it
                if (sentenceSplit[i].Equals('/') && sentenceSplit.Length > i + 1 &&
                    reservedChars.Contains(sentenceSplit[i + 1]))
                {
                    phrase += sentenceSplit[i + 1];
                    i++;
                }
                //if the character is a marker for start of id, record the previous and begin recording the id for the next one
                else if (sentenceSplit[i].Equals('<'))
                {
                    mode = 1;
                    if (!phrase.Equals(""))
                    {
                        sentence.Add(new Phrase(id, phrase, false, isEnglish));
                        phrase = "";
                        id = "";
                    }
                }
                //if the character is a phrase end character, record the current phrase
                else if (sentenceSplit[i].Equals('|'))
                {
                    if (!phrase.Equals(""))
                    {
                        sentence.Add(new Phrase(id, phrase, false, isEnglish));
                        phrase = "";
                        id = "";
                    }
                }
                //if we've hit end of sentence, record the current phrase
                else if(sentenceEndChars.Contains(sentenceSplit[i]))
                {
                    if (!phrase.Equals(""))
                    {
                        sentence.Add(new Phrase(id, phrase, false, isEnglish));
                        phrase = "";
                        id = "";
                    }
                    sentence.Add(new Phrase(id, sentenceSplit[i].ToString(), true, isEnglish));
                }
                //otherwise add the current character to the end of the phrase we're recording
                else
                {
                    phrase += sentenceSplit[i];
                }
            }
            else if (mode == 1)
            {
                //if we've hit the end of the id, return to phrase recording mode
                if (sentenceSplit[i].Equals('>'))
                {
                    mode = 0;
                }
                //otherwise add the current character to the id
                else
                {
                    id += sentenceSplit[i];
                }
            }
        }
        //if we have a phrase still to record, record it
        if (!phrase.Equals(""))
        {
            sentence.Add(new Phrase(id, phrase, false, isEnglish));
        }
    }
}
