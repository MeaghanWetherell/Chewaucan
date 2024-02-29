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

    [SerializeField] private DialProperties.Dial currentDial;

    public void SelectHand(GameObject obj)
    {
        selectedHand = obj.GetComponent<RectTransform>();
        dialProperties = DialProperties.instance;
        if (selectedHand != null)
        {
            if (selectedHand.name.Equals("ArcheologyDial"))
            {
                currentDial = DialProperties.Dial.ARCHEOLOGY;
            }
            else if (selectedHand.name.Equals("GeologyDial"))
            {
                currentDial = DialProperties.Dial.GEOLOGY;
            }
            else if (selectedHand.name.Equals("BiologyDial"))
            {
                currentDial = DialProperties.Dial.BIOLOGY;
            }
        }
    }

    public void MoveHandLeft()
    {
        if (selectedHand == null)
        {
            Debug.Log("Selected a hand to move");
            return;
        }

        if (dialProperties.getCurRotationAmount(currentDial) < maxRotation)
        {
            selectedHand.Rotate(Vector3.forward, rotInterval);
            dialProperties.changeCurRotationAmount(rotInterval, currentDial);
        }
    }

    public void MoveHandRight()
    {
        if (selectedHand == null)
        {
            Debug.Log("Selected a hand to move");
            return;
        }

        if (dialProperties.getCurRotationAmount(currentDial) > minRotation)
        {
            selectedHand.Rotate(Vector3.forward, -rotInterval);
            dialProperties.changeCurRotationAmount(-rotInterval, currentDial);
        }
    }
}
