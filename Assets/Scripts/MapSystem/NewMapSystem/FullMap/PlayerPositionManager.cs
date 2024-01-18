using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.SceneManagement;

public class PlayerPositionManager : MonoBehaviour
{
    public static PlayerPositionManager playerPositionManager;

    private Vector3 currentPlayerPosition;
    private GameObject player;
    
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        SceneManager.activeSceneChanged += FindPlayerWhenSceneChanged;
        playerPositionManager = this;
        DontDestroyOnLoad(transform.gameObject);
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (player != null)
        {
            if (currentPlayerPosition != player.transform.position)
            {
                currentPlayerPosition = player.transform.position;
                //Debug.Log("Current Position: " + currentPlayerPosition);
            }
        }
    }

    public Vector3 getCurrentPlayerPosition()
    {
        return currentPlayerPosition;
    }

    public void setPlayerPosition(Vector3 position)
    {
        currentPlayerPosition = position;
    }

    private void FindPlayerWhenSceneChanged(Scene current, Scene next)
    {
        player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            if (player.transform.position != currentPlayerPosition)
            {
                player.transform.position = currentPlayerPosition;
            }
        }
    }
}
