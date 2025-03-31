using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class EnemyPatrol : MonoBehaviour
{
    [Header("Patrol Settings")]
    public Transform[] patrolPoints;
    public float patrolSpeed = 3f;
    
    private NavMeshAgent agent;
    private int currentPointIndex = 0;
    private bool isValidNavMesh = false;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.speed = patrolSpeed;
        
        // Changed to public method
        isValidNavMesh = CheckNavMeshValidity();
        
        if (isValidNavMesh && patrolPoints.Length > 0)
        {
            MoveToNextPoint();
        }
    }

    // Changed to public
    public bool CheckNavMeshValidity()
    {
        bool valid = agent.isOnNavMesh;
        if (!valid)
        {
            // Try to warp the agent to the nearest valid position
            valid = agent.Warp(transform.position);
            
            if (!valid)
            {
                Debug.LogWarning("Enemy could not find valid NavMesh position!", this);
            }
        }
        return valid;
    }

    public void MoveToNextPoint()
    {
        if (!isValidNavMesh || patrolPoints.Length == 0) return;
        
        agent.destination = patrolPoints[currentPointIndex].position;
        currentPointIndex = (currentPointIndex + 1) % patrolPoints.Length;
    }

    public bool HasReachedDestination()
    {
        if (!isValidNavMesh || !agent.isOnNavMesh || agent.pathPending) return false;
        
        return agent.remainingDistance <= agent.stoppingDistance && 
               (!agent.hasPath || agent.velocity.sqrMagnitude == 0f);
    }
}