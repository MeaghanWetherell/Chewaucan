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
        
        [Tooltip("Bone rotator ref")] public BoneRotater boneRotater;

        private float originX;

        private float originY;

        private bool _mouseDown;

        private Vector3 _orbitAngle;

        private bool isEnabled = false;

        private Camera cam;

        private void Awake()
        {
            secondaryViewManager = this;
            cam = GetComponent<Camera>();
            Vector3 pos = transform.position;
            originX = pos.x;
            originY = pos.y;
        }

        public void SetView(MeshDataObj target)
        {
            view.SetActive(true);
            for (int i = 0; i < bone.transform.childCount; i++)
            {
                Destroy(bone.transform.GetChild(i).gameObject); 
            }

            Instantiate(target.meshPrefab, bone.transform);
            boneRotater.enabled = true;
            isEnabled = true;
        }
        
        private void Update()
        {
            if (isEnabled)
            {
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
