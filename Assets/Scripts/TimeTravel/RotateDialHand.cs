using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class RotateDialHand : MonoBehaviour
{
    private RectTransform selectedHand;
    private DialProperties dialProperties;
    [SerializeField] private float rotInterval = 10;
    [SerializeField] private float minRotation = -180f;
    [SerializeField] private float maxRotation = 0f;

    //the rotation values can be weird in the editor itself, so this keeps track
    // of how far the hand is rotated instead
    private float curRotationAmount = 0f;

    private void Start()
    {
        curRotationAmount = 0f;
    }
    public void SelectHand(GameObject obj)
    {
        selectedHand = obj.GetComponent<RectTransform>();
        dialProperties = obj.GetComponent<DialProperties>();
    }

    public void MoveHandLeft()
    {
        if (selectedHand == null)
        {
            Debug.Log("Selected a hand to move");
            return;
        }

        if (dialProperties.getCurRotationAmount() < maxRotation)
        {
            selectedHand.Rotate(Vector3.forward, rotInterval);
            dialProperties.changeCurRotationAmount(rotInterval);
        }
    }

    public void MoveHandRight()
    {
        if (selectedHand == null)
        {
            Debug.Log("Selected a hand to move");
            return;
        }

        if (dialProperties.getCurRotationAmount() > minRotation)
        {
            selectedHand.Rotate(Vector3.forward, -rotInterval);
            dialProperties.changeCurRotationAmount(-rotInterval);
        }
    }
}
