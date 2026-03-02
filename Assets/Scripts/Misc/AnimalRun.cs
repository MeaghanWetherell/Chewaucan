using System;
using System.Collections;
using System.Collections.Generic;
using Misc;
using ScriptTags;
using UnityEngine;
using UnityEngine.AI;

public class AnimalRun : MonoBehaviour
{
    public NavMeshAgent myAgent;
    
    [Tooltip("Length of time running before the animal fades out")]
    public float runtime;

    [Tooltip("Time to fade out after runtime expires")]
    public float cullTime;

    public MeshRenderer animalRenderer;

    public Collider trigger;
    
    [Tooltip("Time it takes for an animal to respawn after running")]public float TimeToRespawn;

    public Material transMat;

    public Material opaqueMat;

    private float oldSpeed;
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Player>() != null)
        {
            Run();
            trigger.enabled = false;
        }
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

    private void OnPause()
    {
        oldSpeed = myAgent.speed;
        myAgent.speed = 0;
    }

    private void OnResume()
    {
        myAgent.speed = oldSpeed;
    }
    
    protected virtual void Run()
    {
        Vector3 point;
        if (GetTargetPoint(out point, 80f))
        {
            myAgent.SetDestination(point);
            StartCoroutine(FadeOutAfterTime(runtime, cullTime));
        }
        else
        {
            StartCoroutine(FadeOutAfterTime(0, cullTime / 5));
        }
    }

    private IEnumerator FadeOutAfterTime(float time, float ct)
    {
        Vector3 pos = transform.parent.position;
        while (time > 0)
        {
            if (!PauseCallback.pauseManager.isPaused)
                time -= 0.05f;
            yield return new WaitForSeconds(0.05f);
        }

        animalRenderer.material = transMat;
        float curCullTime = 0;
        while (curCullTime < ct)
        {
            curCullTime += 0.05f;
            float val = Mathf.Lerp(1, 0, curCullTime / ct);
            //Debug.Log(val);
            Color temp = animalRenderer.material.color;
            animalRenderer.material.color = new Color(temp.r, temp.g, temp.b, val);
            yield return new WaitForSeconds(0.05f);
        }

        animalRenderer.material = opaqueMat;
        myAgent.ResetPath();
        transform.parent.gameObject.SetActive(false);
        transform.parent.position = pos;
        yield return new WaitForSeconds(TimeToRespawn);
        Respawn();
    }

    private void Respawn()
    {
        trigger.enabled = true;
        transform.parent.gameObject.SetActive(true);
    }

    private bool GetTargetPoint(out Vector3 point, float dist)
    {
        point = Vector3.zero;
        Vector3 awayVector = (transform.position - Player.player.transform.position).normalized*dist;
        NavMeshHit hit;
        if (NavMesh.SamplePosition(transform.position+awayVector, out hit, 30.0f, NavMesh.AllAreas))
        {
            point = hit.position;
            return true;
        }
        return false;
    }
}
