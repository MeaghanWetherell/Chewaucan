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
    
    [Tooltip("Max range to wander in one movement")]
    public float range = 10.0f;

    [Tooltip("Time, on average, between the snake chaning directions")]
    public float averageTimeBetweenMoveChange;

    [Tooltip("Snake animator")]
    public Animator anim;
    
    //stores the agent's original speed
    private float speed;

    //stores the agent's last target
    private Vector3 oldTarget;

    private void OnEnable()
    {
        speed = myAgent.speed;
        SetNewDirection();
        StartCoroutine(SelectNewDirection());
        PauseCallback.pauseManager.SubscribeToPause(OnPause);
        PauseCallback.pauseManager.SubscribeToResume(OnResume);
    }

    private void OnDisable()
    {
        StopAllCoroutines();
        if(myAgent.isActiveAndEnabled)
            myAgent.SetDestination(transform.position);
        anim.SetBool("Moving", false);
        PauseCallback.pauseManager.UnsubToPause(OnPause);
        PauseCallback.pauseManager.UnsubToResume(OnResume);
    }

    private void Update()
    {
        if (myAgent.remainingDistance <= 0.1f)
        {
            anim.SetBool("Moving", false);
        }
    }

    //attempts to find a valid point on the nav mesh in range from the center, returns false if it fails to find one
    bool RandomPoint(Vector3 center, float rangeFromCenter, out Vector3 result)
    {
        for (int i = 0; i < 100; i++)
        {
            Vector3 randomPoint = center + Random.insideUnitSphere * rangeFromCenter;
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

    //at a random interval, changes move direction
    private IEnumerator SelectNewDirection()
    {
        float minTime = averageTimeBetweenMoveChange / 2;
        float maxTime = averageTimeBetweenMoveChange + minTime;
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(minTime, maxTime));
            if (!PauseCallback.pauseManager.isPaused)
            {
                SetNewDirection();
            }
        }
    }

    //sets the snake's next destination to a valid point within range
    public void SetNewDirection()
    {
        anim.SetBool("Moving", true);
        Vector3 point;
        RandomPoint(transform.position, range, out point);
        myAgent.SetDestination(point);
    }

    
    private void OnPause()
    {
        myAgent.speed = 0;
        myAgent.GetComponent<Rigidbody>().velocity = Vector3.zero;
        myAgent.SetDestination(transform.position);
    }

    private void OnResume()
    {
        myAgent.speed = speed;
        myAgent.SetDestination(oldTarget);
    }
}
