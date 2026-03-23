using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class DraggableImage : MonoBehaviour
{
    //singleton ref to the current active image. used to prevent creation of more than one
    public static DraggableImage activeImage;

    public InputActionReference mousePosition;

    public InputActionReference leftClick;

    [Tooltip("The rect transform of this image")]
    public RectTransform myRect;

    [Tooltip("The image component of this image")]
    public Image myImg;

    //the canvas this image is on
    [NonSerialized]public Canvas myCanvas;

    //the current drag target this image is being dragged over. set by the drag target on collision
    [NonSerialized]public DragTarget myTarget;
    
    //reference to the factory this image came from
    [NonSerialized]public DraggableImageFactory parent;

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

    //matches the position of the image to the mouse position
    private void FollowMouse(InputAction.CallbackContext context)
    {
        if (myCanvas == null)
            return;
        Vector2 screenPt = context.ReadValue<Vector2>();
        Vector2 pos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle((RectTransform)myCanvas.transform, screenPt,  myCanvas.worldCamera, out pos);
        myRect.anchoredPosition = pos;
    }

    //when the mouse is released, send our info the drag target we're on if applicable and destroy ourselves
    private void OnMouseReleased(InputAction.CallbackContext context)
    {
        if(myTarget != null)
            myTarget.SetMyValue(this);
        Destroy(gameObject);
    }
}
