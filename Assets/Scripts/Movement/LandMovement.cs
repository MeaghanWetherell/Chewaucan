using Misc;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class LandMovement : MonoBehaviour
{
    //a lot of this should really be in PlayerMovementController, but I don't want to refactor Ellie's stuff
    public float moveSpeed = 5f;
    public float maxDistToGround = 1.15f;
    [SerializeField] float rotationSpeed = 4f;
    [SerializeField] float gravity = -2f; //this constanly move the player down, so isGrounded works correctly.
    [SerializeField] float jumpHeight = 2;
    public float maxStamina = 100f;
    public float currStamina = 100f;
    public float staminaDepletionRate = 10f;
    public GameObject cameraObj;
    public GameObject minimapCamObj;
    public Slider staminaUI;

    CharacterController _controller;
    Vector2 _moveInput;
    private float _rotateInput;
    Vector3 _verticalMovement;
    private const float Gravity = -9.18f;
    public float moveSpeedDefault;
    bool _grounded;
    bool _prevGrounded;
    private float moveSpeedMult;

    public MovementSoundEffects soundEffects;
    CheckGroundTexture _terrainTexture;

    public InputActionReference moveRef;
    public InputActionReference jumpRef;
    public InputActionReference sprintRef;
    public InputActionReference turnRef;

    private void InitializeValues()
    {
        _controller = GetComponent<CharacterController>();
        soundEffects = GetComponent<MovementSoundEffects>();
        _terrainTexture = GetComponent<CheckGroundTexture>();
        _verticalMovement = new Vector3(0f, gravity, 0f);
        _moveInput = Vector2.zero;
        moveSpeedDefault = moveSpeed;
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
        turnRef.action.performed += OnRot;
        turnRef.action.canceled += ZeroRot;
        moveRef.action.canceled += ZeroMove;
        jumpRef.action.started += OnJump;
        sprintRef.action.performed += OnSprint;
        sprintRef.action.canceled += OnSprint;
    }

    private void OnDisable()
    {
        moveRef.action.performed -= OnMove;
        moveRef.action.canceled -= ZeroMove;
        turnRef.action.performed -= OnRot;
        turnRef.action.canceled -= ZeroRot;
        jumpRef.action.started -= OnJump;
        sprintRef.action.performed -= OnSprint;
        sprintRef.action.canceled -= OnSprint;
    }

    private void ZeroMove(InputAction.CallbackContext context)
    {
        _moveInput = Vector2.zero;
    }
    
    private void ZeroRot(InputAction.CallbackContext context)
    {
        _rotateInput = 0;
    }

    //multiplies the movement speed multiplier by mult for the passed number of seconds
    public void ChangeMoveSpeedMultForTime(float mult, float time)
    {
        StartCoroutine(_ChangeMoveSpeedMultForTime(mult, time));
    }

    private IEnumerator _ChangeMoveSpeedMultForTime(float mult, float time)
    {
        moveSpeedMult *= mult;
        yield return new WaitForSeconds(time);
        moveSpeedMult *= 1/mult;
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
            soundEffects.PlayLandSound();
        }

        // keeps the player on the ground when not jumping
        if (_grounded && _verticalMovement.y < gravity)
        {
            _verticalMovement.y = gravity;
        }

        float rotateMovement = _rotateInput * rotationSpeed * moveSpeedMult; 
        this.transform.Rotate(0f, rotateMovement, 0f);

        //Gets forward direction of the player, calculates distance to move, and moves the player accordingly.
        Vector3 movement = (transform.forward * _moveInput.y) + (transform.right * _moveInput.x);
        movement *= (moveSpeed / movement.magnitude);
        movement *= moveSpeedMult;
        

        _controller.Move(movement * Time.deltaTime); //forward movement

        //moves the player downward to ensure isGrounded returns correctly
        _verticalMovement.y += Gravity * Time.deltaTime;
        _controller.Move(_verticalMovement * Time.deltaTime);

        if (_moveInput.y != 0 && _grounded)
        {
            soundEffects.PlayWalkingSound();
        }
        UpdateStamina();
    }

    private void UpdateStamina()
    {
        //determining when the player is sprinting and stopping sprinting when out of stamina
        if (!Mathf.Approximately(moveSpeed, moveSpeedDefault) && currStamina > 0 && _moveInput != Vector2.zero)
        {
            currStamina -= staminaDepletionRate * Time.deltaTime; //depletes stamina when sprinting
        }
        else
        {
            if (currStamina < 0) { currStamina = 0; }
            soundEffects.SetIsSprinting(false);
            moveSpeed = moveSpeedDefault;
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

    /* The following 4 functions are called as part of the action map. The PlayerInput component on the player sends 
     * messages to these function when the corresponding input actions are used (WASD, Space, left shift).
     */
    void OnMove(InputAction.CallbackContext context)
    {
        _moveInput = context.ReadValue<Vector2>();
    }
    
    void OnRot(InputAction.CallbackContext context)
    {
        _rotateInput = context.ReadValue<float>();
    }

    void OnJump(InputAction.CallbackContext context)
    {
        if (_grounded)
        {
            _verticalMovement.y += Mathf.Sqrt(jumpHeight * -3.0f * Gravity);
            soundEffects.PlayJumpSound();
        }
    }

    void OnSprint(InputAction.CallbackContext context)
    {
        bool sprint = context.performed;
        if (sprint && currStamina > 0)
        {
            moveSpeed = moveSpeed * 1.5f;
            soundEffects.SetIsSprinting(true);
        }
        else
        {
            moveSpeed = moveSpeedDefault;
            soundEffects.SetIsSprinting(false);
        }
    }
}
