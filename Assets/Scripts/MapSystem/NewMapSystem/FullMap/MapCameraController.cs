using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class MapCameraController : MonoBehaviour
{
    [SerializeField] private float zoomRate = 0.25f;
    [SerializeField] private float minZoom = 50f;
    [SerializeField] private float maxZoom = 500f;
    [SerializeField] private GameObject mapCameraObj;
    private Camera mapCamera;

    public InputActionReference mapZoomRef;
    public InputActionReference switchToWorldRef;

    private Vector3 clickOrigin;

    private void OnEnable()
    {
        mapZoomRef.action.performed += MapZoom;
        switchToWorldRef.action.performed += SwitchToWorldScene;
    }

    private void OnDisable()
    {
        mapZoomRef.action.performed -= MapZoom;
        switchToWorldRef.action.performed -= SwitchToWorldScene;
    }

    // Start is called before the first frame update
    void Start()
    {
        mapCamera = mapCameraObj.GetComponent<Camera>();
        mapCamera.orthographicSize = 400f;
        clickOrigin = Vector3.zero;
    }

    // Update is called once per frame
    void Update()
    {
        //map view is moved on either center or right mouse button click
        if (Input.GetMouseButtonDown(2) || Input.GetMouseButtonDown(1))
        {
            clickOrigin = mapCamera.ScreenToWorldPoint(Input.mousePosition);
        }

        if (Input.GetMouseButton(2) || Input.GetMouseButton(1))
        {
            Vector3 posDifference = clickOrigin - mapCamera.ScreenToWorldPoint(Input.mousePosition);

            mapCameraObj.transform.position += posDifference;
        }
    }

    //uses the scroll wheel to zoom in and out of the map
    private void MapZoom(InputAction.CallbackContext context)
    {
        Vector2 zoom = context.ReadValue<Vector2>();

        mapCamera.orthographicSize += zoom.y * -1 * zoomRate;
        if (mapCamera.orthographicSize > maxZoom) { mapCamera.orthographicSize = maxZoom; }
        if (mapCamera.orthographicSize < minZoom) { mapCamera.orthographicSize = minZoom; }
    }

    private void SwitchToWorldScene(InputAction.CallbackContext context)
    {
        SceneManager.LoadScene("Modern Map");
    }
}
