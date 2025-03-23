using UnityEngine;

public class EnemyDetection : MonoBehaviour
{
    public float detectionRange = 10f; // Range within which the enemy can detect the player
    public LayerMask playerLayer; // Layer for the player
    public LayerMask obstacleLayer; // Layer for obstacles
    private Transform player; // Reference to the player's transform

    void Start()
    {
        // Find the player GameObject by tag
        player = GameObject.Find("Player").transform;
    }

    public bool IsPlayerDetected()
    {
        if(player.GetComponent<Player>().state == Player.PlayerState.Crouch)
            return false;

        // Check if the player is within detection range
        if (Vector3.Distance(transform.position, player.position) <= detectionRange)
        {
            // Calculate the direction to the player
            Vector3 directionToPlayer = (player.position - transform.position).normalized;
            
            if(!Physics.Raycast(transform.position, directionToPlayer, detectionRange, obstacleLayer)) 
            {
                if(Physics.Raycast(transform.position, directionToPlayer, out RaycastHit hit, detectionRange, playerLayer)) 
                {
                    if(hit.transform.CompareTag("Player"))
                    {
                        return true;
                    }
                }
            } 
            else
            {
                Debug.Log("HIT WALL");
            }
        }
        return false; // Player is not detected
    }

    public Vector3 GetPlayerPosition()
    {
        return player.position; // Return the player's position
    }
}