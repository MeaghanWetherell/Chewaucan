using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TMPro
{
    [Serializable]
    [CreateAssetMenu(fileName = "CustomTextValidator.asset", menuName = "TextMeshPro/Input Validators/RestrictChars", order = 100)]
    public class InputValidator : TMP_InputValidator
    {
        public List<char> invalidChars;
        
        // Custom text input validation function
        public override char Validate(ref string text, ref int pos, char ch)
        {
            if (!invalidChars.Contains(ch) && ch > 31)
            {
                text += ch;
                pos += 1;
                return ch;
            }
            return (char)0;
        }
    } 
}

