using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ValidWordParser 
{
   //parses a valid sentence text file into a list of ValidSentence objects
   public static List<ValidSentence> Parse(string text)
   {
      List<ValidSentence> ret = new List<ValidSentence>();
      String[] textS = text.Split("\n");
      foreach (String rawLine in textS)
      {
         string rl = rawLine.Trim('\r');
         if(rl[0].Equals('#'))
            continue;
         ret.Add(new ValidSentence(rl));
      }
      return ret;
   }
}
