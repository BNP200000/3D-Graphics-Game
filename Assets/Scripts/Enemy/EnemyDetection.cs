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
    //private PlayerMovement playerMovement;
    private float currentDetectionTime;
    private bool wasPlayerVisibleLastFrame;

    void Start()
    {
        player = GameObject.FindWithTag("Player").transform;
        //playerMovement = player.GetComponent<PlayerMovement>();
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

        // Update detection timer
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

    public float GetDetectionProgress() => Mathf.Clamp01(currentDetectionTime / catchTimeRequired);
    public Vector3 GetPlayerPosition() => player.position;

    void OnDrawGizmosSelected()
    {
        // Visualize detection range
        Gizmos.color = new Color(1, 1, 0, 0.1f);
        Gizmos.DrawSphere(transform.position, detectionRange);

        // Visualize FoV
        Gizmos.color = Color.yellow;
        Vector3 leftRay = Quaternion.Euler(0, -fovAngle/2, 0) * transform.forward * detectionRange;
        Vector3 rightRay = Quaternion.Euler(0, fovAngle/2, 0) * transform.forward * detectionRange;
        Gizmos.DrawRay(transform.position, leftRay);
        Gizmos.DrawRay(transform.position, rightRay);
        Gizmos.DrawLine(transform.position + leftRay, transform.position + rightRay);
    }
}