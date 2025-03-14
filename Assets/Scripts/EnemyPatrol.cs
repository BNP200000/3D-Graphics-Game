using UnityEngine;
using UnityEngine.AI;

public class EnemyPatrol : MonoBehaviour
{
    public Transform[] patrolPoints; // Array of patrol points
    private int currentPointIndex = 0;
    private NavMeshAgent agent;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        if (patrolPoints.Length > 0)
        {
            MoveToNextPoint(); // Start patrolling if there are patrol points
        }
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