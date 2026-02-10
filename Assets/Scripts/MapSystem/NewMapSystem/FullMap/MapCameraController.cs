using System.Collections;
using System.Collections.Generic;
using Misc;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class MapCameraController : MonoBehaviour
{
    [SerializeField] private float zoomRate = 0.25f;
    [SerializeField] private float minZoom = 50f;
    [SerializeField] private float maxZoom = 500f;
    [SerializeField] private float dragSpeed = 2f;
    [SerializeField] private GameObject mapCameraObj;
    private Camera mapCamera;

    public InputActionReference mapZoomRef;
    public InputActionReference switchToWorldRef;
    public InputActionReference esc;

    private Vector3 clickOrigin;
    private bool isSlowing = false;

    private void OnEnable()
    {
        mapZoomRef.action.performed += MapZoom;
        switchToWorldRef.action.performed += SwitchToWorldScene;
        esc.action.performed += SwitchToWorldScene;
    }

    private void OnDisable()
    {
        mapZoomRef.action.performed -= MapZoom;
        switchToWorldRef.action.performed -= SwitchToWorldScene;
        esc.action.performed -= SwitchToWorldScene;
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
            StopCoroutine(SmoothCameraStop());
            isSlowing = false;

            clickOrigin = mapCamera.ScreenToWorldPoint(Input.mousePosition);
        }

        if (Input.GetMouseButton(2) || Input.GetMouseButton(1))
        {
            StopCoroutine(SmoothCameraStop());
            isSlowing = false;

            Vector3 posDifference = clickOrigin - mapCamera.ScreenToWorldPoint(Input.mousePosition);
            Vector3 targetPos = mapCameraObj.transform.position + posDifference;

            mapCameraObj.transform.position = Vector3.Lerp(mapCameraObj.transform.position, targetPos, Time.deltaTime * dragSpeed);
        }

        if (Input.GetMouseButtonUp(2) || Input.GetMouseButtonUp(1))
        {
            if (!isSlowing)
            {
                StartCoroutine(SmoothCameraStop());
            }
        }
    }

    //continues smoothly moving the camera for half a second after the mouse button is released
    IEnumerator SmoothCameraStop()
    {
        isSlowing = true;
        int frameRate = (int) (1.0f / Time.deltaTime);
        float dragRate = dragSpeed; //we want the drag speed to gradually slow down

        Vector3 posDifference = clickOrigin - mapCamera.ScreenToWorldPoint(Input.mousePosition);
        Vector3 targetPos = mapCameraObj.transform.position + posDifference;

        for (int i = 0; i < frameRate/2; i++)
        {
            mapCameraObj.transform.position = Vector3.Lerp(mapCameraObj.transform.position, targetPos, Time.deltaTime * dragRate);
            dragRate = dragRate - ( dragSpeed / (frameRate/2) );
            yield return new WaitForEndOfFrame();
        }

        isSlowing = false;
        yield return null;
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
        SceneLoadWrapper.sceneLoadWrapper.LoadScene("Modern Map");
    }
}
