using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportWaypoint : MonoBehaviour
{
    [SerializeField] Vector3 teleportToPosition;
    [SerializeField] Transform waypointObj;

    private Vector3 objPosition;
    
    // Start is called before the first frame update
    void Start()
    {
        waypointObj = this.transform;
        objPosition = waypointObj.localPosition;
        teleportToPosition = new Vector3(objPosition.x, -objPosition.z, objPosition.y);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
