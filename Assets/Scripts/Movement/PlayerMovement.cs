using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;

public class PlayerMovement : MonoBehaviour
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

    CharacterController controller;
    Vector2 moveInput;
    Vector3 verticalMovement;
    private const float GRAVITY = -9.18f;
    float moveSpeedDefault;
    bool grounded;
    bool prevGrounded;

    bool isSwimming;
    bool surfaceSwimming;
    Vector3 waterPosition;
    float swimSpeed = 3f;

    MovementSoundEffects soundEffects;
    CheckGroundTexture terrainTexture;

    public InputActionReference moveRef;
    public InputActionReference jumpRef;
    public InputActionReference sprintRef;
    
    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
        soundEffects = GetComponent<MovementSoundEffects>();
        terrainTexture = GetComponent<CheckGroundTexture>();
        verticalMovement = new Vector3(0f, gravity, 0f);
        moveInput = Vector2.zero;
        moveSpeedDefault = moveSpeed;
        isSwimming = false;
        
    }

    private void OnEnable()
    {
        moveRef.action.performed += OnMove;
        moveRef.action.canceled += (InputAction.CallbackContext context) => { moveInput = Vector2.zero; };
        jumpRef.action.started += JumpOnce;
        sprintRef.action.performed += OnSprint;
    }

    private void OnDisable()
    {
        moveRef.action.performed -= OnMove;
        moveRef.action.canceled -= (InputAction.CallbackContext context) => { moveInput = Vector2.zero; };
        jumpRef.action.started -= JumpOnce;
        sprintRef.action.performed -= OnSprint;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isSwimming)
        {
            LandMovement();
        }
        else
        {
            SwimMovement();
        }
    }

    private void LandMovement()
    {
        terrainTexture.GetGroundTexture();
        prevGrounded = grounded;
        grounded = controller.isGrounded && RaycastToGround(); //checks if the player if standing on the ground

        if (!prevGrounded && grounded)
        {
            soundEffects.PlayLandSound();
        }

        if (grounded && verticalMovement.y < gravity)
        {
            verticalMovement.y = gravity;
        }

        float rotateMovement = moveInput.x * rotationSpeed; //AD rotation uses x value of the vector from OnMove
        this.transform.Rotate(0f, rotateMovement, 0f);

        //Gets forward direction of the player, calculates distance to move, and moves the player accordingly.
        Vector3 forwardDir = this.transform.TransformDirection(Vector3.forward);
        float moveAmount = moveInput.y * moveSpeed;
        Vector3 movement = forwardDir * moveAmount;

        controller.Move(movement * Time.deltaTime); //forward movement

        //moves the player downward to ensure isGrounded returns correctly
        verticalMovement.y += GRAVITY * Time.deltaTime;
        controller.Move(verticalMovement * Time.deltaTime);

        UpdateStamina();

        if (moveInput.y != 0 && grounded)
        {
            soundEffects.PlayWalkingSound();
        }
    }

    private void SwimMovement()
    {
        swimSpeed = moveSpeed * 0.75f;
        float waterSurface = waterPosition.y;

        //regular swimming movement, like land movement but uses camera forward instead of the player forward

        float rotateMovement = moveInput.x * rotationSpeed; //AD rotation uses x value of the vector from OnMove
        this.transform.Rotate(0f, rotateMovement, 0f);

        //Gets forward direction of the player, calculates distance to move, and moves the player accordingly.
        Vector3 forwardDir = cameraObj.transform.TransformDirection(Vector3.forward);
        float moveAmount = moveInput.y * swimSpeed;
        Vector3 movement = forwardDir * moveAmount;

        controller.Move(movement * Time.deltaTime); //forward movement

        if (this.transform.position.y > waterSurface)
        {
            controller.Move(Vector3.down * swimSpeed * Time.deltaTime);
        }

        UpdateStamina();
    }

    /**
     * Called every frame, updates the current stamina level
     * Either recovering stamina when not sprinting or losing stamina when sprinting
     */
    private void UpdateStamina()
    {
        //determining when the player is sprinting and stopping sprinting when out of stamina
        if (moveSpeed != moveSpeedDefault && currStamina > 0)
        {
            currStamina -= staminaDepletionRate * Time.deltaTime; //depletes stamina when sprinting
        }
        else
        {
            if (currStamina < 0) { currStamina = 0; }
            soundEffects.setPlaySpeed(0.9f);
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
    }

    /**
     * This function acts as a secondary check for telling when the player is grounded, to ensure they cannot jump up
     * walls. 
     */
    private bool RaycastToGround()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.down), out hit, controller.height))
        {
            GameObject objectHit = hit.collider.gameObject;
            //Debug.Log("Distance: " + hit.distance + " Raycast points towards ground");
            //<1.1 is the distance if the player is standing on flat ground so any distance larger is likely standing on a slope
            if (hit.distance <= maxDistToGround)
            { 
                return true;
            }
            
        }
        return false;
    }

    public void setSwimming(bool swim, Vector3 waterLevel)
    {
        isSwimming = swim;
        waterPosition = waterLevel;
    }

    /* The following 3 functions are called as part of the action map. The PlayerInput component on the player sends 
     * messages to these function when the corresponding input actions are used (WASD, Space, left shift).
     */
    void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }

    void JumpOnce(InputAction.CallbackContext context)
    {
        if (grounded && !isSwimming)
        {
            verticalMovement.y += Mathf.Sqrt(jumpHeight * -3.0f * GRAVITY);
            soundEffects.PlayJumpSound();
        }
    }

    void OnSprint(InputAction.CallbackContext context)
    {
        bool sprint = context.performed;
        if (sprint && currStamina > 0)
        {
            moveSpeed = moveSpeed * 1.5f;
            soundEffects.setPlaySpeed(0.5f);
        }
        else
        {
            moveSpeed = moveSpeedDefault;
            soundEffects.setPlaySpeed(0.9f);
        }
    }
}
