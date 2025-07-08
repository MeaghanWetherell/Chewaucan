using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class BoneRotatorSelector : MonoBehaviour
{
    public RectTransform img1;
    
    public RectTransform img2;

    public BoneRotater rot1;
    
    public BoneRotater rot2;

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
    
    private void OnClick(InputAction.CallbackContext context)
    {
        Vector2 clickPos = mousePos.action.ReadValue<Vector2>();
        Debug.Log(clickPos);
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
