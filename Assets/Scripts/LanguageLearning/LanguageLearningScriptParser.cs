using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class LanguageLearningScriptParser
{
    private static List<char> sentenceEndChars = new List<char> {'.','?','!'};
    
    public static List<Sentence>[] Parse(String text)
    {
        String[] textS = text.Split("\n");
        int ind = 0;
        List<Sentence>[] ret = new List<Sentence>[2];
        ret[0] = new List<Sentence>();
        ret[1] = new List<Sentence>();
        bool isEnglish = false;
        Sentence add;
        foreach (String rawLine in textS)
        {
            string rl = rawLine.Trim('\r');
            if (rl.Length > 0 && rl[0].Equals('#'))
            {
                continue;
            }
            if (rl.Equals("--break--"))
            {
                ind = 1;
                isEnglish = true;
                continue;
            }
            Char[] chars = rl.ToCharArray();
            String sent = "";
            for (int i = 0; i < chars.Length; i++)
            {
                if (chars[i].Equals('/') && i+1 < chars.Length && sentenceEndChars.Contains(chars[i+1]))
                {
                    sent += chars[i + 1];
                    i++;
                }
                else if (sentenceEndChars.Contains(chars[i]))
                {
                    sent += chars[i];
                    add = new Sentence();
                    add.ParseSentence(sent, isEnglish);
                    ret[ind].Add(add);
                    sent = "";
                }
                else
                {
                    sent += chars[i];
                }
            }
            sent += "\n";
            add = new Sentence();
            add.ParseSentence(sent, isEnglish);
            ret[ind].Add(add);
        }
        return ret;
    }
}
