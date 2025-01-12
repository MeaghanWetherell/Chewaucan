using System;
using System.Collections;
using System.Collections.Generic;
using Misc;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class SnakeMove : MonoBehaviour
{
    public NavMeshAgent myAgent;
    
    public float range = 10.0f;

    public float averageTimeBetweenMoveChange;
    
    private float speed;

    private void Awake()
    {
        speed = myAgent.speed;
        Vector3 point;
        RandomPoint(transform.position, range, out point);
        myAgent.SetDestination(point);
        StartCoroutine(SelectNewDirection());
    }

    private void OnEnable()
    {
        PauseCallback.pauseManager.SubscribeToPause(OnPause);
        PauseCallback.pauseManager.SubscribeToResume(OnResume);
    }

    private void OnDisable()
    {
        PauseCallback.pauseManager.UnsubToPause(OnPause);
        PauseCallback.pauseManager.UnsubToResume(OnResume);
    }

    bool RandomPoint(Vector3 center, float range, out Vector3 result)
    {
        for (int i = 0; i < 30; i++)
        {
            Vector3 randomPoint = center + Random.insideUnitSphere * range;
            NavMeshHit hit;
            if (NavMesh.SamplePosition(randomPoint, out hit, 5.0f, NavMesh.AllAreas))
            {
                result = hit.position;
                return true;
            }
        }
        result = Vector3.zero;
        return false;
    }

    private IEnumerator SelectNewDirection()
    {
        float minTime = averageTimeBetweenMoveChange / 2;
        float maxTime = averageTimeBetweenMoveChange + minTime;
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(minTime, maxTime));
            Vector3 point;
            RandomPoint(transform.position, range, out point);
            myAgent.SetDestination(point);
        }
    }

    
    private void OnPause()
    {
        myAgent.speed = 0;
    }

    private void OnResume()
    {
        myAgent.speed = speed;
    }
}
