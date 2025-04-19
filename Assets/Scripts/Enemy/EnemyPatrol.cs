using UnityEngine;
using UnityEngine.AI;
using System.Collections;

[RequireComponent(typeof(NavMeshAgent))]
public class EnemyPatrol : MonoBehaviour
{
    public Transform[] patrolPoints {get; set;} // Array of patrol points
    private int currentPointIndex = 0;
    private NavMeshAgent agent;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        StartCoroutine(WaitForPatrolPoints());
    }

    // Coroutine to wait until patrol points are initialized
    private IEnumerator WaitForPatrolPoints()
    {
        while (patrolPoints == null || patrolPoints.Length == 0)
        {
            yield return null;
        }

        MoveToNextPoint();
    }

    public void MoveToNextPoint()
    {
        if (patrolPoints.Length == 0) return;

        // Set the destination to the next patrol point
        agent.destination = patrolPoints[currentPointIndex].position;

        // Move to the next point in the array (loop back to the start if necessary)
        currentPointIndex = (currentPointIndex + 1) % patrolPoints.Length;
    }

    public bool HasReachedDestination()
    {
        // Check if the enemy has reached its destination
        return !agent.pathPending && agent.remainingDistance < 0.5f;
    }
}
