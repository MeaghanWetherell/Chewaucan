using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class BoneRotatorSelector : MonoBehaviour
{
    [Tooltip("Image of the compare bone")]public RectTransform img1;
    
    [Tooltip("Image of the mastodon bone")]public RectTransform img2;

    [Tooltip("Ref to the rotator script for the compare bone")]public BoneRotater rot1;
    
    [Tooltip("Ref to the rotator script for the mastodon bone")]public BoneRotater rot2;

    [Tooltip("Mouse position vector2 from input sys")]public InputActionReference mousePos;

    [Tooltip("Appropriate click button from input sys")]public InputActionReference clicked;
    
    private void OnEnable()
    {
        clicked.action.started += OnClick;
    }

    private void OnDisable()
    {
        clicked.action.started -= OnClick;
    }
    
    //When the player clicks on the image of a bone, activate its bone rotator
    private void OnClick(InputAction.CallbackContext context)
    {
        Vector2 clickPos = mousePos.action.ReadValue<Vector2>();
        if (RectTransformUtility.RectangleContainsScreenPoint(img1, clickPos))
        {
            rot2.enabled = false; 
            rot1.enabled = true;
            rot1.StartUp();
        }
        else if (RectTransformUtility.RectangleContainsScreenPoint(img2, clickPos))
        {
            rot1.enabled = false;
            rot2.enabled = true;
            rot2.StartUp();
        }
    }
}
