using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MapCameraController : MonoBehaviour
{
    [SerializeField] private float zoomRate = 0.25f;
    [SerializeField] private float minZoom = 50f;
    [SerializeField] private float maxZoom = 500f;
    [SerializeField] private GameObject mapCameraObj;
    private Camera mapCamera;

    public InputActionReference mapZoomRef;
    public InputActionReference mapMoveRef;

    private void OnEnable()
    {
        mapZoomRef.action.performed += MapZoom;
    }

    private void OnDisable()
    {
        mapZoomRef.action.performed -= MapZoom;
    }

    // Start is called before the first frame update
    void Start()
    {
        mapCamera = mapCameraObj.GetComponent<Camera>();
        mapCamera.orthographicSize = 400f;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //uses the scroll wheel to zoom in and out of the map
    private void MapZoom(InputAction.CallbackContext context)
    {
        Vector2 zoom = context.ReadValue<Vector2>();
        Debug.Log(zoom);

        mapCamera.orthographicSize += zoom.y * -1 * zoomRate;
        if (mapCamera.orthographicSize > maxZoom) { mapCamera.orthographicSize = maxZoom; }
        if (mapCamera.orthographicSize < minZoom) { mapCamera.orthographicSize = minZoom; }
    }
}
