using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraLook : MonoBehaviour
{
    public float minViewDist = 25f;
    [SerializeField] float mouseSensitivity = 25f;

    public Transform mainCamera;
    public InputActionReference lookRef;

    Vector2 _lookInput;
    float _xRotation;
    
    // Start is called before the first frame update
    private void OnEnable()
    {
        Cursor.lockState = CursorLockMode.Locked; //hides the cursor
        Cursor.visible = true;
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
        float mouseX = _lookInput.x * mouseSensitivity * Time.deltaTime;
        float mouseY = _lookInput.y * mouseSensitivity * Time.deltaTime;

        _xRotation -= mouseY;
        _xRotation = Mathf.Clamp(_xRotation, -90f, minViewDist);

        mainCamera.transform.localRotation = Quaternion.Euler(_xRotation, 0f, 0f);
        transform.Rotate(Vector3.up * mouseX);
    }

    public void OnLook(InputAction.CallbackContext context)
    {
        if (!(this.GetComponent<PlayerMovement>().DiveOngoing()))
        {
            _lookInput = context.ReadValue<Vector2>();
        }
        else
        {
            _lookInput = Vector3.zero;
        }
    }

    public void SetMinDist(float n)
    {
        minViewDist = n;
    }
}
