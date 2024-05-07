using Misc;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

/**
 * Swimming movement behavior
 * This script is attached to the player prefab, and enabled when the player
 * moves into water
 * The enabling of this occurs in StartSwimming, which is attached to bodies of water 
 * the player can swim in
 */
public class SwimmingMovement : MonoBehaviour
{
    [SerializeField] float moveSpeed = 5f;
    [SerializeField] float rotationSpeed = 4f;
    [SerializeField] float maxStamina = 100f;
    [SerializeField] float currStamina = 100f;
    [SerializeField] float staminaDepletionRate = 10f;
    [SerializeField] float oxygenDepletionRate = 4f;
    [SerializeField] float currOxygen = 100f;
    public GameObject cameraObj;
    public GameObject minimapCamObj;
    public Slider staminaUI;
    public Slider oxygenUI;

    CharacterController _controller;
    Vector2 _moveInput;
    float _moveSpeedDefault;

    bool _isDiving;
    bool _dive;
    Vector3 _waterPosition;
    float _swimSpeed = 3f;
    AudioSource _waterAudio;

    MovementSoundEffects _soundEffects;

    public InputActionReference moveRef;
    public InputActionReference jumpRef;
    public InputActionReference sprintRef;

    private ActiveSoundManager ambientSoundManager;

    private void InitializeValues()
    {
        _controller = GetComponent<CharacterController>();
        _soundEffects = GetComponent<MovementSoundEffects>();
        _moveInput = Vector2.zero;
        _moveSpeedDefault = moveSpeed;
        staminaUI.minValue = 0f;
        staminaUI.maxValue = maxStamina;
        oxygenUI.minValue = 0f;
        oxygenUI.maxValue = maxStamina;
    }

    // Start is called before the first frame update
    void Start()
    {
        InitializeValues();
        ambientSoundManager = FindFirstObjectByType<ActiveSoundManager>();
    }

    private void OnEnable()
    {
        InitializeValues();
        oxygenUI.gameObject.SetActive(true);

        moveRef.action.performed += OnMove;
        moveRef.action.canceled += (InputAction.CallbackContext context) => { _moveInput = Vector2.zero; };
        jumpRef.action.started += OnJump;
        sprintRef.action.performed += OnSprint;
        sprintRef.action.canceled += OnSprint;
    }

    private void OnDisable()
    {
        if (!oxygenUI.IsDestroyed())
        {
            oxygenUI.gameObject.SetActive(false);
        }

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
        Swim();
    }

    private void Swim()
    {
        _swimSpeed = moveSpeed * 0.75f;
        float waterSurface = _waterPosition.y;

        if (this.transform.position.y >= waterSurface && _isDiving && !_dive)
        {
            _waterAudio.Stop();
            ambientSoundManager.EnableAmbientSounds();
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

    // updates the UI of the blue oxygen bar
    // decreases constantly if underwater, recovers if above water
    private void UpdateOxygen()
    {
        if (currOxygen < 0) //brings player to the water surface when oxygen if out
        {
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

    /* The following 3 functions are called as part of the action map. The PlayerInput component on the player sends 
     * messages to these function when the corresponding input actions are used (WASD, Space, left shift).
     */
    void OnMove(InputAction.CallbackContext context)
    {
        _moveInput = context.ReadValue<Vector2>();
    }

    void OnJump(InputAction.CallbackContext context)
    {
        if (!_isDiving)
        {
            _waterAudio.Play();
            _isDiving = true;
            if (ambientSoundManager != null)
            {
                ambientSoundManager.DisableAmbientSounds();
            }
            StartCoroutine(Dive());
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

    /**
     * Coroutine that similates the act of diving below the water.
     * The player cannot move while this is running. This coroutine
     * rotates the player downward, uses the CharacterController to 
     * move them into the water, then rotates them forward again.
     */
    IEnumerator Dive()
    {
        _dive = true;
        Vector3 rot = new Vector3(0f, transform.rotation.eulerAngles.y, 0f);
        transform.rotation = Quaternion.Euler(rot);
        
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
        transform.rotation = Quaternion.Euler(rot); //sets rotation back to default
        _dive = false;
        yield return null;
    }

    // checks if the Dive coroutine is currently running
    public bool DiveOngoing()
    {
        return _dive;
    }

    public void SetWaterSoundSource(AudioSource audioSource)
    {
        _waterAudio = audioSource;
    }

    public void SetSwimming(bool swim, Vector3 waterLevel)
    {
        _waterPosition = waterLevel;
    }

}
