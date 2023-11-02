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

    public GameObject Player;
    [Range(-10, 15)]
    public float miniMultiplyer = 5.3f;
    [Range(-10, 15)]
    public float fullMultiplyer = 7f;
    private VisualElement _playerRepresentation;

    // Start is called before the first frame update
    void Start()
    {
        _root = GetComponent<UIDocument>().rootVisualElement.Q<VisualElement>("Container");
        _playerRepresentation = _root.Q<VisualElement>("Player");
    }

    private void LateUpdate()
    {
        var multiplyer = IsMapOpen ? fullMultiplyer : miniMultiplyer;

        //another idea, use transform origin

        //_playerRepresentation.style.translate = new Translate(Player.transform.position.x * multiplyer,
            //Player.transform.position.z * -multiplyer, 0);
        _playerRepresentation.style.rotate = new Rotate(new Angle(Player.transform.rotation.eulerAngles.y));
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
