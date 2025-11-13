using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;
using Quaternion = UnityEngine.Quaternion;
using Vector3 = UnityEngine.Vector3;
using Vector2 = UnityEngine.Vector2;

public class ClimbingMovement : MonoBehaviour
{
    [NonSerialized] public Collider curCollider;

    public float rotSpeed;

    public LandMovement landMovement;

    public CharacterController controller;
    public Vector2 moveInput;

    public InputActionReference moveRef;
    public InputActionReference sprintRef;

    private Vector3 targetNormal;
    
    public void OnEnable()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, 3, LayerMask.GetMask("Climbable")))
        {
            targetNormal = -hit.normal;
            CameraLook look = GetComponentInChildren<CameraLook>();
            look.clampY = true;
            look.clampYCenter = targetNormal.y;
        }

        StartCoroutine(rotate());
        moveRef.action.performed += OnMove;
        moveRef.action.canceled += ZeroMove;
        sprintRef.action.performed += OnSprint;
        sprintRef.action.canceled += OnSprint;
    }

    private void OnDisable()
    {
        StopAllCoroutines();
        GetComponentInChildren<CameraLook>().clampY = false;
        
        moveRef.action.performed -= OnMove;
        moveRef.action.canceled -= ZeroMove;
        sprintRef.action.performed -= OnSprint;
        sprintRef.action.canceled -= OnSprint;
    }

    private IEnumerator rotate()
    {
        Quaternion targetRotation = Quaternion.LookRotation(targetNormal, Vector3.up);
        while (!rotApproximatelyEqual(targetRotation, transform.rotation))
        {
            targetRotation = Quaternion.LookRotation(targetNormal, Vector3.up);

            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, 0.05f * rotSpeed);
            yield return new WaitForSeconds(0.05f);
        }
    }

    private bool rotApproximatelyEqual(Quaternion q1, Quaternion q2)
    {
        float tolerance = 0.5f;
        
        Vector3 v1 = q1.eulerAngles;
        Vector3 v2 = q2.eulerAngles;

        if (Mathf.Abs(v1.x - v2.x) < tolerance &&
            Mathf.Abs(v1.y - v2.y) < tolerance &&
            Mathf.Abs(v1.z - v2.z) < tolerance)
        {
            return true;
        }

        return false;
    }

    private void Update()
    {
        Vector3 movement = transform.right*moveInput.x;
        bool grounded = controller.isGrounded && RaycastToGround(); //checks if the player if standing on the ground

        if (transform.position.y > curCollider.bounds.max.y)
        {
            if (moveInput.y > 0)
            {
                movement += transform.forward * moveInput.y;
            }
            else
            {
                movement += transform.up * moveInput.y;
            }
        }
        else if (grounded)
        {
            if (moveInput.y > 0)
            {
                movement += transform.up * moveInput.y;
            }
            else
            {
                movement += transform.forward * moveInput.y;
            }
                
        }
        else
        {
            
            movement += transform.up * moveInput.y;
        }
        
        

        controller.Move(movement * Time.deltaTime); //forward movement

        if (moveInput.y != 0)
        {
            landMovement.soundEffects.PlayWalkingSound();
        }
        UpdateStamina();
    }
    
    private void ZeroMove(InputAction.CallbackContext context)
    {
        moveInput = Vector2.zero;
    }

    // calculates our distance to the ground as an extra check along with controller.isGrounded in line 76
    // ensures that the player cannot jump up steep slopes
    private bool RaycastToGround()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.down), out hit, controller.height))
        {
            GameObject objectHit = hit.collider.gameObject;

            //<1.1 is the distance if the player is standing on flat ground so any distance larger is likely standing on a slope
            if (hit.distance <= landMovement.maxDistToGround)
            {
                return true;
            }

        }
        return false;
    }
    private void UpdateStamina()
    {
        //determining when the player is sprinting and stopping sprinting when out of stamina
        if (!Mathf.Approximately(landMovement.moveSpeed, landMovement.moveSpeedDefault) && landMovement.currStamina > 0 && moveInput != Vector2.zero)
        {
            landMovement.currStamina -= landMovement.staminaDepletionRate * Time.deltaTime; //depletes stamina when sprinting
        }
        else
        {
            if (landMovement.currStamina < 0) { landMovement.currStamina = 0; }
            landMovement.soundEffects.SetIsSprinting(false);
            landMovement.moveSpeed = landMovement.moveSpeedDefault;
            if (landMovement.currStamina < landMovement.maxStamina)
            {
                landMovement.currStamina += landMovement.staminaDepletionRate * Time.deltaTime; //restores stamina when not sprinting, up to maxStamina
            }
            else
            {
                landMovement.currStamina = landMovement.maxStamina;
            }
        }
        landMovement.staminaUI.value = landMovement.currStamina;
    }
    
    /* The following functions are called as part of the action map. The PlayerInput component on the player sends
     * messages to these function when the corresponding input actions are used (WASD, left shift).
     */
    void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }

    void OnSprint(InputAction.CallbackContext context)
    {
        bool sprint = context.performed;
        if (sprint && landMovement.currStamina > 0)
        {
            landMovement.moveSpeed = landMovement.moveSpeed * 1.5f;
            landMovement.soundEffects.SetIsSprinting(true);
        }
        else
        {
            landMovement.moveSpeed = landMovement.moveSpeedDefault;
            landMovement.soundEffects.SetIsSprinting(false);
        }
    }
}
