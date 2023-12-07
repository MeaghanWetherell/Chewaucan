using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class MapUIController : MonoBehaviour
{
    [SerializeField] GameObject minimapObj;

    public InputActionReference hideMapRef;
    public InputActionReference fullMapViewRef;

    private void OnEnable()
    {
        hideMapRef.action.performed += ToggleMap;
        fullMapViewRef.action.performed += SwitchToFullMapView;
    }

    private void OnDisable()
    {
        hideMapRef.action.performed -= ToggleMap;
        fullMapViewRef.action.performed -= SwitchToFullMapView;
    }

    void ToggleMap(InputAction.CallbackContext context)
    {
        minimapObj.SetActive(!minimapObj.activeSelf);
    }

    void SwitchToFullMapView(InputAction.CallbackContext context)
    {
        SceneManager.LoadScene("FullMapView");
    }
}
