using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Match3
{
    public class BoneCam : MonoBehaviour
    {
        public InputActionReference mousePos;

        public InputActionReference clicked;

        public float yFudge;

        public float xFudge;

        private bool mouseDown;

        private Vector2 lastMousePos;

        private Vector3 orbitAngle;

        private void OnEnable()
        {
            clicked.action.started += onClickDown;
            clicked.action.canceled += onClickUp;
        }

        private void OnDisable()
        {
            clicked.action.started -= onClickDown;
            clicked.action.canceled -= onClickUp;
        }

        private void onClickDown(InputAction.CallbackContext callbackContext)
        {
            mouseDown = true;
        }

        private void onClickUp(InputAction.CallbackContext callbackContext)
        {
            mouseDown = false;
        }

        private void Awake()
        {
            lastMousePos = mousePos.action.ReadValue<Vector2>();
        }

        private void Update()
        {
            float yRot=0;
            float xRot=0;
            if (mouseDown)
            {
                Vector2 diff = mousePos.action.ReadValue<Vector2>()-lastMousePos;
                if(diff.x > 0.75f)
                    yRot = xFudge * diff.x * Time.deltaTime;
                //y-rotation was fiddly, so I locked it. 
                /*
                if(diff.y > 0.75f)
                    xRot = yFudge * -diff.y * Time.deltaTime;
                */
            }

            orbitAngle += new Vector3(xRot, yRot, 0);
            Quaternion lookDir = Quaternion.Euler(orbitAngle );
            Vector3 lookPos = Vector3.zero - lookDir*Vector3.forward*10;
            transform.SetPositionAndRotation(lookPos, lookDir);
            lastMousePos = mousePos.action.ReadValue<Vector2>();
        }
    }
}
