using System.Collections;
using System.Collections.Generic;
using Misc;
using UnityEngine;

public class PlayerPositionArrow : MonoBehaviour
{
    [Tooltip("1 for pleistocene, 0 for modern")]
    public int myType;
    
    void OnEnable()
    {
        if (myType == SceneLoadWrapper.sceneLoadWrapper.currentSceneType)
        {
            Vector3 cur = PlayerPositionManager.playerPositionManager.getCurrentPlayerPosition(myType);
            transform.localPosition = new Vector3(cur.x, 0, cur.z);
        }
        else
        {
            gameObject.SetActive(false);
        }
    }

}
