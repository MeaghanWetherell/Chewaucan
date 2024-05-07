using Misc;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class LandMovement : MonoBehaviour
{
    [SerializeField] float moveSpeed = 5f;
    [SerializeField] float maxDistToGround = 1.15f;
    [SerializeField] float rotationSpeed = 4f;
    [SerializeField] float gravity = -2f; //this constanly move the player down, so isGrounded works correctly.
    [SerializeField] float jumpHeight = 2;
    [SerializeField] float maxStamina = 100f;
    [SerializeField] float currStamina = 100f;
    [SerializeField] float staminaDepletionRate = 10f;
    public GameObject cameraObj;
    public GameObject minimapCamObj;
    public Slider staminaUI;

    CharacterController _controller;
    Vector2 _moveInput;
    Vector3 _verticalMovement;
    private const float Gravity = -9.18f;
    float _moveSpeedDefault;
    bool _grounded;
    bool _prevGrounded;

    MovementSoundEffects _soundEffects;
    CheckGroundTexture _terrainTexture;

    public InputActionReference moveRef;
    public InputActionReference jumpRef;
    public InputActionReference sprintRef;

    private void InitializeValues()
    {
        _controller = GetComponent<CharacterController>();
        _soundEffects = GetComponent<MovementSoundEffects>();
        _terrainTexture = GetComponent<CheckGroundTexture>();
        _verticalMovement = new Vector3(0f, gravity, 0f);
        _moveInput = Vector2.zero;
        _moveSpeedDefault = moveSpeed;
        staminaUI.minValue = 0f;
        staminaUI.maxValue = maxStamina;
    }

    // Start is called before the first frame update
    void Start()
    {
        InitializeValues();
    }

    private void OnEnable()
    {
        InitializeValues();

        moveRef.action.performed += OnMove;
        moveRef.action.canceled += (InputAction.CallbackContext context) => { _moveInput = Vector2.zero; };
        jumpRef.action.started += OnJump;
        sprintRef.action.performed += OnSprint;
        sprintRef.action.canceled += OnSprint;
    }

    private void OnDisable()
    {
        moveRef.action.performed -= OnMove;
        moveRef.action.canceled -= (InputAction.CallbackContext context) => { _moveInput = Vector2.zero; };
        jumpRef.action.started -= OnJump;
        sprintRef.action.performed -= OnSprint;
        sprintRef.action.canceled -= OnSprint;
    }

    // Update is called once per frame
    void Update()
    {
        minimapCamObj.transform.rotation = Quaternion.Euler(90f, 0f, 0f);
        Move();
    }

    private void Move()
    {
        _terrainTexture.GetGroundTexture(); // get texture on the ground to know what sound type to play

        _prevGrounded = _grounded;
        _grounded = _controller.isGrounded && RaycastToGround(); //checks if the player if standing on the ground

        // play a landing sound if we have just landed
        if (!_prevGrounded && _grounded)
        {
            _soundEffects.PlayLandSound();
        }

        // keeps the player on the ground when not jumping
        if (_grounded && _verticalMovement.y < gravity)
        {
            _verticalMovement.y = gravity;
        }

        float rotateMovement = _moveInput.x * rotationSpeed; //AD rotation uses x value of the vector from OnMove
        this.transform.Rotate(0f, rotateMovement, 0f);

        //Gets forward direction of the player, calculates distance to move, and moves the player accordingly.
        Vector3 forwardDir = this.transform.TransformDirection(Vector3.forward);
        float moveAmount = _moveInput.y * moveSpeed;
        Vector3 movement = forwardDir * moveAmount;

        _controller.Move(movement * Time.deltaTime); //forward movement

        //moves the player downward to ensure isGrounded returns correctly
        _verticalMovement.y += Gravity * Time.deltaTime;
        _controller.Move(_verticalMovement * Time.deltaTime);

        if (_moveInput.y != 0 && _grounded)
        {
            _soundEffects.PlayWalkingSound();
        }
        UpdateStamina();
    }

    private void UpdateStamina()
    {
        //determining when the player is sprinting and stopping sprinting when out of stamina
        if (moveSpeed != _moveSpeedDefault && currStamina > 0 && _moveInput != Vector2.zero)
        {
            currStamina -= staminaDepletionRate * Time.deltaTime; //depletes stamina when sprinting
        }
        else
        {
            if (currStamina < 0) { currStamina = 0; }
            _soundEffects.SetIsSprinting(false);
            moveSpeed = _moveSpeedDefault;
            if (currStamina < maxStamina)
            {
                currStamina += staminaDepletionRate * Time.deltaTime; //restores stamina when not sprinting, up to maxStamina
            }
            else
            {
                currStamina = maxStamina;
            }
        }
        staminaUI.value = currStamina;
    }

    // calculates our distance to the ground as an extra check along with controller.isGrounded in line 76
    // ensures that the player cannot jump up steep slopes
    private bool RaycastToGround()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.down), out hit, _controller.height))
        {
            GameObject objectHit = hit.collider.gameObject;

            //<1.1 is the distance if the player is standing on flat ground so any distance larger is likely standing on a slope
            if (hit.distance <= maxDistToGround)
            {
                return true;
            }

        }
        return false;
    }

    /* The following 3 functions are called as part of the action map. The PlayerInput component on the player sends 
     * messages to these function when the corresponding input actions are used (WASD, Space, left shift).
     */
    void OnMove(InputAction.CallbackContext context)
    {
        _moveInput = context.ReadValue<Vector2>();
    }

    void OnJump(InputAction.CallbackContext context)
    {
        if (_grounded)
        {
            _verticalMovement.y += Mathf.Sqrt(jumpHeight * -3.0f * Gravity);
            _soundEffects.PlayJumpSound();
        }
    }

    void OnSprint(InputAction.CallbackContext context)
    {
        bool sprint = context.performed;
        if (sprint && currStamina > 0)
        {
            moveSpeed = moveSpeed * 1.5f;
            _soundEffects.SetIsSprinting(true);
        }
        else
        {
            moveSpeed = _moveSpeedDefault;
            _soundEffects.SetIsSprinting(false);
        }
    }
}
