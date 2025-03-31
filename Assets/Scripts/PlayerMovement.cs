using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Stealth Settings")]
    public bool isCrouching = false;
    public KeyCode crouchKey = KeyCode.LeftControl;

    void Update()
    {
        if (Input.GetKeyDown(crouchKey))
        {
            isCrouching = !isCrouching;
            Debug.Log(isCrouching ? "Crouching (Hidden)" : "Standing (Visible)");
        }
    }
}