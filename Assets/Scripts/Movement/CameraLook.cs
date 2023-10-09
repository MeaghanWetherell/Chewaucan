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

    Vector2 lookInput;
    float xRotation;
    
    // Start is called before the first frame update
    void OnEnable()
    {
        Cursor.lockState = CursorLockMode.Locked; //hides the cursor
    }

    private void OnDisable()
    {
        Cursor.lockState = CursorLockMode.None;
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

    void OnLook(InputValue value)
    {
        lookInput = value.Get<Vector2>();
    }
}
