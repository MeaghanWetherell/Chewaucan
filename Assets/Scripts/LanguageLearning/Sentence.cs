using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sentence
{
    private List<Phrase> sentence = new List<Phrase>();

    private static List<char> reservedChars = new List<char> { '<', '>', '|' };

    public String GetSentenceVal(List<String> unlockedIds)
    {
        String ret = "";
        bool allUnlocked = true;
        foreach (Phrase p in sentence)
        {
            if (!unlockedIds.Contains(p.id))
            {
                allUnlocked = false;
                break;
            }
        }
        foreach (Phrase p in sentence)
        {
            ret += p.GetVal(unlockedIds.Contains(p.id) || allUnlocked);
        }
        return ret;
    }

    public void ParseSentence(String sent, bool isEnglish)
    {
        int mode = 0;
        String phrase = "";
        String id = "";
        bool isPunct = true;
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
                        sentence.Add(new Phrase(id, phrase, isPunct, isEnglish));
                        phrase = "";
                        id = "";
                        isPunct = true;
                    }
                }
                else if (sentenceSplit[i].Equals('|'))
                {
                    if (!phrase.Equals(""))
                    {
                        sentence.Add(new Phrase(id, phrase, isPunct, isEnglish));
                        phrase = "";
                        id = "";
                        isPunct = true;
                    }
                }
                else
                {
                    phrase += sentenceSplit[i];
                    if (Char.IsLetterOrDigit(sentenceSplit[i]))
                    {
                        isPunct = false;
                    }
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
            sentence.Add(new Phrase(id, phrase, isPunct, isEnglish));
        }
    }
}
