using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class EnemyChase : MonoBehaviour
{
    public float chaseSpeed = 5f; // Speed at which the enemy chases the player
    private NavMeshAgent agent;
    private Transform player;
    //private Health playerHealth;
    private Player playerScript;
    [SerializeField] float captureRange = 3.0f;


    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        player = GameObject.Find("Player").transform;
        playerScript = player.GetComponent<Player>();
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
        //playerHealth.lives -= 1;
        playerScript.maxHealth -= 1;
        playerScript.bar.SetHealth(playerScript.maxHealth);
        player.position = new Vector3(0f, 0.5f, 0f);
    }
}