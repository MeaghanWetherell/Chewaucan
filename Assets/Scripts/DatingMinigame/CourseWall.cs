using System;
using System.Collections;
using System.Collections.Generic;
using Misc;
using ScriptTags;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

public class CourseWall : MonoBehaviour
{
    private CourseManager manager;

    public GameObject tumbleweedPrefab;

    [Tooltip("Spawn positions should be along Z forward")]
    public List<Transform> spawnPositions;

    [Tooltip("Average time in seconds to spawn a tumbleweed")]
    public float averageTimeToSpawn;

    private bool inited = false;

    private List<GameObject> weeds = new List<GameObject>();

    private void Start()
    {
        manager = gameObject.GetComponentInParent<CourseManager>();
        
        if (!inited)
        {
            manager.Started.AddListener(OnStart);
            manager.Stopped.AddListener(OnStop);
            inited = true;
            transform.parent.gameObject.SetActive(false);
        }
    }

    private void OnStart()
    {
        transform.parent.gameObject.SetActive(true);
        StartCoroutine(spawnTumbleweeds());
    }

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

    private void SpawnTumbleweed()
    {
        int randPos = Random.Range(0, spawnPositions.Count);
        weeds.Add(Instantiate(tumbleweedPrefab, spawnPositions[randPos].position, spawnPositions[randPos].rotation));
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Player>() != null)
        {
            manager.Reset("You went out of bounds!");
        }
    }
}
