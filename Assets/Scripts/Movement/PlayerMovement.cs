using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;
using UnityEngine.UI;

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
    [SerializeField] float oxygenDepletionRate = 4f;
    [SerializeField] float currOxygen = 100f;
    public GameObject cameraObj;
    public Slider staminaUI;
    public Slider oxygenUI;

    CharacterController _controller;
    Vector2 _moveInput;
    Vector3 _verticalMovement;
    private const float Gravity = -9.18f;
    float _moveSpeedDefault;
    bool _grounded;
    bool _prevGrounded;

    bool _isSwimming;
    bool _isDiving;
    bool _dive;
    Vector3 _waterPosition;
    float _swimSpeed = 3f;
    AudioSource _waterAudio;

    MovementSoundEffects _soundEffects;
    CheckGroundTexture _terrainTexture;

    public InputActionReference moveRef;
    public InputActionReference jumpRef;
    public InputActionReference sprintRef;
    
    // Start is called before the first frame update
    void Start()
    {
        _controller = GetComponent<CharacterController>();
        _soundEffects = GetComponent<MovementSoundEffects>();
        _terrainTexture = GetComponent<CheckGroundTexture>();
        _verticalMovement = new Vector3(0f, gravity, 0f);
        _moveInput = Vector2.zero;
        _moveSpeedDefault = moveSpeed;
        staminaUI.minValue = 0f;
        staminaUI.maxValue = maxStamina;
        oxygenUI.minValue = 0f;
        oxygenUI.maxValue = maxStamina;
        
    }

    private void OnEnable()
    {
        moveRef.action.performed += OnMove;
        moveRef.action.canceled += (InputAction.CallbackContext context) => { _moveInput = Vector2.zero; };
        jumpRef.action.started += JumpOnce;
        sprintRef.action.performed += OnSprint;
    }

    private void OnDisable()
    {
        moveRef.action.performed -= OnMove;
        moveRef.action.canceled -= (InputAction.CallbackContext context) => { _moveInput = Vector2.zero; };
        jumpRef.action.started -= JumpOnce;
        sprintRef.action.performed -= OnSprint;
    }

    // Update is called once per frame
    void Update()
    {
        if (!_isSwimming)
        {
            oxygenUI.gameObject.SetActive(false);
            LandMovement();
        }
        else if (_isSwimming && !_dive)
        {
            oxygenUI.gameObject.SetActive(true);
            SwimMovement();
        }
    }

    private void LandMovement()
    {
        _terrainTexture.GetGroundTexture();
        _prevGrounded = _grounded;
        _grounded = _controller.isGrounded && RaycastToGround(); //checks if the player if standing on the ground

        if (!_prevGrounded && _grounded)
        {
            _soundEffects.PlayLandSound();
        }

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

    private void SwimMovement()
    {
        _swimSpeed = moveSpeed * 0.75f;
        float waterSurface = _waterPosition.y;

        if (this.transform.position.y >= waterSurface && _isDiving && !_dive)
        {
            _waterAudio.Stop();
            _isDiving = false;
        }

        //regular swimming movement, like land movement but uses camera forward instead of the player forward

        float rotateMovement = _moveInput.x * rotationSpeed; //AD rotation uses x value of the vector from OnMove
        this.transform.Rotate(0f, rotateMovement, 0f);

        //Gets forward direction of the player, calculates distance to move, and moves the player accordingly.
        //Uses the camera forward direction if underwater, otherwise uses the normal player transform forward
        Vector3 forwardDir = this.transform.TransformDirection(Vector3.forward);
        if (!_isDiving)
        {
            forwardDir = this.transform.TransformDirection(Vector3.forward);
            this.GetComponent<CameraLook>().SetMinDist(10f);
        }
        else
        {
            this.GetComponent<CameraLook>().SetMinDist(50f);
            forwardDir = cameraObj.transform.TransformDirection(Vector3.forward);
            _controller.Move(Vector3.up * Time.deltaTime);
        }
        float moveAmount = _moveInput.y * _swimSpeed;
        Vector3 movement = forwardDir * moveAmount;

        _controller.Move(movement * Time.deltaTime); //forward movement
        
        UpdateStamina();
        UpdateOxygen();
        if (_moveInput != Vector2.zero)
        {
            _soundEffects.PlaySwimSound(); //plays swimming sounds if moving
        }
    }

    /**
     * Called every frame, updates the current stamina level
     * Either recovering stamina when not sprinting or losing stamina when sprinting
     */
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
            _soundEffects.SetPlaySpeed(0.9f);
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

    private void UpdateOxygen()
    {
        if (currOxygen < 0) { 
            currOxygen = 0;
            this.transform.rotation.eulerAngles.Set(0f, transform.rotation.eulerAngles.y, 0f);
            this.transform.position = new Vector3(transform.position.x, _waterPosition.y, transform.position.z);
        }
        if (currOxygen < maxStamina && !_isDiving)
        {
            currOxygen += oxygenDepletionRate * Time.deltaTime;
        }
        else if (_isDiving)
        {
            currOxygen -= oxygenDepletionRate * Time.deltaTime;
        }
        else
        {
            currOxygen = maxStamina;
        }
        oxygenUI.value = currOxygen;
    }

    /**
     * This function acts as a secondary check for telling when the player is grounded, to ensure they cannot jump up
     * walls. 
     */
    private bool RaycastToGround()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.down), out hit, _controller.height))
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

    public void SetSwimming(bool swim, Vector3 waterLevel)
    {
        _isSwimming = swim;
        _waterPosition = waterLevel;
    }

    /* The following 3 functions are called as part of the action map. The PlayerInput component on the player sends 
     * messages to these function when the corresponding input actions are used (WASD, Space, left shift).
     */
    void OnMove(InputAction.CallbackContext context)
    {
        _moveInput = context.ReadValue<Vector2>();
    }

    void JumpOnce(InputAction.CallbackContext context)
    {
        if (_grounded && !_isSwimming)
        {
            _verticalMovement.y += Mathf.Sqrt(jumpHeight * -3.0f * Gravity);
            _soundEffects.PlayJumpSound();
        }
        else if (_isSwimming && !_isDiving)
        {
            _waterAudio.Play();
            _isDiving = true;
            StartCoroutine(Dive());
        }
    }

    void OnSprint(InputAction.CallbackContext context)
    {
        bool sprint = context.performed;
        if (sprint && currStamina > 0)
        {
            moveSpeed = moveSpeed * 1.5f;
            _soundEffects.SetPlaySpeed(0.5f);
        }
        else
        {
            moveSpeed = _moveSpeedDefault;
            _soundEffects.SetPlaySpeed(0.9f);
        }
    }

    /*
     * Moves the player under the water surface
     */
    IEnumerator Dive()
    {
        _dive = true;
        Vector3 rot = new Vector3(0f, transform.rotation.eulerAngles.y, 0f);
        transform.rotation = Quaternion.Euler(rot);
        //yield return new WaitForSeconds(3f);
        for (int i = 0; i < 15; i++)
        {
            this.transform.Rotate(2f, 0f, 0f);
            yield return new WaitForEndOfFrame();
        }
        for (int i = 0; i < 10; i++)
        {
            _controller.Move(this.transform.TransformDirection(Vector3.forward));
            yield return new WaitForEndOfFrame();
        }
        for (int i = 0; i < 15; i++)
        {
            this.transform.Rotate(-2f, 0f, 0f);
            yield return new WaitForEndOfFrame();
        }
        transform.rotation = Quaternion.Euler(rot);
        _dive = false;
        yield return null;
    }

    public void SetWaterSoundSource(AudioSource audioSource)
    {
        _waterAudio = audioSource;
    }

    public bool DiveOngoing()
    {
        return _dive;
    }
}
