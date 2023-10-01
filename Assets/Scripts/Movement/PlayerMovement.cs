using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] float moveSpeed = 5f;
    [SerializeField] float rotationSpeed = 5f;
    [SerializeField] float gravity = -2f; //this constanly move the player down, so isGrounded works correctly.
    [SerializeField] float jumpHeight = 2;

    CharacterController controller;
    Vector2 moveInput;
    Vector3 verticalMovement;
    float constantGravity = -9.18f;
    float moveSpeedDefault;
    
    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
        verticalMovement = new Vector3(0f, gravity, 0f);
        moveSpeedDefault = moveSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        bool grounded = controller.isGrounded; //checks if the player if standing on the ground

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

        controller.Move(movement * Time.deltaTime);

        //moves the player downward to ensure isGrounded returns correctly
        verticalMovement.y += constantGravity * Time.deltaTime;
        controller.Move(verticalMovement * Time.deltaTime);
        
    }

    /* The following 3 functions are called as part of the action map. The PlayerInput component on the player sends 
     * messages to these function when the corresponding input actions are used (WASD, Space, left shift).
     */
    void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
    }

    void OnJump()
    {
        if (controller.isGrounded)
        {
            verticalMovement.y += Mathf.Sqrt(jumpHeight * -3.0f * constantGravity);
        }
    }

    void OnSprint(InputValue value)
    {
        bool sprint = value.isPressed;
        if (sprint)
        {
            moveSpeed = moveSpeed * 1.5f;
        }
        else
        {
            moveSpeed = moveSpeedDefault;
        }
    }
}
