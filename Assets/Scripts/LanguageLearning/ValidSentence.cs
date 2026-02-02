using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ValidSentence
{
    public String[] sentence = new String[3];

    public int[] altLangOrder = new int[3];

    public int[] englishOrder = new int[3];

    public ValidSentence(string readString)
    {
        readString = readString.Trim();
        string[] stringSplit = readString.Split(" ");
        if (stringSplit.Length != 3 && stringSplit.Length != 4 && stringSplit.Length != 5)
        {
            Debug.LogError("Got invalid sentence format from valid sentence text file");
        }
        sentence[0] = stringSplit[0];
        sentence[1] = stringSplit[1];
        sentence[2] = stringSplit[2];
        if (stringSplit.Length > 3)
        {
            char[] altOrder = stringSplit[3].ToCharArray();
            if (altOrder.Length != 3)
            {
                Debug.LogError("Got invalid sentence format from valid sentence text file");
            }
            altLangOrder[0] = Int32.Parse(altOrder[0].ToString());
            altLangOrder[1] = Int32.Parse(altOrder[1].ToString());
            altLangOrder[2] = Int32.Parse(altOrder[2].ToString());
        }
        else
        {
            altLangOrder[0] = 1;
            altLangOrder[1] = 2;
            altLangOrder[2] = 3;
        }
        if (stringSplit.Length > 4)
        {
            char[] engOrder = stringSplit[4].ToCharArray();
            if (engOrder.Length != 3)
            {
                Debug.LogError("Got invalid sentence format from valid sentence text file");
            }
            englishOrder[0] = Int32.Parse(engOrder[0].ToString());
            englishOrder[1] = Int32.Parse(engOrder[1].ToString());
            englishOrder[2] = Int32.Parse(engOrder[2].ToString());
        }
        else
        {
            englishOrder[0] = 1;
            englishOrder[1] = 2;
            englishOrder[2] = 3;
        }
    }

    public bool isMatch(string[] input)
    {
        return input.Length == 3 && input[0].Equals(sentence[0]) && input[1].Equals(sentence[1]) && input[2].Equals(sentence[2]);
    }
}
