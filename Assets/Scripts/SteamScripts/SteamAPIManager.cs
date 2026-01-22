using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class SteamAPIManager
{
    private const int appID = 3512920;
    private static bool connected = false;

    public static bool init()
    {
        if (connected) return true;
        try
        {
            Steamworks.SteamClient.Init(appID);
            connected = true;
            return connected;
        }
        catch (Exception)
        {
            Debug.Log("Failed to get Steam connection.");
            return false;
        }
    }

    public static bool UnlockAch(string id)
    {
        if (!connected) return false;
        var ach = new Steamworks.Data.Achievement(id);
        ach.Trigger();
        return true;
    }


}
