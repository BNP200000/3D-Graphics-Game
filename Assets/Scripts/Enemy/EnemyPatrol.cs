using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class EnemyPatrol : MonoBehaviour
{
    [Header("Patrol Settings")]
    public Transform[] patrolPoints;
    public float patrolSpeed = 3f;
    
    private NavMeshAgent _agent;
    private int _currentPointIndex = 0;
    private bool _isInitialized = false;

    void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();
        if (_agent == null)
        {
            Debug.LogError("NavMeshAgent component missing!", this);
            enabled = false;
            return;
        }
        
        _agent.speed = patrolSpeed;
        _isInitialized = true;
    }

    void Start()
    {
        if (_isInitialized && patrolPoints.Length > 0 && _agent.isOnNavMesh)
        {
            MoveToNextPoint();
        }
    }

    public bool CheckNavMeshValidity()
    {
        if (!_isInitialized) return false;
        
        if (!_agent.isOnNavMesh)
        {
            return _agent.Warp(transform.position);
        }
        return true;
    }

    public void MoveToNextPoint()
    {
        if (!_isInitialized || patrolPoints.Length == 0 || !_agent.isOnNavMesh) return;
        
        _agent.destination = patrolPoints[_currentPointIndex].position;
        _currentPointIndex = (_currentPointIndex + 1) % patrolPoints.Length;
    }

    public bool HasReachedDestination()
    {
        if (!_isInitialized || !_agent.isOnNavMesh || _agent.pathPending) return false;
        
        return _agent.remainingDistance <= _agent.stoppingDistance && 
               (!_agent.hasPath || _agent.velocity.sqrMagnitude == 0f);
    }
}