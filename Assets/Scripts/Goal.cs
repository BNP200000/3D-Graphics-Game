using UnityEngine;

public class Goal : MonoBehaviour
{
    public GameOverScript gameOverManager;

    void Start()
    {
        if (gameOverManager == null)
        {
            gameOverManager = FindFirstObjectByType<GameOverScript>();
        }
    }

    void OnCollisionEnter(Collision other)
    {
        if(other.gameObject.CompareTag("Player")) {
            gameOverManager.gameOver();
            Debug.Log("VICTORY");
        }
    }
}

