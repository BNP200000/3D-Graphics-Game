using UnityEngine;

public class EnemyDetection : MonoBehaviour
{
    [Header("Detection Settings")]
    public float detectionRange = 10f;
    public float fovAngle = 90f;
    public float catchTimeRequired = 2f;
    public LayerMask playerLayer;
    public LayerMask obstacleLayer;
    
    private Transform player;
    private float currentDetectionTime;
    private bool wasPlayerVisibleLastFrame;

    void Start()
    {
        player = GameObject.FindWithTag("Player").transform;
    }

    public bool IsPlayerDetected()
    {
        if (player == null) return false;

        Vector3 directionToPlayer = (player.position - transform.position).normalized;
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);
        float angleToPlayer = Vector3.Angle(transform.forward, directionToPlayer);

        bool isPlayerVisible = distanceToPlayer <= detectionRange && 
                             angleToPlayer <= fovAngle/2 &&
                             !Physics.Raycast(transform.position, directionToPlayer, distanceToPlayer, obstacleLayer) &&
                             player.GetComponent<Player>().state != Player.PlayerState.Crouch;

        // Timed detection logic
        if (isPlayerVisible)
        {
            currentDetectionTime += Time.deltaTime;
            wasPlayerVisibleLastFrame = true;
        }
        else if (wasPlayerVisibleLastFrame)
        {
            currentDetectionTime = 0f;
            wasPlayerVisibleLastFrame = false;
        }

        return currentDetectionTime >= catchTimeRequired;
    }

    public Vector3 GetPlayerPosition() => player.position;

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
        
        Vector3 leftRay = Quaternion.Euler(0, -fovAngle/2, 0) * transform.forward * detectionRange;
        Vector3 rightRay = Quaternion.Euler(0, fovAngle/2, 0) * transform.forward * detectionRange;
        Gizmos.DrawRay(transform.position, leftRay);
        Gizmos.DrawRay(transform.position, rightRay);
    }
}