using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IControllerCollisionEnter
{
    //allow oncollision enter to trigger with land movement
    public void OnControllerCollisionEnter(GameObject collision);
}
