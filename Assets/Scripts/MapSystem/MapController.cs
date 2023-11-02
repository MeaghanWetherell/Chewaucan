using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class MapController : MonoBehaviour
{
    public InputActionReference mapRef;
    private VisualElement _root;
    private bool IsMapOpen => _root.ClassListContains("root-container-full");

    // Start is called before the first frame update
    void Start()
    {
        _root = GetComponent<UIDocument>().rootVisualElement.Q<VisualElement>("Container");
    }

    private void OnEnable()
    {
        mapRef.action.performed += (InputAction.CallbackContext context) => { ToggleMap(!IsMapOpen); };
    }

    private void OnDisable()
    {
        mapRef.action.performed -= (InputAction.CallbackContext context) => { ToggleMap(!IsMapOpen); };
    }

    void ToggleMap(bool mode)
    {
        _root.EnableInClassList("root-container-mini", !mode);
        _root.EnableInClassList("root-container-full", mode);
    }
}
