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
    [NonSerialized]public Quaternion targRot;

    [NonSerialized] public Collider curCollider;

    public float rotSpeed;

    public LandMovement landMovement;

    public CharacterController controller;
    Vector2 _moveInput;

    public InputActionReference moveRef;
    public InputActionReference sprintRef;
    
    
    private void OnEnable()
    {
        StartCoroutine(RotateToSurface());
    }

    private void OnDisable()
    {
        moveRef.action.performed -= OnMove;
        moveRef.action.canceled -= (InputAction.CallbackContext context) => { _moveInput = Vector2.zero; };
        sprintRef.action.performed -= OnSprint;
        sprintRef.action.canceled -= OnSprint;
    }

    private void Update()
    {
        Vector3 movement = transform.right*_moveInput.x;
        bool grounded = controller.isGrounded && RaycastToGround(); //checks if the player if standing on the ground

        if (transform.position.y > curCollider.bounds.max.y)
        {
            if (_moveInput.y > 0)
            {
                movement += transform.forward * _moveInput.y;
            }
            else
            {
                movement += transform.up * _moveInput.y;
            }
        }
        else if (grounded)
        {
            if (_moveInput.y > 0)
            {
                movement += transform.up * _moveInput.y;
            }
            else
            {
                movement += transform.forward * _moveInput.y;
            }
                
        }
        else
        {
            movement += transform.up * _moveInput.y;
        }

        controller.Move(movement * Time.deltaTime); //forward movement

        if (_moveInput.y != 0)
        {
            landMovement.soundEffects.PlayWalkingSound();
        }
        UpdateStamina();
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
        if (!Mathf.Approximately(landMovement.moveSpeed, landMovement.moveSpeedDefault) && landMovement.currStamina > 0 && _moveInput != Vector2.zero)
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
        _moveInput = context.ReadValue<Vector2>();
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

    private IEnumerator RotateToSurface()
    {
        float x = targRot.eulerAngles.x - transform.eulerAngles.x;
        float z = targRot.eulerAngles.z - transform.eulerAngles.z;
        while (Mathf.Abs(x)+Mathf.Abs(z)>6f)
        {
            if (x > 3f)
            {
                transform.Rotate(Vector3.left, rotSpeed*0.05f);
            }

            else if (x < -3f)
            {
                transform.Rotate(Vector3.right, rotSpeed*0.05f);
            }

            if (z > 3f)
            {
                transform.Rotate(Vector3.forward, rotSpeed*0.05f);
            }
            else if (z < -3f)
            {
                transform.Rotate(Vector3.back, rotSpeed*0.05f);
            }
            
            x = targRot.eulerAngles.x - transform.eulerAngles.x;
            z = targRot.eulerAngles.z - transform.eulerAngles.z;
            yield return new WaitForSeconds(0.05f);
        }

        moveRef.action.performed += OnMove;
        moveRef.action.canceled += (InputAction.CallbackContext context) => { _moveInput = Vector2.zero; };
        sprintRef.action.performed += OnSprint;
        sprintRef.action.canceled += OnSprint;
    }
}
