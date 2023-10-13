using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraLook : MonoBehaviour
{
    [SerializeField] float minViewDist = 25f;
    [SerializeField] float mouseSensitivity = 25f;

    public Transform mainCamera;
    public InputActionReference lookRef;

    Vector2 lookInput;
    float xRotation;
    
    // Start is called before the first frame update
    private void OnEnable()
    {
        Cursor.lockState = CursorLockMode.Locked; //hides the cursor
        lookRef.action.performed += OnLook;
    }

    private void OnDisable()
    {
        Cursor.lockState = CursorLockMode.None;
        lookRef.action.performed -= OnLook;
    }

    // Update is called once per frame
    void Update()
    {
        //This code could be altered to only allow camera rotation in the vertical direction, since AD already rotates horizontal
        float mouseX = lookInput.x * mouseSensitivity * Time.deltaTime;
        float mouseY = lookInput.y * mouseSensitivity * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, minViewDist);

        mainCamera.transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        transform.Rotate(Vector3.up * mouseX);
    }

    public void OnLook(InputAction.CallbackContext context)
    {
        lookInput = context.ReadValue<Vector2>();
    }
}
