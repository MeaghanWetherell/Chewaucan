using System;
using System.Collections;
using System.Collections.Generic;
using Misc;
using UnityEngine;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.SceneManagement;

public class PlayerPositionManager : MonoBehaviour
{
    public static PlayerPositionManager playerPositionManager;

    //index 0 is modern, index 1 is pleistocene
    private List<Vector3> nextPlayerPosition = new List<Vector3>();
    
    private GameObject player;
    
    void Start()
    {
        if (playerPositionManager != null)
        {
            Destroy(gameObject);
            return;
        }
        nextPlayerPosition.Add(Vector3.negativeInfinity);
        nextPlayerPosition.Add(Vector3.negativeInfinity);
        player = GameObject.FindGameObjectWithTag("Player");
        SceneManager.activeSceneChanged += FindPlayerWhenSceneChanged;
        playerPositionManager = this;
        DontDestroyOnLoad(transform.gameObject);
    }

    public Vector3 getCurrentPlayerPosition(int index)
    {
        return nextPlayerPosition[index];
    }

    public void setPlayerPosition(Vector3 position, int index)
    {
        nextPlayerPosition[index] = position;
    }

    private void FindPlayerWhenSceneChanged(Scene current, Scene next)
    {
        player = GameObject.FindGameObjectWithTag("Player");
        if (player != null && !float.IsNegativeInfinity(nextPlayerPosition[SceneLoadWrapper.sceneLoadWrapper.currentSceneType].x))
        {
            player.transform.position = nextPlayerPosition[SceneLoadWrapper.sceneLoadWrapper.currentSceneType];
        }
    }
}
