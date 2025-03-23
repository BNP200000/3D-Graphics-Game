using UnityEngine;

public class Goal : MonoBehaviour
{
    GameManager gm;
    void Awake()
    {
        gm = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log("VICTORY");

            Debug.Log(GameManager.instance);

            // Call GameManager to trigger the victory screen
            if (GameManager.instance != null)
            {
                GameManager.instance.PlayerWins();
            }
        }
    }
}
