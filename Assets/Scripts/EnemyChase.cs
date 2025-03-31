using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class EnemyChase : MonoBehaviour
{
    [Header("Chase Settings")]
    public float chaseSpeed = 5f;
    public float chaseRange = 15f;
    public string playerTag = "Player";
    
    private NavMeshAgent agent;
    private Transform player;
    private bool isInitialized = false;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        
        GameObject playerObj = GameObject.FindWithTag(playerTag);
        if (playerObj != null)
        {
            player = playerObj.transform;
            isInitialized = true;
        }
        else
        {
            Debug.LogError($"No GameObject with tag '{playerTag}' found!", this);
            enabled = false; // Disable this script
        }
    }

    public void StartChasing()
    {
        if (!isInitialized || !agent.isOnNavMesh) return;
        agent.speed = chaseSpeed;
        agent.isStopped = false;
    }

    public void StopChasing()
    {
        if (!isInitialized || !agent.isOnNavMesh) return;
        agent.isStopped = true;
        agent.ResetPath();
    }

    public void ChasePlayer()
    {
        if (!isInitialized || !agent.isOnNavMesh || agent.isStopped || player == null) return;
        agent.SetDestination(player.position);
    }

    public bool IsPlayerInChaseRange()
    {
        if (!isInitialized || player == null) return false;
        return Vector3.Distance(transform.position, player.position) <= chaseRange;
    }
}