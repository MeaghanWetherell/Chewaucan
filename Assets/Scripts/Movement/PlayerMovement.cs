using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] float moveSpeed = 5f;
    [SerializeField] float rotationSpeed = 4f;
    [SerializeField] float gravity = -2f; //this constanly move the player down, so isGrounded works correctly.
    [SerializeField] float jumpHeight = 2;
    [SerializeField] float maxStamina = 100f;
    [SerializeField] float currStamina = 100f;
    [SerializeField] float staminaDepletionRate = 10f;

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
        jumpRef.action.performed += (InputAction.CallbackContext context) => { surfaceSwimming = true; };
        jumpRef.action.canceled += (InputAction.CallbackContext context) => { surfaceSwimming = false; };
        sprintRef.action.performed += OnSprint;
    }

    private void OnDisable()
    {
        moveRef.action.performed -= OnMove;
        moveRef.action.canceled -= (InputAction.CallbackContext context) => { moveInput = Vector2.zero; };
        jumpRef.action.started -= JumpOnce;
        jumpRef.action.performed -= (InputAction.CallbackContext context) => { surfaceSwimming = true; };
        jumpRef.action.canceled -= (InputAction.CallbackContext context) => { surfaceSwimming = false; };
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
        float waterSurface = waterPosition.y;
        //Debug.Log("Water: " + waterSurface);
        if (surfaceSwimming)
        {
            if (this.transform.position.y < waterSurface)
            {
                controller.Move(Vector3.up * Time.deltaTime *swimSpeed);
            }
            else
            {
                float newY = Mathf.Sin(Time.time) * swimSpeed;
                Vector3 pos = this.transform.position;
                Vector3 newPos = new Vector3(0, newY, 0);
                Debug.Log("Swimming UP");
                controller.Move(newPos * Time.deltaTime);
            }
        }
        //regular swimming movement
        //Gets forward direction of the player, calculates distance to move, and moves the player accordingly.
        Vector3 forwardDir = this.transform.TransformDirection(Vector3.forward);
        float moveAmount = moveInput.y * moveSpeed;
        Vector3 movement = forwardDir * moveAmount;

        controller.Move(movement * Time.deltaTime); //forward movement

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
            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.down) * hit.distance, Color.black);
            GameObject objectHit = hit.collider.gameObject;

            //<1.1 is the distance if the player is standing on flat ground so any distance larger is likely standing on a slope
            if (hit.distance <= 1.1f)
            { 
                //Debug.Log("Distance: "+ hit.distance+" Raycast points towards ground");
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
