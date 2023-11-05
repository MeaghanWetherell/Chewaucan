using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class MapController : MonoBehaviour
{
    public InputActionReference mapRef;
    private VisualElement _root;
    private bool IsMapOpen => _root.ClassListContains("root-container-full");

    public GameObject Player;
    [Range(-5, 5)]
    public float miniMultiplyerX = -1.1f;
    [Range(-5, 5)]
    public float fullMultiplyerX = -1.1f;

    [Range(-5, 5)]
    public float miniMultiplyerY = -1.6f;
    [Range(-5, 5)]
    public float fullMultiplyerY = -1.6f;

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
        var multiplierX = IsMapOpen ? fullMultiplyerX : miniMultiplyerX;
        var multiplierY = IsMapOpen ? fullMultiplyerY : miniMultiplyerY;

        _playerRepresentation.style.translate = new Translate((Player.transform.position.x * multiplierX),
        (Player.transform.position.z * -multiplierY), 0);

        _playerRepresentation.style.rotate = new Rotate(new Angle(Player.transform.rotation.eulerAngles.y));

        
        if (!IsMapOpen)
        {
            var clampWidth = _mapImage.worldBound.width / 2 -
                _mapContainer.worldBound.width / 2;
            var clampHeight = _mapImage.worldBound.height / 2 -
                _mapContainer.worldBound.height / 2;

            var xPos = Mathf.Clamp((Player.transform.position.x * multiplierX),
                -clampWidth, clampWidth);
            var yPos = Mathf.Clamp((Player.transform.position.z * -multiplierY),
                -clampHeight, clampHeight);

            Debug.Log("X: "+xPos+", "+yPos);

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
