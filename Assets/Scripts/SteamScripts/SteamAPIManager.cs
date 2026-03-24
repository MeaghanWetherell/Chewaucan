using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//handles interactions with the steam api
public static class SteamAPIManager
{
    private const int appID = 3512920;
    private static bool connected = false;

    //initializes the connection with the steam api, must be called before using steam API methods
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

    //unlocks the steam achievement with the passed id
    public static bool UnlockAch(string id)
    {
        if (!connected) return false;
        var ach = new Steamworks.Data.Achievement(id);
        return ach.Trigger();
    }

    //gets the stat progress for the stat with the passed id
    public static float GetProg(string id)
    {
        if (!connected) return -1;
        var stat = new Steamworks.Data.Stat(id);
        var ret = stat.GetFloat();
        return ret;
    }

    //sets the stat progress for the stat with the passed id
    public static bool SetProg(string id, float set)
    {
        if (!connected) return false;
        var stat = new Steamworks.Data.Stat(id);
        var ret = stat.Set(set);
        stat.Store();
        return ret;
    }
}
