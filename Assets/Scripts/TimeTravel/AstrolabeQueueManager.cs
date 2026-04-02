using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using UnityEngine;

public class AstrolabeQueueManager : MonoBehaviour
{
    public static AstrolabeQueueManager queueManager;

    [Tooltip("Location to save the destination queue for the modern map")]
    public string modernMapSaveFileName;
    
    [Tooltip("Location to save the destination queue for the pleistocene map")]
    public string pleistMapSaveFileName;

    private Stack<v3Wrapper> _modernStack;

    private Stack<v3Wrapper> _pleistStack;

    private void Awake()
    {
        if (queueManager != null)
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);
        queueManager = this;
        SaveHandler.saveHandler.subToLoad(Load);
        SaveHandler.saveHandler.subToSave(Save);
    }
    
    //send map = 1 for modern, map = 2 for pleistocene
    public void EnqueueDestination(Vector3 dest, int map = 2)
    {
        v3Wrapper destWrapper = new v3Wrapper(dest);
        if (map == 1)
        {
            _modernStack.Push(destWrapper);
        }
        else
        {
            _pleistStack.Push(destWrapper);
        }
        AstrolabeUIIconManager.SetNewDest(true, map-1);
    }

    private static Vector3 pop(Stack<v3Wrapper> queue)
    {
        v3Wrapper res;
        if (!queue.TryPop(out res))
            return Vector3.negativeInfinity;
        return res.getVector();
    }

    private static Vector3 peek(Stack<v3Wrapper> queue)
    {
        v3Wrapper res;
        if (!queue.TryPeek(out res))
            return Vector3.negativeInfinity;
        return res.getVector();
    }
    
    //1 for modern 2 for pleistocene
    public static Vector3 peek(int map = 2)
    {
        if (queueManager == null) return Vector3.negativeInfinity;
        if (map == 1)
            return peek(queueManager._modernStack);
        else
        {
            return peek(queueManager._pleistStack);
        }
    }

    public static Vector3 peekModern()
    {
        if (queueManager == null) return Vector3.negativeInfinity;
        return peek(queueManager._modernStack);
    }

    public static Vector3 peekPleist()
    {
        if (queueManager == null) return Vector3.negativeInfinity;
        return peek(queueManager._pleistStack);
    }
    
    //1 for modern 2 for pleistocene
    public static Vector3 pop(int map = 2)
    {
        if (queueManager == null) return Vector3.negativeInfinity;
        if (map == 1)
            return pop(queueManager._modernStack);
        else
        {
            return pop(queueManager._pleistStack);
        }
    }
    
    public static Vector3 popModern()
    {
        if (queueManager == null) return Vector3.negativeInfinity;
        return pop(queueManager._modernStack);
    }

    public static Vector3 popPleist()
    {
        if (queueManager == null) return Vector3.negativeInfinity;
        return pop(queueManager._pleistStack);
    }

    public void Save(string path)
    {
        string svJson = JsonSerializer.Serialize(_modernStack);
        File.WriteAllText(path+"/" +  modernMapSaveFileName +".json", svJson);
        svJson = JsonSerializer.Serialize(_pleistStack);
        File.WriteAllText(path+"/" +  pleistMapSaveFileName +".json", svJson);
    }

    public void Load(string path)
    {
        try
        {
            _modernStack =
                JsonSerializer.Deserialize<Stack<v3Wrapper>>(File.ReadAllText(path + "/" + modernMapSaveFileName + ".json"));
            if(_modernStack.Count > 0)
                AstrolabeUIIconManager.SetNewDest(true, 0);
        }
        catch (IOException)
        {
            _modernStack = new Stack<v3Wrapper>();
        }
        try
        {
            _pleistStack =
                JsonSerializer.Deserialize<Stack<v3Wrapper>>(File.ReadAllText(path + "/" + pleistMapSaveFileName + ".json"));
            if(_pleistStack.Count > 0)
                AstrolabeUIIconManager.SetNewDest(true, 1);
        }
        catch (IOException)
        {
            _pleistStack = new Stack<v3Wrapper>();
        }
    }
}
