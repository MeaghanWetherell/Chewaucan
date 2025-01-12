using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Snake : MonoBehaviour
{
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.GetComponentInChildren<DateRock>() != null)
        {
            Vector3 pos = transform.position;
            transform.position = new Vector3(pos.x + 3, pos.y, pos.z);
        }
    }
}
