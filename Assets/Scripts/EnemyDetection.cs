using UnityEngine;

public class EnemyDetection : MonoBehaviour
{
    [Header("Detection Settings")]
    public float detectionRange = 10f;
    public float fovAngle = 90f;
    public float catchTimeRequired = 2f;
    public string playerTag = "Player";
    public LayerMask obstacleLayer;
    
    private Transform player;
    private PlayerMovement playerMovement;
    private float currentDetectionTime;
    private bool isInitialized = false;

    void Start()
    {
        GameObject playerObj = GameObject.FindWithTag(playerTag);
        if (playerObj != null)
        {
            player = playerObj.transform;
            playerMovement = player.GetComponent<PlayerMovement>();
            isInitialized = true;
        }
        else
        {
            Debug.LogError($"No GameObject with tag '{playerTag}' found!", this);
        }
    }

    public bool IsPlayerDetected()
    {
        if (!isInitialized || player == null || playerMovement == null) return false;

        Vector3 directionToPlayer = (player.position - transform.position).normalized;
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        // Check if player is in range and within field of view
        if (distanceToPlayer <= detectionRange && 
            Vector3.Angle(transform.forward, directionToPlayer) <= fovAngle/2)
        {
            // Check line of sight and stealth status
            if (!Physics.Raycast(transform.position, directionToPlayer, distanceToPlayer, obstacleLayer) &&
                !playerMovement.isCrouching)
            {
                currentDetectionTime += Time.deltaTime;
                return currentDetectionTime >= catchTimeRequired;
            }
        }
        
        currentDetectionTime = 0f;
        return false;
    }

    public Vector3 GetPlayerPosition()
    {
        return isInitialized && player != null ? player.position : Vector3.zero;
    }
}