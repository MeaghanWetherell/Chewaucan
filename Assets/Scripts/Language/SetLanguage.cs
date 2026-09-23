using UnityEngine;
using TMPro;

public class SetLanguage : MonoBehaviour
{
    
    [Tooltip("Camera sensitivity settings dropdown")]
    public TMP_Dropdown langDropdown;
    
    private void Start()
    {
        Init();
    }
    
    //initialize the current language
    private void Init()
    {
        langDropdown.value = LanguageManager.LoadLanguage();
    }
    
    //set the language when changing the dropdown value
    public void SetLang()
    {
        LanguageManager.SaveLanguage(langDropdown.value);
    }
}
