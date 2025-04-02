using UnityEngine;
using UnityEngine.AI;

public class EnemyChase : MonoBehaviour
{
    public float chaseSpeed = 5f; // Speed at which the enemy chases the player
    private NavMeshAgent agent;
    private Transform player;
    private Health playerHealth;
    [SerializeField] float captureRange = 3.0f;


    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        player = GameObject.Find("Player").transform;
        playerHealth = FindAnyObjectByType<Health>();
    }

    public void StartChasing()
    {
        agent.speed = chaseSpeed;
        agent.isStopped = false; // Ensure the agent is moving
    }

    public void StopChasing()
    {
        agent.isStopped = true; // Stop the agent
        agent.ResetPath(); // Clear the current path
    }

    public void ChasePlayer()
    {
        agent.SetDestination(player.position); // Move toward the player
        if(Vector3.Distance(transform.position, player.position) <= captureRange) {
            Capture();
        }
    }

    void Capture()
    {
        playerHealth.lives -= 1;
        player.position = new Vector3(0f, 0.5f, 0f);
    }
}