using UnityEngine;
using UnityEngine.AI;

public class EnemyChase : MonoBehaviour
{
    [Header("Chase Settings")]
    public float chaseSpeed = 5f;
    public float chaseRange = 15f;
    
    private NavMeshAgent agent;
    private Transform player;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        player = GameObject.FindWithTag("Player").transform;
    }

    public void StartChasing()
    {
        agent.speed = chaseSpeed;
        agent.isStopped = false;
    }

    public void StopChasing()
    {
        agent.isStopped = true;
        agent.ResetPath();
    }

    public void ChasePlayer()
    {
        if (!agent.isStopped)
        {
            agent.SetDestination(player.position);
        }
    }
}