using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;


/**
 * To be used in the ModernMap scene. Attached to the canvas minimap.
 * Controls toggling minimap on and off and switching to the full map view scene
 */
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

    //toggles the minimap on or off when the HideMap action is performed
    void ToggleMap(InputAction.CallbackContext context)
    {
        minimapObj.SetActive(!minimapObj.activeSelf);
    }

    //switches to the scene FullMapView, where the full world map can be observed
    void SwitchToFullMapView(InputAction.CallbackContext context)
    {
        SceneManager.LoadScene("FullMapView");
    }
}
