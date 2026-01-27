using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Phrase
{
    public String id;

    public String value;

    public bool isPunct;

    public bool isEnglish;

    public Phrase(String inId, String inValue, bool inPunct, bool inEnglish)
    {
        id = inId;
        value = inValue;
        isPunct = inPunct;
        isEnglish = inEnglish;
    }

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
