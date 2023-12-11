using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportWaypoint : MonoBehaviour
{
    [SerializeField] Vector3 teleportToPosition;
    [SerializeField] Transform waypointObj;
    [SerializeField] GameObject teleportUI;
    [SerializeField] Animator animator;

    private Vector3 objPosition;
    
    // Start is called before the first frame update
    void Start()
    {
        waypointObj = this.transform;
        objPosition = waypointObj.localPosition;
        teleportToPosition = new Vector3(objPosition.x, -objPosition.z, objPosition.y);
        animator = teleportUI.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnMouseDown()
    {
        Debug.Log("Teleport to "+teleportToPosition);
        animator.SetBool("active", true);
    }


}
