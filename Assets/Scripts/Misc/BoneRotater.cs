using System.Collections;
using System.Collections.Generic;
using Match3.DataClasses;
using UnityEngine;
using UnityEngine.InputSystem;

public class BoneRotater : MonoBehaviour
{
        [Tooltip("Mouse position vector2 from input sys")]public InputActionReference mousePos;
        
        [Tooltip("Scroll delta from input sys")]public InputActionReference scroll;

        [Tooltip("Appropriate click button from input sys")]public InputActionReference clicked;
        
        [Tooltip("Flip button from input sys")]public InputActionReference flip;

        [Tooltip("The bone object that the render texture camera points to")]public GameObject bone;

        [Tooltip("Scalar on the x rotation when moving the camera")]public float xFudge;

        [Tooltip("Scalar for the distance scrolled")] public float distScalar;

        [Tooltip("Max size of the camera")] public float maxSize;

        [Tooltip("Min size of the camera")] public float minSize;

        [Tooltip("Whether this script should control scrolling")]
        public bool canScroll = true;
        
        public AudioSource rotateSound;

        public AudioSource flipSound;

        private bool _mouseDown;

        private Vector3 _orbitAngle;

        public bool isEnabled = false;

        private Camera cam;
        
        private bool playingRotateSound = false;

        private Vector2 diff = Vector2.zero;

        private float dt = 0;

        public void StartUp()
        {
            _mouseDown = true;
        }

        private void Awake()
        {
            cam = GetComponent<Camera>();
        }
        
        private void OnEnable()
        {
            rotateSound.ignoreListenerPause = true;
            flipSound.ignoreListenerPause = true;
            clicked.action.started += ONClickDown;
            clicked.action.canceled += ONClickUp;
            flip.action.started += Flip;
        }

        private void OnDisable()
        {
            playingRotateSound = false;
            if(rotateSound != null)
                rotateSound?.Stop();
            clicked.action.started -= ONClickDown;
            clicked.action.canceled -= ONClickUp;
            flip.action.started -= Flip;
        }

        private void ONClickDown(InputAction.CallbackContext callbackContext)
        {
            _mouseDown = true;
            diff = Vector2.zero;
            dt = 0;
        }

        private void ONClickUp(InputAction.CallbackContext callbackContext)
        {
            _mouseDown = false;
        }

        private void Flip(InputAction.CallbackContext callbackContext)
        {
            flipSound?.Play();
            bone.transform.Rotate(Vector3.right, 180);
        }
        
        private void Update()
        {
            if (isEnabled)
            {
                if (_mouseDown)
                {
                    if (!playingRotateSound)
                    {
                        playingRotateSound = true;
                        rotateSound?.Play();
                    }
                    diff += mousePos.action.ReadValue<Vector2>();
                    dt += Time.deltaTime;
                    if (Mathf.Abs(diff.x) > 1.5f)
                    {
                        float yRot = xFudge * diff.x * dt * 1000;
                        bone.transform.Rotate(Vector3.up, yRot);
                        diff = Vector2.zero;
                        dt = 0;
                    }
                }
                else
                {
                    playingRotateSound = false;
                    rotateSound?.Stop();
                }
                if (canScroll)
                {
                    float delta = scroll.action.ReadValue<Vector2>().y;
                    cam.orthographicSize -= delta*Time.deltaTime*distScalar*1000;
                    if (cam.orthographicSize > maxSize)
                        cam.orthographicSize = maxSize;
                    else if (cam.orthographicSize < minSize)
                        cam.orthographicSize = minSize;
                }
            }
            
        }
}
