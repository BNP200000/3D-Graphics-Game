using UnityEngine;

public class Goal : MonoBehaviour
{
    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            // Call GameManager to trigger the victory screen
            if (GameManager.instance != null)
            {
                GameManager.instance.PlayerWins();
            }
        }
    }
}
