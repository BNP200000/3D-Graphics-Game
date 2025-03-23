using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    [SerializeField] float walkSpeed = 6f;
    [SerializeField] float runSpeed = 12f;
    [SerializeField] float crouchSpeed = 2f;
    [SerializeField] Material normal, stealth;
    
    float currSpeed;    
    Vector2 moveInput;
    PlayerInput input;
    Rigidbody rb;
    public enum PlayerState {Walk, Run, Crouch};
    public PlayerState state {get; private set;}

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
        Move();
    }

    void Move()
    {
        HandleSpeed();
        Vector3 playerVelocity = new Vector3(
            moveInput.x * currSpeed,
            rb.linearVelocity.y,
            moveInput.y * currSpeed
        );
        rb.linearVelocity = transform.TransformDirection(playerVelocity);
    }

    void HandleSpeed() 
    {
        if(input.actions["Move"].IsPressed()) 
        {
            Debug.Log("Walking");
            ChangeBody(normal);
            currSpeed = walkSpeed;
            state = PlayerState.Walk;
        }   
        if(input.actions["Crouch"].IsPressed()) 
        {
            Debug.Log("Crouching");
            ChangeBody(stealth);
            currSpeed = crouchSpeed;
            state = PlayerState.Crouch;
        }
        else if(input.actions["Sprint"].IsPressed())
        {
            Debug.Log("Sprinting");
            ChangeBody(normal);
            currSpeed = runSpeed;
            state = PlayerState.Run;
            
        }
    }

    void ChangeBody(Material mat)
    {
        GetComponent<MeshRenderer>().material = mat;
    }
}
