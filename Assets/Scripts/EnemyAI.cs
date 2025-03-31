using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    [Header("AI Settings")]
    public bool resetPatrolOnChaseEnd = true;
    public float groundCheckDistance = 2f;
    
    private EnemyPatrol enemyPatrol;
    private EnemyDetection enemyDetection;
    private EnemyChase enemyChase;
    private bool isChasing = false;

    void Start()
    {
        enemyPatrol = GetComponent<EnemyPatrol>();
        enemyDetection = GetComponent<EnemyDetection>();
        enemyChase = GetComponent<EnemyChase>();
        
        // Now using the public method
        if (!enemyPatrol.CheckNavMeshValidity())
        {
            Debug.LogWarning("Patrol disabled - attempting to place on NavMesh...");
            TryRepositionEnemy();
        }
    }

    void TryRepositionEnemy()
    {
        if (Physics.Raycast(transform.position + Vector3.up, Vector3.down, out RaycastHit hit, groundCheckDistance))
        {
            if (UnityEngine.AI.NavMesh.SamplePosition(hit.point, out UnityEngine.AI.NavMeshHit navHit, 1f, UnityEngine.AI.NavMesh.AllAreas))
            {
                transform.position = navHit.position;
                // Recheck validity after repositioning
                enemyPatrol.CheckNavMeshValidity();
            }
        }
    }

    void Update()
    {
        if (!enemyPatrol.enabled) return;

        if (enemyDetection.IsPlayerDetected())
        {
            if (!isChasing) StartChasing();
        }
        else if (isChasing && !enemyChase.IsPlayerInChaseRange())
        {
            StopChasing();
        }

        if (!isChasing && enemyPatrol.HasReachedDestination())
        {
            enemyPatrol.MoveToNextPoint();
        }
    }

    void StartChasing()
    {
        isChasing = true;
        enemyChase.StartChasing();
    }

    void StopChasing()
    {
        isChasing = false;
        enemyChase.StopChasing();
        if (resetPatrolOnChaseEnd) enemyPatrol.MoveToNextPoint();
    }
}