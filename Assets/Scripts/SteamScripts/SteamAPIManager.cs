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
        return ach.Trigger();
    }

    public static float GetProg(string id)
    {
        if (!connected) return -1;
        var stat = new Steamworks.Data.Stat(id);
        var ret = stat.GetFloat();
        return ret;
    }

    public static bool SetProg(string id, float set)
    {
        if (!connected) return false;
        var stat = new Steamworks.Data.Stat(id);
        var ret = stat.Set(set);
        stat.Store();
        return ret;
    }
}
