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

        [Tooltip("Move controls from the input sys")] public InputActionReference move;
        
        [Tooltip("The bone object that the render texture camera points to")]public GameObject bone;

        [Tooltip("The image on the UI that the bone will be rendered to")]public GameObject view;
        
        [Tooltip("Scalar on the x rotation when moving the camera")]public float xFudge;

        [Tooltip("Scalar for the distance scrolled")] public float distScalar;

        [Tooltip("Max size of the camera")] public float maxSize;

        [Tooltip("Min size of the camera")] public float minSize;

        [Tooltip("Max x movement of the camera from start point")]
        public float maxX;
        
        [Tooltip("Max y movement of the camera from start point")]
        public float maxY;

        [Tooltip("Scalar on panning movement")]
        public float moveScalar;

        private float originX;

        private float originY;

        private bool _mouseDown;

        private Vector2 _lastMousePos;

        private Vector3 _orbitAngle;

        private bool isEnabled = false;

        private Camera cam;

        private void Awake()
        {
            secondaryViewManager = this;
            _lastMousePos = mousePos.action.ReadValue<Vector2>();
            cam = GetComponent<Camera>();
            Vector3 pos = transform.position;
            originX = pos.x;
            originY = pos.y;
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
                Vector2 movement = move.action.ReadValue<Vector2>();
                var orthographicSize = cam.orthographicSize;
                transform.Translate(movement.x*Time.deltaTime*moveScalar*orthographicSize, movement.y*Time.deltaTime*moveScalar*orthographicSize, 0, Space.World);
                Vector3 pos = transform.position;
                if (pos.x < originX-maxX)
                    transform.position = new Vector3(originX-maxX, pos.y, pos.z);
                else if (pos.x > originX + maxX)
                {
                    transform.position = new Vector3(originX + maxX, pos.y, pos.z);
                }
                pos = transform.position;
                if (pos.y < originY - maxY)
                    transform.position = new Vector3(pos.x, originY - maxY, pos.z);
                else if (pos.y > originY + maxY)
                    transform.position = new Vector3(pos.x, originY + maxY, pos.z);
                _lastMousePos = mousePos.action.ReadValue<Vector2>();
                float delta = scroll.action.ReadValue<Vector2>().y;
                cam.orthographicSize -= delta*Time.deltaTime*distScalar;
                if (cam.orthographicSize > maxSize)
                    cam.orthographicSize = maxSize;
                else if (cam.orthographicSize < minSize)
                    cam.orthographicSize = minSize;
            }
            
        }
    }
}
