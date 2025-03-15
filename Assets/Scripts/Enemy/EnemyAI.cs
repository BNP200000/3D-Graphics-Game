using UnityEngine;

public class EnemyAI: MonoBehaviour
{
    public float chaseRange = 15f; // Range within which the enemy will chase the player
    private EnemyPatrol enemyPatrol;
    private EnemyDetection enemyDetection;
    private EnemyChase enemyChase;
    private bool isChasing = false; // Track whether the enemy is chasing the player

    void Start()
    {
        // Get references to other scripts
        enemyPatrol = GetComponent<EnemyPatrol>();
        enemyDetection = GetComponent<EnemyDetection>();
        enemyChase = GetComponent<EnemyChase>();
    }


    void Update()
    {
        // Check if the player is detected
        if(enemyDetection.IsPlayerDetected())
        {
            if (!isChasing)
            {
                Debug.Log("Player detected: Starting chase.");
                isChasing = true;
                enemyChase.StartChasing();
            }
        }
        else if (isChasing && Vector3.Distance(transform.position, enemyDetection.GetPlayerPosition()) > chaseRange)
        {
            Debug.Log("Player out of range. Stopping chase.");
            isChasing = false;
            enemyChase.StopChasing();
            enemyPatrol.MoveToNextPoint(); // Resume patrolling
        }

        // If chasing, move toward the player
        if(isChasing)
        {
            enemyChase.ChasePlayer();
        }
        // If not chasing, continue patrolling
        else if (enemyPatrol.HasReachedDestination())
        {
            enemyPatrol.MoveToNextPoint();
        }
    }


}
