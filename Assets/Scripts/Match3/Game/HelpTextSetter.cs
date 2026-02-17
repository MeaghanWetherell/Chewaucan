using System;
using System.Collections;
using System.Collections.Generic;
using Match3;
using TMPro;
using UnityEngine;

public class HelpTextSetter : MonoBehaviour
{
    public GameObject anatomy;

    public GameObject species;

    public GameObject identical;

    private void Start()
    {
        LevelData lvl = MatchLevelManager.matchLevelManager.GETCurLevel();
        anatomy.SetActive(false);
        species.SetActive(false);
        identical.SetActive(false);
        switch (lvl.matchType)
        {
            case MatchObject.MatchType.identical:
                identical.SetActive(true);
                break;
            case MatchObject.MatchType.sameBone:
                anatomy.SetActive(true);
                break;
            case MatchObject.MatchType.sameSpecies:
                species.SetActive(true);
                break;
        }
    }
}
