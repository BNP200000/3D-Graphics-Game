using UnityEngine;

public class EnemyAI : MonoBehaviour
{
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
            
            // Your existing player reset function would trigger here
            // when IsPlayerDetected() returns true
        }
        else if (isChasing && Vector3.Distance(transform.position, enemyDetection.GetPlayerPosition()) > enemyChase.chaseRange)
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
        enemyPatrol.MoveToNextPoint();
    }
}