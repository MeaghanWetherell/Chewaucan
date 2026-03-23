using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//records a single phrase in a sentence
public class Phrase
{
    //id of the phrase. if "freewordid" is always unlocked, if "" is only unlocked if the whole sentence is unlocked or it is punctuation
    public String id;

    public String value;

    //whether the phrase is entirely punctuation, in which case it will always return as unlocked
    public bool isPunct;

    public bool isEnglish;

    public Phrase(String inId, String inValue, bool inPunct, bool inEnglish)
    {
        id = inId;
        value = inValue;
        isPunct = inPunct;
        isEnglish = inEnglish;
    }

    //get the string of the phrase based on whether it is unlocked or not
    public String GetVal(bool isUnlocked)
    {
        if (isUnlocked || isPunct || id.Equals("freewordid"))
        {
            return "<style=\"LLUnlockedWord\">"+value+"</style>";
        }

        String ret = "<style=\"LLLockedWord\">";
        char[] val = value.ToCharArray();
        for (int i = 0; i < val.Length; i++)
        {
            if (Char.IsLetterOrDigit(val[i]) && isEnglish)
            {
                ret += "?";
            }
            else
            {
                ret += val[i];
            }
        }
        ret += "</style>";
        return ret;
    }
}
