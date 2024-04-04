using Match3.DataClasses;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace Match3.Game
{
    public class SecondaryViewManager : MonoBehaviour
    {
        public static SecondaryViewManager secondaryViewManager;
        
        [Tooltip("Mouse position vector2 from input sys")]public InputActionReference mousePos;
        
        [Tooltip("Scroll delta from input sys")]public InputActionReference scroll;

        [Tooltip("Right click button from input sys")]public InputActionReference clicked;
        
        [Tooltip("The bone object that the render texture camera points to")]public GameObject bone;

        [Tooltip("The image on the UI that the bone will be rendered to")]public GameObject view;
        
        [Tooltip("Scalar on the x rotation when moving the camera")]public float xFudge;

        [Tooltip("Scalar for the distance scrolled")] public float distScalar;

        [Tooltip("Max distance from the camera")] public float maxDist;

        [Tooltip("Min distance from the camera")] public float minDist;

        private bool _mouseDown;

        private Vector2 _lastMousePos;

        private Vector3 _orbitAngle;

        private bool isEnabled = false;

        private void Awake()
        {
            secondaryViewManager = this;
            _lastMousePos = mousePos.action.ReadValue<Vector2>();
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

        public void SetView(MeshDataObj target)
        {
            view.SetActive(true);
            bone.GetComponent<MeshRenderer>().material = target.material;
            bone.GetComponent<MeshFilter>().mesh = target.mesh;
            isEnabled = true;
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
                        float yRot = xFudge * diff.x * Time.deltaTime;
                        bone.transform.Rotate(Vector3.up, yRot);
                    }
                }
                _lastMousePos = mousePos.action.ReadValue<Vector2>();
                float delta = scroll.action.ReadValue<Vector2>().y;
                bone.transform.Translate(new Vector3(0,0,delta*distScalar*Time.deltaTime));
                Vector3 pos = bone.transform.position;
                if (pos.z < minDist)
                    bone.transform.position = new Vector3(pos.x, pos.y, minDist);
                else if (pos.z > maxDist)
                    bone.transform.position = new Vector3(pos.x, pos.y, maxDist);
            }
            
        }
    }
}
