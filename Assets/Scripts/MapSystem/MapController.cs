using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class MapController : MonoBehaviour
{
    public InputActionReference mapRef;
    public InputActionReference hideMapRef;

    private VisualElement _root;
    private bool IsMapOpen => _root.ClassListContains("root-container-full");

    public GameObject Player;

    [Range(0, 350)]
    public float differenceX = 275;
    [Range(0, 350)]
    public float differenceY = 265;

    private VisualElement _playerRepresentation;

    private VisualElement _mapContainer;
    private VisualElement _mapImage;

    // Start is called before the first frame update
    void Start()
    {
        _root = GetComponent<UIDocument>().rootVisualElement.Q<VisualElement>("Container");
        _playerRepresentation = _root.Q<VisualElement>("Player");
        _mapImage = _root.Q<VisualElement>("Image");
        _mapContainer = _root.Q<VisualElement>("Map");
    }

    private void LateUpdate()
    {
        //sets the player icon to a position representative of world position
        _playerRepresentation.style.rotate = new Rotate(new Angle(Player.transform.rotation.eulerAngles.y));

        Length x = new Length((Player.transform.position.x) - differenceX, LengthUnit.Pixel);
        Length y = new Length((-Player.transform.position.z) + differenceY, LengthUnit.Pixel);

        _playerRepresentation.style.translate = new Translate(x, y, 0);

        //moves the minimap view along with the player
        if (!IsMapOpen)
        {
            var clampWidth = _mapImage.worldBound.width / 2 -
                _mapContainer.worldBound.width / 2;
            var clampHeight = _mapImage.worldBound.height / 2 -
                _mapContainer.worldBound.height / 2;

            var xPos = Mathf.Clamp((Player.transform.position.x) - differenceX,
                -clampWidth, clampWidth);
            var yPos = Mathf.Clamp((-Player.transform.position.z) + differenceY,
                -clampHeight, clampHeight);

            _mapImage.style.translate = new Translate(xPos*-1, yPos*-1, 0);
        }
        else
        {
            _mapImage.style.translate = new Translate(0, 0, 0);
        }
    }

    private void OnEnable()
    {
        mapRef.action.performed += (InputAction.CallbackContext context) => { ToggleMap(!IsMapOpen); };
        hideMapRef.action.performed += (InputAction.CallbackContext context) => { DisableMap(); };
    }

    private void OnDisable()
    {
        mapRef.action.performed -= (InputAction.CallbackContext context) => { ToggleMap(!IsMapOpen); };
        hideMapRef.action.performed -= (InputAction.CallbackContext context) => { DisableMap(); };
    }

    //press M to switch between a full and mini map view
    void ToggleMap(bool mode)
    {
        _root.EnableInClassList("root-container-mini", !mode);
        _root.EnableInClassList("root-container-full", mode);
    }

    //press 0 to hide the map in either view
    void DisableMap()
    {
        var display = _root.style.display == DisplayStyle.Flex ? DisplayStyle.None : DisplayStyle.Flex;
        _root.style.display = display;
    }
}
