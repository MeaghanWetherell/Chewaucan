using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialProperties : MonoBehaviour
{
    private float curRotationAmount;

    
    // Start is called before the first frame update
    void Start()
    {
        curRotationAmount = 0f;
    }

    public float getCurRotationAmount() { return  curRotationAmount; }

    public void changeCurRotationAmount(float n)
    {
        curRotationAmount += n;
    }
}
