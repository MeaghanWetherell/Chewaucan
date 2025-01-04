using System;
using System.Collections;
using System.Collections.Generic;
using ScriptTags;
using UnityEngine;
using Random = UnityEngine.Random;

public class DateRock : MonoBehaviour
{
    public float myPoints;

    public AudioSource mySE;

    public CourseManager manager;

    private void Awake()
    {
        float scaleX = Random.Range(0.5f, 2);
        float scaleY = Random.Range(0.5f, 2);
        float scaleZ = Random.Range(0.5f, 2);
        Vector3 scale = transform.localScale;
        transform.localScale = new Vector3(scale.x * scaleX, scale.y * scaleY, scale.z * scaleZ);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Player>() != null)
        {
            manager.AddPoints(myPoints);
            mySE.Play();
            StartCoroutine(DestroyAfterTime(1));
        }
    }

    private IEnumerator DestroyAfterTime(float secs)
    {
        yield return new WaitForSeconds(secs);
        Destroy(gameObject);
    }
}
