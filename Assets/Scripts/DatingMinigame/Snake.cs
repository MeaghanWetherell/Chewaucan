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
            if (other.collider.bounds.Contains(transform.position))
            {
                Vector3 pos = transform.position;
                transform.position = new Vector3(pos.x + 3, pos.y, pos.z);
            }
            else
            {
                SnakeMove movement = gameObject.GetComponent<SnakeMove>();
                if (movement != null)
                {
                    movement.SetNewDirection();
                }
            }
            
        }
    }
}
