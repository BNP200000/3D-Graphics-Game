using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    [SerializeField] float walkSpeed = 6f;
    [SerializeField] float runSpeed = 12f;
    [SerializeField] float crouchSpeed = 3f;
    
    float currSpeed;    
    Vector2 moveInput;
    PlayerInput input;
    Rigidbody rb;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        input = GetComponent<PlayerInput>();
        currSpeed = walkSpeed;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        moveInput = input.actions["Move"].ReadValue<Vector2>();
        HandleSpeed();
        Move();
    }

    void Move()
    {
        

        Vector3 playerVelocity = new Vector3(
            moveInput.x * currSpeed,
            rb.linearVelocity.y,
            moveInput.y * currSpeed
        );
        rb.linearVelocity = transform.TransformDirection(playerVelocity);
    }

    void HandleSpeed() 
    {
        if(input.actions["Crouch"].IsPressed()) 
        {
            Debug.Log("Crouching");
            currSpeed = crouchSpeed;
        }
        else if(input.actions["Sprint"].IsPressed())
        {
            Debug.Log("Sprinting");
            currSpeed = runSpeed;
        }
        else if(moveInput.magnitude > 0)
        {
            Debug.Log("Walking");
            currSpeed = walkSpeed;
        }
    }
}
