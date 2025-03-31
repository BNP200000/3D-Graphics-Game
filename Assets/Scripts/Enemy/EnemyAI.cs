using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    [Header("AI Settings")]
    public bool resetPatrolOnChaseEnd = true;
    
    private EnemyPatrol enemyPatrol;
    private EnemyDetection enemyDetection;
    private EnemyChase enemyChase;
    private bool isChasing = false;

    void Start()
    {
        enemyPatrol = GetComponent<EnemyPatrol>();
        enemyDetection = GetComponent<EnemyDetection>();
        enemyChase = GetComponent<EnemyChase>();
    }

    void Update()
    {
        if (enemyDetection.IsPlayerDetected())
        {
            if (!isChasing)
            {
                StartChasing();
            }
            
            // Your existing player reset would trigger here
            // when IsPlayerDetected() returns true
        }
        else if (isChasing && ShouldStopChasing())
        {
            StopChasing();
        }

        if (!isChasing && enemyPatrol.HasReachedDestination())
        {
            enemyPatrol.MoveToNextPoint();
        }
    }

    bool ShouldStopChasing()
    {
        return Vector3.Distance(transform.position, enemyDetection.GetPlayerPosition()) > enemyChase.chaseRange;
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