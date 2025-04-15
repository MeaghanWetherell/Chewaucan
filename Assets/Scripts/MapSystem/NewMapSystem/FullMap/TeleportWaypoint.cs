using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TeleportWaypoint : MonoBehaviour
{
    public enum MapType
    {
        modern,
        pleistocene
    }
    
    [Tooltip("The y coordinate to teleport the player to")]
    [SerializeField] float yValue;

    [Tooltip("Map this waypoint is on")] public MapType mapType;

    [Tooltip("Default lock state of this waypoint")] public bool unlocked;

    [Tooltip("Unique name for the waypoint")]
    public String wpName;
    public String wpNameNice;//the name that is tidy and will be displayed

    // the world position this waypoint teleports to. Will be automatically set
    [NonSerialized] public Vector3 teleportToPosition;

    private Transform waypointObj; // the transform of this gameobject

    // gameobjects and components pertaining to the canvas. All are automatically set
    private GameObject teleportUI;
    private TextMeshProUGUI teleportPositionLabel;
    private TextMeshProUGUI teleportWaypointTitle;
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

        // teleportPositionLabel = teleportUI.GetComponentInChildren<TextMeshProUGUI>();
        teleportPositionLabel = teleportUI.transform.Find("position label").GetComponentInChildren<TextMeshProUGUI>();
        teleportWaypointTitle = teleportUI.transform.Find("Waypoint Title").GetComponentInChildren<TextMeshProUGUI>();
    }

    // When this waypoint is clicked, the UI animates in to show the coordinates and 
    // button to teleport. The button's coordinates are set to this object's teleportToPosition
    private void OnMouseDown()
    {
        Debug.Log("clicked WP");
        if (!unlocked) return;
        Debug.Log("Teleport to "+teleportToPosition);
        animator.SetBool("active", true);
        teleportPositionLabel.text = teleportToPosition.ToString();
        teleportWaypointTitle.text = wpNameNice;
        teleportButton.setTeleportTo(this);
    }


}
