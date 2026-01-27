using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class DraggableImage : MonoBehaviour
{
    public static DraggableImage activeImage;

    [NonSerialized]public DraggableImageFactory parent;

    public InputActionReference mousePosition;

    public InputActionReference leftClick;

    public RectTransform myRect;

    public Image myImg;

    [NonSerialized]public Canvas myCanvas;

    [NonSerialized]public DragTarget myTarget;

    private void Start()
    {
        activeImage = this;
        mousePosition.action.performed += FollowMouse;
        leftClick.action.canceled += OnMouseReleased;
    }

    private void OnDestroy()
    {
        activeImage = null;
        mousePosition.action.performed -= FollowMouse;
        leftClick.action.canceled -= OnMouseReleased;
    }

    private void FollowMouse(InputAction.CallbackContext context)
    {
        if (myCanvas == null)
            return;
        Vector2 screenPt = context.ReadValue<Vector2>();
        Vector2 pos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle((RectTransform)myCanvas.transform, screenPt,  myCanvas.worldCamera, out pos);
        myRect.anchoredPosition = pos;
    }

    private void OnMouseReleased(InputAction.CallbackContext context)
    {
        if(myTarget != null)
            myTarget.SetMyValue(this);
        Destroy(gameObject);
    }
}
