using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spin : MonoBehaviour
{
    public Vector3 initVel;
    
    public Vector3 accel;

    public float maxVel;

    private Vector3 vel;
    
    private void OnEnable()
    {
        vel = initVel;
    }

    private void Update()
    {
        transform.Rotate(vel.x*Time.deltaTime, vel.y*Time.deltaTime, vel.z*Time.deltaTime);
        vel.x = Mathf.Max(vel.x+accel.x*Time.deltaTime, maxVel);
        vel.y = Mathf.Max(vel.y+accel.y*Time.deltaTime, maxVel);
        vel.z = Mathf.Max(vel.z+accel.z*Time.deltaTime, maxVel);;
    }
}
