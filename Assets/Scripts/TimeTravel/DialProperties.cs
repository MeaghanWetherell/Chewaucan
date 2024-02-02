using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialProperties : MonoBehaviour
{

    //the rotation values can be weird in the editor itself, so this keeps track
    // of how far the hand is rotated instead
    private float curRotationAmount;
    private int progress;
    
    // Start is called before the first frame update
    void Start()
    {
        curRotationAmount = 0f;
        progress = 0;
    }

    public float getCurRotationAmount() { return  curRotationAmount; }

    public void changeCurRotationAmount(float n)
    {
        curRotationAmount += n;
    }
}
