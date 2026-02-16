using System;
using System.Collections;
using System.Collections.Generic;
using ScriptTags;
using UnityEngine;

public class FallOffMapAch : MonoBehaviour
{
    private void Update()
    {
        if (Player.player != null && Player.player.transform.position.y < -20)
        {
            SteamAPIManager.UnlockAch("FellOffMap");
        }
    }
}
