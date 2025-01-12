using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Rock : MonoBehaviour
{
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.GetComponentInChildren<DateRock>() != null || other.gameObject.GetComponentInChildren<SnakeKill>())
        {
            SetPos(other.transform);
        }
    }

    private void SetPos(Transform other)
    {
        int dirX = Random.Range(1,2);
        if (dirX == 1) dirX = -1;
        else dirX = 1;
        int dirZ = Random.Range(-1, 1);
        Vector3 pos = other.position;
        Vector3 scale = transform.localScale;
        other.position = new Vector3(pos.x+dirX*scale.x+1, pos.y, pos.z+dirZ*scale.z+1);
    }
}
