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
    bool grounded;
    
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
        grounded = controller.isGrounded && RaycastToGround(); //checks if the player if standing on the ground

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

    /**
     * This function acts as a secondary check for telling when the player is grounded, to ensure they cannot jump up
     * walls. With this, it is considerably harder to jump up walls, but still possible with some effort.
     */
    private bool RaycastToGround()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.down), out hit, controller.height))
        {
            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.down) * hit.distance, Color.black);
            GameObject objectHit = hit.collider.gameObject;
            
            //Terrain hasTerrain = objectHit.GetComponent<Terrain>();
            // <1.1 is the distance if the player is standing on flat ground so any distance larger is likely standing on a slope
            if (hit.distance <= 1.1f)
            { 
                Debug.Log("Distance: "+ hit.distance+" Raycast points towards ground");
                return true;
            }
            
        }
        return false;
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
        if (grounded)
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
