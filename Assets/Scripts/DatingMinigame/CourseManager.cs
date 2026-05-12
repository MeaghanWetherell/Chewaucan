using System;
using System.Collections;
using System.Collections.Generic;
using Audio;
using LoadGUIFolder;
using ScriptTags;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class CourseManager : MonoBehaviour
{
    //runs when the player wins the course, passing the level won
    public static UnityEvent<int> win = new UnityEvent<int>();

    //static ref set automatically to the data for each lake level
    private static LakeLevelData data;

    [Tooltip("Should match the index (0 is first index) of this course's data in the LakeLevelData object")]
    public int levelID;

    [Tooltip("Ref to the timer on this manager's canvas")]
    public CourseTimer timer;

    [Tooltip("Ref to thsi manager's canvas")]
    public Canvas courseUI;

    [Tooltip("Slider that tracks the player's progress toward getting the correct date")]
    public Slider progBar;

    [Tooltip("Text that displays the player's % progress toward the correct date")]
    public TextMeshProUGUI progText;

    [Tooltip("Position to which the player will be returned after the game")]
    public Transform startPosition;

    [Tooltip("Amount of 'points' to win. Points per rock is adjustable on their prefabs")]
    public float datingPointsToWin;

    [Tooltip("Amount of time the player has to complete the dating")]
    public float courseTime;

    [Tooltip("Number of tuffa rocks to spawn")]
    public int tuffaToSpawn;

    [Tooltip("Number of carbonate rocks to spawn")]
    public int carbonateToSpawn;

    [Tooltip("Number of rocks to spawn")] public int rocksToSpawn;

    [Tooltip("Number of date rocks to spawn at incorrect height")]
    public int badDateRocksToSpawn;

    [Tooltip("Number of stationary snakes to spawn")]
    public int stationarySnakeCount;

    [Tooltip("Number of moving snakes to spawn")]
    public int movingSnakeCount;

    [Tooltip("Reference to an empty gameobject whose children are all possible spawn locations for tuffa/carbonate")]
    public Transform dateRockSpawnLocations;

    [Tooltip(
        "Reference to an empty gameobject whose children are all possible spawn locations at incorrect height for tuffa/carbonate")]
    public Transform badDateRockSpawnLocations;

    [Tooltip("Reference to an empty gameobject whose children are all possible spawn locations for rocks")]
    public Transform rockSpawnLocations;

    [Tooltip("Reference to an empty gameobject whose children are all possible spawn locations for snakes")]
    public Transform snakeSpawnLocations;

    [Tooltip("List of all tuffa prefabs")] public List<GameObject> tuffaPrefab;

    [Tooltip("List of all carbonate prefabs")]
    public List<GameObject> carboPrefab;

    [Tooltip("List of all rock prefabs")] public List<GameObject> rockPrefab;

    [Tooltip("Ref to the stationary snake prefab(s)")]
    public List<GameObject> stationarySnakePrefab;

    [Tooltip("Ref to the moving snake prefab(s)")]
    public List<GameObject> movingSnakePrefab;

    //stores the max random date for tuffa at this level
    private int dateMax;

    //stores the min random date for tuffa at this level
    private int dateMin;

    //min world y for rocks to be correct for this level
    [NonSerialized] public float yMin;

    //max world y for rocks to be correct for this level
    [NonSerialized] public float yMax;


    [Tooltip("Valid rich text color tag like 'white' or valid hex string like #ffffff")]
    public string goodRockTextColor;

    [Tooltip("Valid rich text color tag like 'white' or valid hex string like #ffffff")]
    public string badRockTextColor;

    //stores all the objects this manager creates for later cleanup
    private List<GameObject> spawnedObjects = new List<GameObject>();

    //the player's current date 'points'
    [NonSerialized] public float curPoints;

    //whether this manager is currently active
    [NonSerialized] public bool active;

    //called when this manager starts its level
    [NonSerialized] public UnityEvent Started = new UnityEvent();

    //called when this manager stops its level
    [NonSerialized] public UnityEvent Stopped = new UnityEvent();

    [Tooltip("Background music to play during the game")]
    public List<AudioClip> bgm;



    private void Start()
    {
        //if the level data hasn't been loaded already, load it
        if (data == null)
        {
            data = Resources.Load<LakeLevelData>("PlateauData");
        }

        //get data from the level data object
        dateMin = data.dateMin[levelID];
        dateMax = data.dateMax[levelID];
        yMin = data.yMin[levelID];
        yMax = data.yMax[levelID];
        //set reference objects inactive
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

    //set up the course ui, start the BGM, and create all the randomized rocks and snakes
    public void StartCourse()
    {
        courseUI.gameObject.SetActive(true);
        timer.SetTimer(courseTime);
        timer.timerStopped.AddListener(Reset);
        active = true;
        SoundManager.soundManager.SetBGM(bgm);
        Player.player.GetComponentInChildren<RandomAmbientSound>().enabled = false;
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
        foreach (GameObject tuffa in tuffaPrefab)
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

        if (rockSpawnLocations != null)
            SpawnItems(rockSpawnLocations, rocksToSpawn, rockPrefab);
        List<Transform> snakes = SpawnItems(snakeSpawnLocations, stationarySnakeCount, stationarySnakePrefab);
        snakes = SpawnItems(snakeSpawnLocations, movingSnakeCount, movingSnakePrefab, snakes);
        LoadGUIManager.loadGUIManager.Load("DatingHelpMenu");
        Started.Invoke();
    }

    //sets a rock to be a good or bad random date based on its position 
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
            dr.date = Random.Range(20000, 50000).ToString();
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

                dr.date = Random.Range(20000, 50000).ToString();
                return;
            }
        }

        dr.date = Random.Range(20000, 50000).ToString();

    }

    //returns whether targ falls within the range of min to max
    private bool inRange(float targ, float min, float max)
    {
        return targ >= min && targ <= max;
    }

    //spawns prefabs from the passed list at random locations from amongst the children of the spawnlocs transform. adds ymod to the spawn y.
    public List<Transform> SpawnItems(Transform spawnLocs, int numToSpawn, List<GameObject> spawnPrefabs,
        float ymod = 0)
    {
        int rand;
        List<Transform> childList = new List<Transform>();
        foreach (Transform child in spawnLocs) childList.Add(child);
        List<Transform> ret = new List<Transform>();
        while (numToSpawn > 0 && childList.Count > 0)
        {
            rand = Random.Range(0, spawnPrefabs.Count);
            GameObject spawnPrefab = spawnPrefabs[rand];
            rand = Random.Range(0, childList.Count);
            Vector3 targetPos = spawnLocs.localToWorldMatrix * childList[rand].position;
            GameObject obj = Instantiate(spawnPrefab,  targetPos+ new Vector3(0, ymod, 0),
                Quaternion.identity);
            ret.Add(obj.transform);
            spawnedObjects.Add(obj);
            childList.RemoveAt(rand);
            numToSpawn--;
        }

        return ret;
    }

    //spawns prefabs from the passed list at random locations from amongst the children of the spawnlocs transform, excluding positions on the blacklist. adds ymod to the spawn y
    public List<Transform> SpawnItems(Transform spawnLocs, int numToSpawn, List<GameObject> spawnPrefabs,
        List<Transform> blackListedLocations, float ymod = 0)
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

            if (add) childList.Add(child);
        }

        List<Transform> ret = new List<Transform>();
        while (numToSpawn > 0 && childList.Count > 0)
        {
            rand = Random.Range(0, spawnPrefabs.Count);
            GameObject spawnPrefab = spawnPrefabs[rand];
            rand = Random.Range(0, childList.Count);
            GameObject obj = Instantiate(spawnPrefab, childList[rand].position + new Vector3(0, ymod, 0),
                Quaternion.identity);
            ret.Add(obj.transform);
            spawnedObjects.Add(obj);
            childList.RemoveAt(rand);
            numToSpawn--;
        }

        return ret;
    }

    //ends the game with a loss or win depending on current points, sending a popup with the passed loss message in the case of a loss
    //destroys all spawned objects, stops background music, and resets the player's position
    public void Reset(string loseMessage)
    {
        if (Math.Abs(curPoints - datingPointsToWin) < 0.01f)
        {
           // LoadGUIManager.loadGUIManager.InstantiatePopUp("You Win!", "You got the right date!");
        }
        else
        {
            LoadGUIManager.loadGUIManager.InstantiatePopUp("You Lose!", loseMessage);
        }

        Stopped.Invoke();
        foreach (GameObject obj in spawnedObjects)
        {
            if (obj != null)
                Destroy(obj);
        }
        
        Player.player.transform.position = startPosition.position;
        Player.player.transform.eulerAngles = startPosition.eulerAngles;
        Player.player.GetComponentInChildren<RandomAmbientSound>().enabled = true;
        SoundManager.soundManager.StopBGM();
        curPoints = 0;
        AddPoints(0);
        active = false;
        timer.StopTimer();
        courseUI.gameObject.SetActive(false);
    }

    public void Reset()
    {
        Reset("");
    }

    //overload of reset to be called by the course timer. timer calls with val true when the time is up, false otherwise
    private void Reset(bool val)
    {
        if(val)
            Reset("You didn't get the right date in time!");
    }

    //add dating points and update UIs
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
