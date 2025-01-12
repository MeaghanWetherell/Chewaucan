using System;
using System.Collections;
using System.Collections.Generic;
using LoadGUIFolder;
using ScriptTags;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class CourseManager : MonoBehaviour
{
    public static UnityEvent<int> win = new UnityEvent<int>();
    
    public int levelID;
    
    public CourseTimer timer;
    
    public Canvas courseUI;

    public Slider progBar;

    public TextMeshProUGUI progText;
    
    [Tooltip("Position to which the player will be returned after the game")]public Transform startPosition;

    [Tooltip("Amount of 'points' to win. Points per rock is adjustable on their prefabs")]public float datingPointsToWin;

    [Tooltip("Amount of time the player has to complete the dating")]public float courseTime;

    [Tooltip("Number of tuffa rocks to spawn")]public int tuffaToSpawn;

    [Tooltip("Number of carbonate rocks to spawn")]public int carbonateToSpawn;
    
    [Tooltip("Number of rocks to spawn")]public int rocksToSpawn;
    
    [Tooltip("Number of stationary snakes to spawn")]public int stationarySnakeCount;
    
    [Tooltip("Number of moving snakes to spawn")]public int movingSnakeCount;

    [Tooltip("Reference to an empty gameobject whose children are all possible spawn locations for tuffa/carbonate")]
    public Transform dateRockSpawnLocations;

    [Tooltip("Reference to an empty gameobject whose children are all possible spawn locations for rocks")]
    public Transform rockSpawnLocations;
    
    [Tooltip("Reference to an empty gameobject whose children are all possible spawn locations for snakes")]
    public Transform snakeSpawnLocations;

    public GameObject tuffaPrefab;

    public GameObject carboPrefab;

    public GameObject rockPrefab;

    public GameObject stationarySnakePrefab;

    public GameObject movingSnakePrefab;

    private List<GameObject> spawnedObjects = new List<GameObject>();
    
    [NonSerialized]public float curPoints;

    [NonSerialized]public bool active;
    
    [NonSerialized]public UnityEvent Started = new UnityEvent();

    [NonSerialized]public UnityEvent Stopped = new UnityEvent();
    
    public void StartCourse()
    {
        courseUI.gameObject.SetActive(true);
        timer.SetTimer(courseTime);
        timer.timerStopped.AddListener(Reset);
        active = true;
        List<Transform> rocks = SpawnItems(dateRockSpawnLocations, tuffaToSpawn, tuffaPrefab);
        foreach (Transform rock in rocks)
        {
            rock.GetComponentInChildren<DateRock>().manager = this;
        }
        rocks = SpawnItems(dateRockSpawnLocations, carbonateToSpawn, carboPrefab, rocks);
        foreach (Transform rock in rocks)
        {
            rock.GetComponentInChildren<DateRock>().manager = this;
        }
        SpawnItems(rockSpawnLocations, rocksToSpawn, rockPrefab);
        List<Transform> snakes = SpawnItems(snakeSpawnLocations, stationarySnakeCount, stationarySnakePrefab);
        foreach (Transform snake in snakes)
        {
            snake.GetComponentInChildren<SnakeKill>().manager = this;
        }
        snakes = SpawnItems(snakeSpawnLocations, movingSnakeCount, movingSnakePrefab, snakes);
        foreach (Transform snake in snakes)
        {
            snake.GetComponentInChildren<SnakeKill>().manager = this;
        }
        Started.Invoke();
    }

    public List<Transform> SpawnItems(Transform spawnLocs, int numToSpawn, GameObject spawnPrefab)
    {
        int rand;
        List<Transform> childList = new List<Transform>();
        foreach(Transform child in spawnLocs) childList.Add(child);
        List<Transform> ret = new List<Transform>();
        while (numToSpawn > 0 && childList.Count > 0)
        {
            rand = Random.Range(0, childList.Count);
            GameObject obj = Instantiate(spawnPrefab, childList[rand].position, Quaternion.identity);
            ret.Add(obj.transform);
            spawnedObjects.Add(obj);
            childList.RemoveAt(rand);
            numToSpawn--;
        }

        return ret;
    }
    
    public List<Transform> SpawnItems(Transform spawnLocs, int numToSpawn, GameObject spawnPrefab, List<Transform> blackListedLocations)
    {
        int rand;
        List<Transform> childList = new List<Transform>();
        foreach (Transform child in spawnLocs)
        {
            bool add = true;
            foreach (Transform bad in blackListedLocations)
            {
                if (child.position.Equals(bad.position))
                {
                    add = false;
                    break;
                }
            }
            if(add) childList.Add(child);
        }
        List<Transform> ret = new List<Transform>();
        while (numToSpawn > 0 && childList.Count > 0)
        {
            rand = Random.Range(0, childList.Count);
            GameObject obj = Instantiate(spawnPrefab, childList[rand].position, Quaternion.identity);
            ret.Add(obj.transform);
            spawnedObjects.Add(obj);
            childList.RemoveAt(rand);
            numToSpawn--;
        }

        return ret;
    }

    public void Reset(string loseMessage)
    {
        if (Math.Abs(curPoints - datingPointsToWin) < 0.01f)
        {
            LoadGUIManager.loadGUIManager.InstantiatePopUp("You Win!","You got the right date!");
        }
        else
        {
            LoadGUIManager.loadGUIManager.InstantiatePopUp("You Lose!",loseMessage);
        }
        foreach (GameObject obj in spawnedObjects)
        {
            if(obj != null)
                Destroy(obj);
        }
        Player.player.GetComponent<CharacterController>().enabled = false;
        Player.player.transform.position = startPosition.position;
        Player.player.transform.eulerAngles = startPosition.eulerAngles;
        Player.player.GetComponent<CharacterController>().enabled = true;
        curPoints = 0;
        AddPoints(0);
        active = false;
        timer.StopTimer();
        courseUI.gameObject.SetActive(false);
        Stopped.Invoke();
    }

    private void Reset(bool val)
    {
        if(val)
            Reset("You didn't get the right date in time!");
    }

    public void AddPoints(float points)
    {
        curPoints += points;
        if (curPoints < 0) curPoints = 0;
        if (curPoints >= datingPointsToWin)
        {
            curPoints = datingPointsToWin;
            win.Invoke(levelID);
            Reset("");
        }
        progBar.value = curPoints / datingPointsToWin;
        progText.text = (curPoints / datingPointsToWin) * 100 + "%";
    }
}
