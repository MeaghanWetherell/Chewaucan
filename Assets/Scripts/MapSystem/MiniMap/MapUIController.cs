using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MapUIController : MonoBehaviour
{
    [SerializeField] GameObject minimapObj;

    public InputActionReference hideMapRef;
    public InputActionReference fullMapViewRef;

    private void OnEnable()
    {
        hideMapRef.action.performed += ToggleMap;
    }

    private void OnDisable()
    {
        hideMapRef.action.performed -= ToggleMap;
    }

    void ToggleMap(InputAction.CallbackContext context)
    {
        minimapObj.SetActive(!minimapObj.activeSelf);
    }
}
