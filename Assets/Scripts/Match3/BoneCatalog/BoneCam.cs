using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Match3
{
    //Manages the bone viewer camera
    public class BoneCam : MonoBehaviour
    {
        [Tooltip("Mouse position vector2 from input sys")]public InputActionReference mousePos;

        [Tooltip("Left click button from input sys")]public InputActionReference clicked;
        
        //currently disabled
        [NonSerialized][Tooltip("Scalar on the y rotation when moving the camera")]public float yFudge;

        [Tooltip("Scalar on the x rotation when moving the camera")]public float xFudge;

        private bool _mouseDown;

        private Vector2 _lastMousePos;

        private Vector3 _orbitAngle;

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

        private void Awake()
        {
            _lastMousePos = mousePos.action.ReadValue<Vector2>();
        }

        private void Update()
        {
            float yRot=0;
            float xRot=0;
            if (_mouseDown)
            {
                Vector2 diff = mousePos.action.ReadValue<Vector2>()-_lastMousePos;
                if(diff.x > 0.75f)
                    yRot = xFudge * diff.x * Time.deltaTime;
                //y-rotation was fiddly, so I locked it. 
                /*
                if(diff.y > 0.75f)
                    xRot = yFudge * -diff.y * Time.deltaTime;
                */
            }

            _orbitAngle += new Vector3(xRot, yRot, 0);
            Quaternion lookDir = Quaternion.Euler(_orbitAngle );
            Vector3 lookPos = Vector3.zero - lookDir*Vector3.forward*10;
            transform.SetPositionAndRotation(lookPos, lookDir);
            _lastMousePos = mousePos.action.ReadValue<Vector2>();
        }
    }
}
