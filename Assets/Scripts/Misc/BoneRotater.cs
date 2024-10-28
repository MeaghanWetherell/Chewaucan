using System.Collections;
using System.Collections.Generic;
using Match3.DataClasses;
using UnityEngine;
using UnityEngine.InputSystem;

public class BoneRotater : MonoBehaviour
{
        [Tooltip("Mouse position vector2 from input sys")]public InputActionReference mousePos;
        
        [Tooltip("Scroll delta from input sys")]public InputActionReference scroll;

        [Tooltip("Right click button from input sys")]public InputActionReference clicked;

        [Tooltip("The bone object that the render texture camera points to")]public GameObject bone;

        [Tooltip("Scalar on the x rotation when moving the camera")]public float xFudge;

        [Tooltip("Scalar for the distance scrolled")] public float distScalar;

        [Tooltip("Max size of the camera")] public float maxSize;

        [Tooltip("Min size of the camera")] public float minSize;

        private bool _mouseDown;

        private Vector2 _lastMousePos;

        private Vector3 _orbitAngle;

        public bool isEnabled = false;

        private Camera cam;

        private void Awake()
        {
            _lastMousePos = mousePos.action.ReadValue<Vector2>();
            cam = GetComponent<Camera>();
        }
        
        private void OnEnable()
        {
            clicked.action.started += ONClickDown;
            clicked.action.canceled += ONClickUp;
        }

        private void OnDisable()
        {
            clicked.action.started -= ONClickDown;
            clicked.action.canceled -= ONClickUp;
        }

        private void ONClickDown(InputAction.CallbackContext callbackContext)
        {
            _mouseDown = true;
        }

        private void ONClickUp(InputAction.CallbackContext callbackContext)
        {
            _mouseDown = false;
        }
        
        private void Update()
        {
            if (isEnabled)
            {
                if (_mouseDown)
                {
                    Vector2 diff = mousePos.action.ReadValue<Vector2>()-_lastMousePos;
                    if (diff.x > 0.75f)
                    {
                        float yRot = xFudge * diff.x * Time.deltaTime * 1000;
                        bone.transform.Rotate(Vector3.up, yRot);
                    }
                }
                _lastMousePos = mousePos.action.ReadValue<Vector2>();
                float delta = scroll.action.ReadValue<Vector2>().y;
                cam.orthographicSize -= delta*Time.deltaTime*distScalar*1000;
                if (cam.orthographicSize > maxSize)
                    cam.orthographicSize = maxSize;
                else if (cam.orthographicSize < minSize)
                    cam.orthographicSize = minSize;
            }
            
        }
}
