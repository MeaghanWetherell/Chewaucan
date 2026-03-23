using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class LanguageLearningScriptParser
{
    //the characters that mark the end of a sentence as far as the parser is concerned.
    public static List<char> sentenceEndChars = new List<char> {'.','?','!'};
    
    //parse a validly formatted script file into a Sentence object representation
    public static List<Sentence>[] Parse(String text)
    {
        //init
        String[] textS = text.Split("\n");
        int ind = 0;
        List<Sentence>[] ret = new List<Sentence>[2];
        ret[0] = new List<Sentence>();
        ret[1] = new List<Sentence>();
        bool isEnglish = false;
        Sentence add;
        //loop over each line in the text
        foreach (String rawLine in textS)
        {
            string rl = rawLine.Trim('\r');
            //ignore lines that are empty or start with the comment char 
            if ((rl.Length > 0 && rl[0].Equals('#'))||rl.Trim().Length == 0)
            {
                continue;
            }
            //when we hit a --break-- line we know we will now be recording the English part of the script
            if (rl.Equals("--break--"))
            {
                ind = 1;
                isEnglish = true;
                continue;
            }
            Char[] chars = rl.ToCharArray();
            String sent = "";
            //loop over the individual characters in the line
            for (int i = 0; i < chars.Length; i++)
            {
                //if we find the / break character, we should automatically record the following character, regardless what it is
                if (chars[i].Equals('/') && i+1 < chars.Length && sentenceEndChars.Contains(chars[i+1]))
                {
                    sent += chars[i + 1];
                    i++;
                }
                //if the character we found is a sentence ender, record what we've found so far as a new sentence and reset the sentence string
                else if (sentenceEndChars.Contains(chars[i]))
                {
                    sent += chars[i];
                    add = new Sentence();
                    add.ParseSentence(sent, isEnglish);
                    ret[ind].Add(add);
                    sent = "";
                }
                //otherwise add the current character to the sentence string
                else
                {
                    sent += chars[i];
                }
            }
            //if there's still a setence left in the sentence string, record it
            if (!sent.Trim().Equals(""))
            {
                sent += "\n";
                add = new Sentence();
                add.ParseSentence(sent, isEnglish);
                ret[ind].Add(add);
            }
        }
        return ret;
    }
}
