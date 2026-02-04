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

    private static LakeLevelData data;
    
    [Tooltip("Should match the index (0 is first index) of this course's data in the LakeLevelData object")]
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
    
    [Tooltip("Number of date rocks to spawn at incorrect height")]public int badDateRocksToSpawn;
    
    [Tooltip("Number of stationary snakes to spawn")]public int stationarySnakeCount;
    
    [Tooltip("Number of moving snakes to spawn")]public int movingSnakeCount;

    [Tooltip("Reference to an empty gameobject whose children are all possible spawn locations for tuffa/carbonate")]
    public Transform dateRockSpawnLocations;
    
    [Tooltip("Reference to an empty gameobject whose children are all possible spawn locations at incorrect height for tuffa/carbonate")]
    public Transform badDateRockSpawnLocations;

    [Tooltip("Reference to an empty gameobject whose children are all possible spawn locations for rocks")]
    public Transform rockSpawnLocations;
    
    [Tooltip("Reference to an empty gameobject whose children are all possible spawn locations for snakes")]
    public Transform snakeSpawnLocations;

    public List<GameObject> tuffaPrefab;

    public List<GameObject> carboPrefab;

    public List<GameObject> rockPrefab;

    public List<GameObject> stationarySnakePrefab;

    public List<GameObject> movingSnakePrefab;

    private int dateMax;

    private int dateMin;

    public float yMin;

    public float yMax;
    

    [Tooltip("Valid rich text color tag like 'white' or valid hex string like #ffffff")]public string goodRockTextColor;

    [Tooltip("Valid rich text color tag like 'white' or valid hex string like #ffffff")]public string badRockTextColor;

    private List<GameObject> spawnedObjects = new List<GameObject>();
    
    [NonSerialized]public float curPoints;

    [NonSerialized]public bool active;
    
    [NonSerialized]public UnityEvent Started = new UnityEvent();

    [NonSerialized]public UnityEvent Stopped = new UnityEvent();

    

    private void Start()
    {
        if (data == null)
        {
            data = Resources.Load<LakeLevelData>("PlateauData");
        }
        dateMin = data.dateMin[levelID];
        dateMax = data.dateMax[levelID];
        yMin = data.yMin[levelID];
        yMax = data.yMax[levelID];
        foreach (Transform child in dateRockSpawnLocations)
        {
            child.gameObject.SetActive(false);
        }
        foreach (Transform child in badDateRockSpawnLocations)
        {
            child.gameObject.SetActive(false);
        }

        if (rockSpawnLocations != null)
        {
            foreach (Transform child in rockSpawnLocations)
            {
                child.gameObject.SetActive(false);
            }
        }
        foreach (Transform child in snakeSpawnLocations)
        {
            child.gameObject.SetActive(false);
        }
    }

    public void StartCourse()
    {
        courseUI.gameObject.SetActive(true);
        timer.SetTimer(courseTime);
        timer.timerStopped.AddListener(Reset);
        active = true;
        GameObject[] allDrs = GameObject.FindGameObjectsWithTag("dateRock");
        foreach (GameObject rockG in allDrs)
        {
            DateRock dr = rockG.GetComponentInChildren<DateRock>();
            dr.manager = this;
            Transform rock = dr.transform;
            setDateAndColorByPosition(rock.position.y, dr);
        }
        List<Transform> rocks = SpawnItems(dateRockSpawnLocations, tuffaToSpawn, tuffaPrefab);
        foreach (Transform rock in rocks)
        {
            DateRock dr = rock.GetComponentInChildren<DateRock>();
            dr.manager = this;
            dr.date = Random.Range(dateMin, dateMax).ToString();
            dr.dateTextColor = goodRockTextColor;
        }
        rocks = SpawnItems(dateRockSpawnLocations, carbonateToSpawn, carboPrefab, rocks);
        foreach (Transform rock in rocks)
        {
            DateRock dr = rock.GetComponentInChildren<DateRock>();
            dr.manager = this;
            dr.date = Random.Range(20000, 50000).ToString();
            dr.dateTextColor = badRockTextColor;
        }

        List<GameObject> tuffaAndCarbo = new List<GameObject>();
        foreach(GameObject tuffa in tuffaPrefab)
            tuffaAndCarbo.Add(tuffa);
        foreach (GameObject carbo in carboPrefab)
        {
            tuffaAndCarbo.Add(carbo);
        }

        if (badDateRockSpawnLocations != null)
        {
            rocks = SpawnItems(badDateRockSpawnLocations, badDateRocksToSpawn, tuffaAndCarbo);
            foreach (Transform rock in rocks)
            {
                DateRock dr = rock.GetComponentInChildren<DateRock>();
                dr.manager = this;
                setDateAndColorByPosition(dr.transform.position.y, dr);
            }
        }
        if(rockSpawnLocations != null)
            SpawnItems(rockSpawnLocations, rocksToSpawn, rockPrefab);
        List<Transform> snakes = SpawnItems(snakeSpawnLocations, stationarySnakeCount, stationarySnakePrefab);
        snakes = SpawnItems(snakeSpawnLocations, movingSnakeCount, movingSnakePrefab, snakes);
        Started.Invoke();
    }

    private void setDateAndColorByPosition(float yPos, DateRock dr)
    {
        if (dr.overrideDateMin > 0 && dr.overrideDateMax > 0)
        {
            dr.date = Random.Range(dr.overrideDateMin, dr.overrideDateMax).ToString();
        }
        if (inRange(yPos, yMin, yMax))
        {
            if (dr.myPoints > 0)
            {
                dr.dateTextColor = goodRockTextColor;
                dr.date = Random.Range(dateMin, dateMax).ToString();
                return;
            }
            dr.dateTextColor = badRockTextColor;
            dr.date = Random.Range(20000,50000).ToString();
            return;
        }
        dr.dateTextColor = badRockTextColor;
        for (int i = 0; i < data.yMin.Count; i++)
        {
            if (inRange(yPos, data.yMin[i], data.yMax[i]))
            {
                if (dr.myPoints > 0)
                {
                    dr.date = Random.Range(data.dateMin[i], data.dateMax[i]).ToString();
                    return;
                }
                dr.date = Random.Range(20000,50000).ToString();
                return;
            }
        }
        dr.date = Random.Range(20000,50000).ToString();
        
    }

    private bool inRange(float targ, float min, float max)
    {
        return targ >= min && targ <= max;
    }

    public List<Transform> SpawnItems(Transform spawnLocs, int numToSpawn, List<GameObject> spawnPrefabs, float ymod=0)
    {
        int rand;
        List<Transform> childList = new List<Transform>();
        foreach(Transform child in spawnLocs) childList.Add(child);
        List<Transform> ret = new List<Transform>();
        while (numToSpawn > 0 && childList.Count > 0)
        {
            rand = Random.Range(0, spawnPrefabs.Count);
            GameObject spawnPrefab = spawnPrefabs[rand];
            rand = Random.Range(0, childList.Count);
            GameObject obj = Instantiate(spawnPrefab, childList[rand].position+new Vector3(0,ymod,0), Quaternion.identity);
            ret.Add(obj.transform);
            spawnedObjects.Add(obj);
            childList.RemoveAt(rand);
            numToSpawn--;
        }
        return ret;
    }
    
    public List<Transform> SpawnItems(Transform spawnLocs, int numToSpawn, List<GameObject> spawnPrefabs, List<Transform> blackListedLocations, float ymod=0)
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
            rand = Random.Range(0, spawnPrefabs.Count);
            GameObject spawnPrefab = spawnPrefabs[rand];
            rand = Random.Range(0, childList.Count);
            GameObject obj = Instantiate(spawnPrefab, childList[rand].position+new Vector3(0,ymod,0), Quaternion.identity);
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
        Stopped.Invoke();
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
