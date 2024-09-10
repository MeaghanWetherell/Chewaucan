using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class v3Wrapper
{
    public float xVal { get; set; }

    public float yVal { get; set;  }

    public float zVal { get; set;  }
    public v3Wrapper(Vector3 v3)
    {
        xVal = v3.x;
        yVal = v3.y;
        zVal = v3.z;
    }

    public v3Wrapper(float xVali, float yVali, float zVali)
    {
        xVal = xVali;
        yVal = yVali;
        zVal = zVali;
    }
    
    public  v3Wrapper(){}

    public Vector3 getVector()
    {
        return new Vector3(xVal, yVal, zVal);
    }
}
