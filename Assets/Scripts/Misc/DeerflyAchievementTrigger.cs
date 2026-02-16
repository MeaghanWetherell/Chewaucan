using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeerflyAchievementTrigger : MonoBehaviour
{
    public Narration.Narration deerflyNarration;
    
    private void Awake()
    {
        if (DeerflySwarmTrigger.swarmed && !deerflyNarration.HasPlayed())
        {
            DeerflySwarmTrigger.swarmed = false;
            deerflyNarration.Begin();
            SteamAPIManager.UnlockAch("DeerflyAchievement");
        }
    }
}
