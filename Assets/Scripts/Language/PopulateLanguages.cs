using System;
using TMPro;
using UnityEngine;

public class PopulateLanguages : MonoBehaviour
{
    private TMP_Dropdown langDropdown;
    void OnEnable()
    {
        langDropdown =  GetComponent<TMP_Dropdown>();
        for (int i = 0; i < LanguageManager.GetLanguagesNumber(); i++)
        {
            langDropdown.options.Add(new TMP_Dropdown.OptionData {text = LanguageManager.GetLanguageObj().dropdownLanguages[i]});
        }
        
    }
}
