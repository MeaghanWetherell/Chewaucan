using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class RotateDialHand : MonoBehaviour
{
    private RectTransform selectedHand;
    [SerializeField] private float minRotation = 180f;
    [SerializeField] private float maxRotation = 0f;

    private void Start()
    {
        
    }

    public void SelectHand(GameObject obj)
    {
        selectedHand = obj.GetComponent<RectTransform>();
    }

    public void MoveHandLeft()
    {
        if (selectedHand == null)
        {
            Debug.Log("Selected a hand to move");
            return;
        }

        if (selectedHand.rotation.z < maxRotation)
        {
            selectedHand.Rotate(Vector3.forward, 10);
        }
    }

    public void MoveHandRight()
    {
        if (selectedHand == null)
        {
            Debug.Log("Selected a hand to move");
            return;
        }

        Debug.Log(selectedHand.rotation.normalized);
        if (selectedHand.rotation.z <= minRotation)
        {
            selectedHand.Rotate(Vector3.forward, -10);
        }
    }
}
