using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    [Header("AI Settings")]
    public bool resetPatrolOnChaseEnd = true;
    public float groundCheckDistance = 2f;
    
    private EnemyPatrol _enemyPatrol;
    private EnemyDetection _enemyDetection;
    private EnemyChase _enemyChase;
    private bool _isChasing = false;
    private bool _isInitialized = false;

    void Awake()
    {
        _enemyPatrol = GetComponent<EnemyPatrol>();
        _enemyDetection = GetComponent<EnemyDetection>();
        _enemyChase = GetComponent<EnemyChase>();
        
        if (_enemyPatrol == null || _enemyDetection == null || _enemyChase == null)
        {
            Debug.LogError("Missing required enemy components!", this);
            enabled = false;
            return;
        }
        
        _isInitialized = true;
    }

    void Start()
    {
        if (!_isInitialized) return;
        
        if (!_enemyPatrol.CheckNavMeshValidity())
        {
            Debug.LogWarning("Agent not on NavMesh - attempting reposition...");
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
                _enemyPatrol.CheckNavMeshValidity();
            }
        }
    }

    void Update()
    {
        if (!_isInitialized) return;

        if (_enemyDetection.IsPlayerDetected())
        {
            if (!_isChasing) StartChasing();
        }
        else if (_isChasing && !_enemyChase.IsPlayerInChaseRange())
        {
            StopChasing();
        }

        if (!_isChasing && _enemyPatrol.HasReachedDestination())
        {
            _enemyPatrol.MoveToNextPoint();
        }
    }

    void StartChasing()
    {
        _isChasing = true;
        _enemyChase.StartChasing();
    }

    void StopChasing()
    {
        _isChasing = false;
        _enemyChase.StopChasing();
        if (resetPatrolOnChaseEnd) _enemyPatrol.MoveToNextPoint();
    }
}