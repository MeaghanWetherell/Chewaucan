using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomAudioTrigger : MonoBehaviour
{
    public AudioSource target;

    public float intervalMin, intervalMax;

    private void Start()
    {
        StartCoroutine(PlayAtRandomIntervals());
    }

    private IEnumerator PlayAtRandomIntervals()
    {
        while (true)
        {
            float waitTime = Random.Range(intervalMin, intervalMax);
            yield return new WaitForSeconds(waitTime);
            target.Play();
        }
    }
}
