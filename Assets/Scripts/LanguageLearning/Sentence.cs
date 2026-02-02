using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sentence
{
    private List<Phrase> sentence = new List<Phrase>();

    private static List<char> reservedChars = new List<char> { '<', '>', '|' };
    
    private static List<char> sentenceEndChars = new List<char> {'.','?','!'};

    private bool _isEnglish;

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

    public void ParseSentence(String sent, bool isEnglish)
    {
        _isEnglish = isEnglish;
        int mode = 0;
        String phrase = "";
        String id = "";
        char[] sentenceSplit = sent.ToCharArray();
        for (int i = 0; i < sentenceSplit.Length; i++)
        {
            if (mode == 0)
            {
                if (sentenceSplit[i].Equals('/') && sentenceSplit.Length > i + 1 &&
                    reservedChars.Contains(sentenceSplit[i + 1]))
                {
                    phrase += sentenceSplit[i + 1];
                    i++;
                }
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
                else if (sentenceSplit[i].Equals('|'))
                {
                    if (!phrase.Equals(""))
                    {
                        sentence.Add(new Phrase(id, phrase, false, isEnglish));
                        phrase = "";
                        id = "";
                    }
                }
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
                else
                {
                    phrase += sentenceSplit[i];
                }
            }

            else if (mode == 1)
            {
                if (sentenceSplit[i].Equals('>'))
                {
                    mode = 0;
                }
                else
                {
                    id += sentenceSplit[i];
                }
            }
        }

        if (!phrase.Equals(""))
        {
            sentence.Add(new Phrase(id, phrase, false, isEnglish));
        }
    }
}
