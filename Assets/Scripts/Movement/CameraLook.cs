using System;
using System.Collections;
using System.Collections.Generic;
using Misc;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraLook : MonoBehaviour
{
    public float minViewDist = 25f;
    [SerializeField] float mouseSensitivity = 25f;

    public Transform mainCamera;
    public InputActionReference lookRef;
    public Vector2 lookDecel;

    Vector2 _lookInput;
    float _xRotation;
    private float _yRotation;

    [NonSerialized] public bool clampY;
    [NonSerialized] public float clampYCenter;

    //subscribe to event functions
    private void Start()
    {
        _lookInput = Vector3.zero;
        PauseCallback.pauseManager.SubscribeToPause(OnPause);
        PauseCallback.pauseManager.SubscribeToResume(OnResume);
        if(PauseCallback.pauseManager.isPaused)
            OnPause();
    }

    //unsubscribe
    private void OnDestroy()
    {
        PauseCallback.pauseManager.UnsubToPause(OnPause);
        PauseCallback.pauseManager.UnsubToResume(OnResume);
    }

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
        float mouseX = _lookInput.x * mouseSensitivity * Time.deltaTime;
        float mouseY = _lookInput.y * mouseSensitivity * Time.deltaTime;

        _xRotation -= mouseY;
        _xRotation = Mathf.Clamp(_xRotation, -90f, minViewDist);

        if (clampY)
        {
            _yRotation += mouseX;
            _yRotation = Mathf.Clamp(_yRotation, clampYCenter-90, clampYCenter+90);
            
            mainCamera.transform.localRotation = Quaternion.Euler(_xRotation, _yRotation, 0f);
        }
        else
        {
            mainCamera.transform.localRotation = Quaternion.Euler(_xRotation, 0f, 0f);
            transform.Rotate(Vector3.up * mouseX);
        }

        if (_lookInput.x > 0)
        {
            _lookInput.x -= lookDecel.x*Time.deltaTime;
            if (_lookInput.x < 0)
                _lookInput.x = 0;
        }
        else
        {
            _lookInput.x += lookDecel.x*Time.deltaTime;
            if (_lookInput.x > 0)
                _lookInput.x = 0;
        }
        if (_lookInput.y > 0)
        {
            _lookInput.y -= lookDecel.y*Time.deltaTime;
            if (_lookInput.y < 0)
                _lookInput.y = 0;
        }
        else
        {
            _lookInput.y += lookDecel.y*Time.deltaTime;
            if (_lookInput.y > 0)
                _lookInput.y = 0;
        }
    }

    //on any mouse movement
    public void OnLook(InputAction.CallbackContext context)
    {
        if (!(this.GetComponent<SwimmingMovement>().DiveOngoing()))
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

    //disable look on pause
    public void OnPause()
    {
        this.enabled = false;
    }

    //reenable on resume
    public void OnResume()
    {
        this.enabled = true;
    }
}
