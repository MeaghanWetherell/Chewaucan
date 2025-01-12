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

    public float scaleMinX;

    public float scaleMaxX;

    public float scaleMinY;

    public float scaleMaxY;

    public float scaleMinZ;

    public float scaleMaxZ;

    private void Awake()
    {
        float scaleX = Random.Range(scaleMinX, scaleMaxX);
        float scaleY = Random.Range(scaleMinY, scaleMaxY);
        float scaleZ = Random.Range(scaleMinZ, scaleMaxZ);
        Vector3 scale = transform.parent.localScale;
        transform.parent.localScale = new Vector3(scale.x * scaleX, scale.y * scaleY, scale.z * scaleZ);

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Player>() != null)
        {
            manager.AddPoints(myPoints);
            mySE.Play();
            StartCoroutine(DestroyAfterTime(0.5f));
            GetComponent<BoxCollider>().enabled = false;
        }
    }

    private IEnumerator DestroyAfterTime(float secs)
    {
        yield return new WaitForSeconds(secs);
        Destroy(transform.parent.gameObject);
    }
}
