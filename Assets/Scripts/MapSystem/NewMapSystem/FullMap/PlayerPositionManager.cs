using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.Json;
using System.IO;
using System.Text.Json.Serialization;
using Misc;
using ScriptTags;
using UnityEngine;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.SceneManagement;
using UnityEngine.Windows;
using File = System.IO.File;

public class PlayerPositionManager : MonoBehaviour
{
    public static PlayerPositionManager playerPositionManager;

    //index 0 is modern, index 1 is pleistocene
    private List<Vector3> nextPlayerPosition = new List<Vector3>();
    
    [NonSerialized]public bool loadModernMap = true;

    public string playerPosFileName;

    public string lastMapFileName;

    public string playerMoveTypeFileName;

    //index 0 is modern, index 1 is pleistocene
    //true means land movement
    private List<bool> nextPlayerMoveType = new List<bool>() {true, true};

    private PlayerMovementController _movementController;
    
    public void Reset()
    {
        nextPlayerPosition = new List<Vector3>();
        nextPlayerPosition.Add(Vector3.negativeInfinity);
        nextPlayerPosition.Add(Vector3.negativeInfinity);
        nextPlayerMoveType = new List<bool>() {true, true};
    }

    private void Awake()
    {
        if (playerPositionManager != null)
        {
            Destroy(gameObject);
            return;
        }
        SceneManager.activeSceneChanged += FindPlayerWhenSceneChanged;
        playerPositionManager = this;
        DontDestroyOnLoad(transform.gameObject);
        SaveHandler.saveHandler.subToSave(Save);
        SaveHandler.saveHandler.subToLoad(Load);
    }

    private void Load(string path)
    {
        string json;
        try
        {
            var opts = new JsonSerializerOptions
            {
                IncludeFields = true,
                IgnoreReadOnlyProperties = true,
                NumberHandling = JsonNumberHandling.AllowNamedFloatingPointLiterals
            };
            json = File.ReadAllText(path + "/" + playerPosFileName + ".json");
            nextPlayerPosition = JsonSerializer.Deserialize<List<Vector3>>(json, opts);
            //Debug.Log(nextPlayerPosition[0]);
            //Debug.Log(nextPlayerPosition[1]);
        }
        catch (IOException){ Reset(); }

        try
        {
            json = File.ReadAllText(path + "/" + lastMapFileName + ".json");
            loadModernMap = JsonSerializer.Deserialize<bool>(json);
        }
        catch (IOException)
        {
            loadModernMap = true;
        }
        
        try
        {
            json = File.ReadAllText(path + "/" + playerMoveTypeFileName + ".json");
            nextPlayerMoveType = JsonSerializer.Deserialize<List<bool>>(json);
        }
        catch (IOException){ nextPlayerMoveType = new List<bool>() {true, true}; }
    }

    private void Save(string path)
    {
        var opts = new JsonSerializerOptions
        {
            IncludeFields = true,
            IgnoreReadOnlyProperties = true,
            NumberHandling = JsonNumberHandling.AllowNamedFloatingPointLiterals
        };
        string posSave = JsonSerializer.Serialize(nextPlayerPosition, opts);
        File.WriteAllText(path+"/"+playerPosFileName+".json", posSave);
        string lastMap = JsonSerializer.Serialize(loadModernMap);
        File.WriteAllText(path+"/"+lastMapFileName+".json", lastMap);
        string moveTypes = JsonSerializer.Serialize(nextPlayerMoveType);
        File.WriteAllText(path+"/"+playerMoveTypeFileName+".json", moveTypes);
    }
    
    public Vector3 getCurrentPlayerPosition(int index)
    {
        return nextPlayerPosition[index];
    }

    public void setPlayerPosition(Vector3 position, int index)
    {
        nextPlayerPosition[index] = position;
    }

    private void Update()
    {
        if (Player.player != null)
        {
            GameObject player = Player.player;
            if (_movementController == null)
                _movementController = player.GetComponent<PlayerMovementController>();
            if (SceneLoadWrapper.sceneLoadWrapper.modernMapScenes.Contains(SceneManager.GetActiveScene().name))
            {
                setPlayerPosition(player.transform.position, 0);
                nextPlayerMoveType[0] = _movementController.isWalkingOrClimbing();
            }
            else
            {
                setPlayerPosition(player.transform.position, 1);
                nextPlayerMoveType[1] = _movementController.isWalkingOrClimbing();
            }
        }
    }

    private void FindPlayerWhenSceneChanged(Scene current, Scene next)
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (next.name.Equals("Modern Map"))
        {
            loadModernMap = true;
        }
        else if (next.name.Equals("PleistoceneMap"))
        {
            loadModernMap = false;
        }
        if (player != null && !float.IsNegativeInfinity(nextPlayerPosition[SceneLoadWrapper.sceneLoadWrapper.currentSceneType].x))
        {
            LandMovement move = player.GetComponent<LandMovement>();
            if (move != null)
            {
                player.transform.position = nextPlayerPosition[SceneLoadWrapper.sceneLoadWrapper.currentSceneType];
                move.enabled = nextPlayerMoveType[SceneLoadWrapper.sceneLoadWrapper.currentSceneType];
            }
        }
    }
}
