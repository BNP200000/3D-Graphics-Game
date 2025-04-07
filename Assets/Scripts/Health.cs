using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] GameObject[] hearts;
    public int lives { get; set; }

    void Start()
    {
        lives = hearts.Length;
    }

    void Update()
    {
        for(int i = 0; i < hearts.Length; i++) 
        {
            hearts[i].SetActive(i < lives);
        }

        if (lives <= 0)
        {
            GameManager.instance.GameOver();
        }
    }
}
