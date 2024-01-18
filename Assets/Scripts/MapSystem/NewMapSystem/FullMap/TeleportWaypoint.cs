using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TeleportWaypoint : MonoBehaviour
{
    [SerializeField] float yValue;
    [SerializeField] Vector3 teleportToPosition;
    private Transform waypointObj;
    [SerializeField] GameObject teleportUI;
    [SerializeField] TextMeshProUGUI teleportPositionLabel;
    private Animator animator;
    private TeleportButton teleportButton;

    private Vector3 objPosition;
    
    // Start is called before the first frame update
    void Start()
    {
        waypointObj = this.transform;
        objPosition = waypointObj.localPosition;
        teleportToPosition = new Vector3(objPosition.x, yValue, objPosition.y);
        animator = teleportUI.GetComponent<Animator>();
        teleportButton = FindAnyObjectByType<TeleportButton>();
    }

    private void OnMouseDown()
    {
        Debug.Log("Teleport to "+teleportToPosition);
        animator.SetBool("active", true);
        teleportPositionLabel.text = teleportToPosition.ToString();
        teleportButton.setTeleportTo(teleportToPosition);
    }


}
