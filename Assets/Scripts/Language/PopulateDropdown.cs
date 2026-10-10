using TMPro;
using UnityEngine;

public class PopulateDropdown : MonoBehaviour
{
    private TMP_Dropdown langDropdown;
    void Awake()
    {
        langDropdown =  GetComponent<TMP_Dropdown>();
        for (int i = 0; i < langDropdown.options.Count; i++)
        {
            langDropdown.options[i].text = LanguageManager.GetLanguageObj().dropdownButtons[i];
        }
        
    }
}
