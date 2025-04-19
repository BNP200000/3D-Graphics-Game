using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    [SerializeField] float walkSpeed = 6f; // Player's normal speed
    [SerializeField] float runSpeed = 12f; // Player's running speed
    [SerializeField] float crouchSpeed = 2f; // Player's crouching speed
    public int maxHealth {get; set;} = 3; // The amount of lives given to the player
    public Health bar; // A reference to the Player Health Bar
    [SerializeField] Material normal, stealth; // A reference to the Player material
    AudioManager am; // Reference to the AudioManager to play the sounds
    
    float currSpeed; // Used to set the speed of the player based on the state
    Vector2 moveInput; // Used to define the movement vector for the player 
    PlayerInput input; // Reference to the PlayerInput component 
    Rigidbody rb; // Reference to the Rigidbody component
    public enum PlayerState {Walk, Run, Crouch}; // Player State
    public PlayerState state {get; private set;} // Reference to the PlayerState

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        input = GetComponent<PlayerInput>();
        currSpeed = walkSpeed;
        bar.SetMaxHealth(maxHealth);
        am = FindFirstObjectByType<AudioManager>();
    }
    
    // Update is called once per frame
    void FixedUpdate()
    {
        moveInput = input.actions["Move"].ReadValue<Vector2>();
        Move();
    }
    
    // Move the player in the intended XZ coordinate plane
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

    // Alternate the player speed based on which action is pressed
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

    // Play collision sound effect on wall hit
    void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Wall"))
        {
            am.Play("Collision");
        }
    }

    // Change the material of the player object
    void ChangeBody(Material mat)
    {
        GetComponent<MeshRenderer>().material = mat;
    }
}
