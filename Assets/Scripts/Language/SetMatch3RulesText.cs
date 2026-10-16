using TMPro;
using UnityEngine;

public class SetMatch3RulesText : MonoBehaviour
{
    public enum RulesType{Anatomy, Species, Both};
    public RulesType rulesType;
    private TextMeshProUGUI textMesh;

    void OnEnable()
    {
        textMesh = GetComponent<TextMeshProUGUI>();
        textMesh.text = LanguageManager.GetLanguageObj().GetMatch3Rules((int) rulesType);
    }

}
