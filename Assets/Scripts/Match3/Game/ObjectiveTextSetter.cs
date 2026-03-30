using System.Collections;
using System.Collections.Generic;
using Match3;
using TMPro;
using UnityEngine;

public class ObjectiveTextSetter : MonoBehaviour
{
    public TextMeshProUGUI objectiveTextMesh;

    private void Start()
    {
        LevelData lvl = MatchLevelManager.matchLevelManager.GETCurLevel();
        objectiveTextMesh.text = lvl.objective;
    }
}
