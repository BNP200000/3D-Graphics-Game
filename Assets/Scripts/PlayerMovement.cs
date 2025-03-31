using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    public Camera playerCamera;
    public float walkSpeed = 6f;
    public float runSpeed = 12f;
    public float crouchSpeed = 3f;
    public float lookSpeed = 2f;
    public float lookXLimit = 45f;
    public KeyCode crouchKey = KeyCode.LeftControl;
    public float jumpPower = 7f;
    public float gravity = 10f;
    
    [Header("Stealth Settings")]
    [SerializeField] private bool _isCrouching = false;
    public bool IsCrouching => _isCrouching;

    private Vector3 moveDirection = Vector3.zero;
    private float rotationX = 0;
    private CharacterController characterController;
    private float originalHeight;
    private Vector3 originalCameraPosition;
    private bool canMove = true;

    void Start()
    {
        characterController = GetComponent<CharacterController>();
        originalHeight = characterController.height;
        originalCameraPosition = playerCamera.transform.localPosition;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        // Handle movement
        Vector3 forward = transform.TransformDirection(Vector3.forward);
        Vector3 right = transform.TransformDirection(Vector3.right);

        bool isRunning = Input.GetKey(KeyCode.LeftShift);
        float currentSpeed = _isCrouching ? crouchSpeed : (isRunning ? runSpeed : walkSpeed);
        
        float curSpeedX = canMove ? currentSpeed * Input.GetAxis("Vertical") : 0;
        float curSpeedY = canMove ? currentSpeed * Input.GetAxis("Horizontal") : 0;
        float movementDirectionY = moveDirection.y;
        moveDirection = (forward * curSpeedX) + (right * curSpeedY);

        // Handle jumping
        if (Input.GetButton("Jump") && canMove && characterController.isGrounded)
        {
            moveDirection.y = jumpPower;
        }
        else
        {
            moveDirection.y = movementDirectionY;
        }

        // Apply gravity
        if (!characterController.isGrounded)
        {
            moveDirection.y -= gravity * Time.deltaTime;
        }

        // Handle crouching
        if (Input.GetKeyDown(crouchKey) && canMove && characterController.isGrounded)
        {
            ToggleCrouch();
        }

        // Move the controller
        characterController.Move(moveDirection * Time.deltaTime);

        // Handle camera rotation
        if (canMove)
        {
            rotationX += -Input.GetAxis("Mouse Y") * lookSpeed;
            rotationX = Mathf.Clamp(rotationX, -lookXLimit, lookXLimit);
            playerCamera.transform.localRotation = Quaternion.Euler(rotationX, 0, 0);
            transform.rotation *= Quaternion.Euler(0, Input.GetAxis("Mouse X") * lookSpeed, 0);
        }
    }

    private void ToggleCrouch()
    {
        _isCrouching = !_isCrouching;
        
        if (_isCrouching)
        {
            characterController.height = originalHeight / 2f;
            playerCamera.transform.localPosition = new Vector3(
                originalCameraPosition.x,
                originalCameraPosition.y / 2f,
                originalCameraPosition.z
            );
            Debug.Log("Crouching (Stealth Mode)");
        }
        else
        {
            characterController.height = originalHeight;
            playerCamera.transform.localPosition = originalCameraPosition;
            Debug.Log("Standing (Normal Mode)");
        }
    }

    public void SetMovementEnabled(bool enabled)
    {
        canMove = enabled;
        Cursor.lockState = enabled ? CursorLockMode.Locked : CursorLockMode.None;
        Cursor.visible = !enabled;
    }
}