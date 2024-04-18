using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TeleportWaypoint : MonoBehaviour
{
    [Tooltip("The y coordinate to teleport the player to")]
    [SerializeField] float yValue;

    // the world position this waypoint teleports to. Will be automotically set
    private Vector3 teleportToPosition;

    private Transform waypointObj; // the transform of this gameobject

    // gameobjects and components pertaining to the canvas. All are automotically set
    private GameObject teleportUI;
    private TextMeshProUGUI teleportPositionLabel;
    private Animator animator;
    private TeleportButton teleportButton;

    private Vector3 objPosition;

    // Start is called before the first frame update
    void Start()
    {
        waypointObj = this.transform;
        objPosition = waypointObj.localPosition;
        
        teleportToPosition = new Vector3(objPosition.x, yValue, objPosition.z);

        teleportUI = GameObject.Find("Teleport Info");
        animator = teleportUI.GetComponent<Animator>();
        teleportButton = FindAnyObjectByType<TeleportButton>();
        teleportPositionLabel = teleportUI.GetComponentInChildren<TextMeshProUGUI>();
    }

    // When this waypoint is clicked, the UI animates in to show the coordinates and 
    // button to teleport. The button's coordinates are set to this object's teleportToPosition
    private void OnMouseDown()
    {
        Debug.Log("Teleport to "+teleportToPosition);
        animator.SetBool("active", true);
        teleportPositionLabel.text = teleportToPosition.ToString();
        teleportButton.setTeleportTo(teleportToPosition);
    }


}
