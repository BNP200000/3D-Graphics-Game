using UnityEngine;

public class EnemyDetection : MonoBehaviour
{
    [Header("Detection Settings")]
    public float detectionRange = 10f;
    public float fovAngle = 90f;
    public float catchTimeRequired = 2f;
    public LayerMask obstacleLayer;
    
    private Transform player;
    private bool isInitialized = false;
    private float currentDetectionTime;

    void Start()
    {
        GameObject playerObj = GameObject.FindWithTag("Player");
        if (playerObj != null)
        {
            player = playerObj.transform;
            isInitialized = true;
        }
        else
        {
            Debug.LogError("Player not found!", this);
            enabled = false;
        }
    }

public bool IsPlayerDetected()
{
    if (!isInitialized) return false;

    Vector3 directionToPlayer = (player.position - transform.position).normalized;
    float distanceToPlayer = Vector3.Distance(transform.position, player.position);
    float angleToPlayer = Vector3.Angle(transform.forward, directionToPlayer);

    // Get PlayerMovement component safely
    var playerMovement = player.GetComponent<PlayerMovement>();
    if (playerMovement == null) return false;

    bool isPlayerVisible = distanceToPlayer <= detectionRange && 
                         angleToPlayer <= fovAngle/2 &&
                         !Physics.Raycast(transform.position, directionToPlayer, distanceToPlayer, obstacleLayer) &&
                         !playerMovement.IsCrouching; // ← Now using the public property

    if (isPlayerVisible)
    {
        currentDetectionTime += Time.deltaTime;
        return currentDetectionTime >= catchTimeRequired;
    }
    
    currentDetectionTime = 0f;
    return false;
}

    public Vector3 GetPlayerPosition()
    {
        return isInitialized ? player.position : Vector3.zero;
    }
}