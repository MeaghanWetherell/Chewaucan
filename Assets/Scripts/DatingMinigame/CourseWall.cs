using System;
using System.Collections;
using System.Collections.Generic;
using Misc;
using ScriptTags;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

//Actual walling functionality is deprecated, but will be activated if a trigger collider is on this object
public class CourseWall : MonoBehaviour
{
    //ref to corresponding manager
    private CourseManager manager;

    public GameObject tumbleweedPrefab;

    [Tooltip("Spawn positions for tumbleweeds. Should be along Z forward")]
    public List<Transform> spawnPositions;

    [Tooltip("Average time in seconds to spawn a tumbleweed")]
    public float averageTimeToSpawn;

    //whether this object has been initialized
    //don't remember why this was necessary but it works so I'm not touching it
    private bool inited = false;

    //store refs to all spawned tumbleweeds
    private List<GameObject> weeds = new List<GameObject>();

    private void Start()
    {
        manager = gameObject.GetComponentInParent<CourseManager>();
        manager.Stopped.AddListener(OnStop);
        
        if (!inited)
        {
            manager.Started.AddListener(OnStart);
            inited = true;
            transform.parent.gameObject.SetActive(false);
        }
    }

    //activate and begin spawning tumbleweeds when level starts
    private void OnStart()
    {
        transform.parent.gameObject.SetActive(true);
        StartCoroutine(spawnTumbleweeds());
    }

    //deactivate and destroy all spawned tumbleweeds
    private void OnStop()
    {
        StopAllCoroutines();
        transform.parent.gameObject.SetActive(false);
        foreach (GameObject weed in weeds)
        {
            if (weed != null)
            {
                Destroy(weed);
            }
        }
    }

    //spawn tumbleweeds at a random interval
    private IEnumerator spawnTumbleweeds()
    {
        while (true)
        {
            float randSecs = Random.Range(averageTimeToSpawn * 0.5f, averageTimeToSpawn * 1.5f);
            yield return new WaitForSeconds(randSecs);
            if(!PauseCallback.pauseManager.isPaused)
                SpawnTumbleweed();
        }
    }

    //spawn a tumbleweed at a random position
    private void SpawnTumbleweed()
    {
        int randPos = Random.Range(0, spawnPositions.Count);
        weeds.Add(Instantiate(tumbleweedPrefab, spawnPositions[randPos].position, spawnPositions[randPos].rotation));
    }

    //cause the player to lose when they touch a wall
    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Player>() != null)
        {
            manager.Reset("You went out of bounds!");
        }
    }
}
