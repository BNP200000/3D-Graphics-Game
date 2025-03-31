using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class EnemyChase : MonoBehaviour
{
    [Header("Chase Settings")]
    public float chaseSpeed = 5f;
    public float chaseRange = 15f;
    
    private NavMeshAgent _agent;
    private Transform _player;
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

        GameObject playerObj = GameObject.FindWithTag("Player");
        if (playerObj != null)
        {
            _player = playerObj.transform;
            _isInitialized = true;
        }
        else
        {
            Debug.LogError("Player not found!", this);
            enabled = false;
        }
    }

    public void StartChasing()
    {
        if (!_isInitialized || !_agent.isOnNavMesh) return;
        _agent.speed = chaseSpeed;
        _agent.isStopped = false;
    }

    public void StopChasing()
    {
        if (!_isInitialized || !_agent.isOnNavMesh) return;
        _agent.isStopped = true;
        _agent.ResetPath();
    }

    public void ChasePlayer()
    {
        if (!_isInitialized || !_agent.isOnNavMesh || _agent.isStopped || _player == null) return;
        _agent.SetDestination(_player.position);
    }

    public bool IsPlayerInChaseRange()
    {
        return _isInitialized && _player != null && 
               Vector3.Distance(transform.position, _player.position) <= chaseRange;
    }
}