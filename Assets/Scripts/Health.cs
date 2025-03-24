using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] GameObject[] hearts; // A list of heart images (3 max)
    public int lives {get; set;} // No. of lives the player will get

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    // Set lives to be the no. of heart elements
    void Start()
    {
        lives = hearts.Length;
    }

    // Update is called once per frame
    // Update the no. of lives displayed on the game UI
    void Update()
    {
        for(int i = 0; i < hearts.Length; i++) 
        {
            hearts[i].SetActive(i < lives);
        }
    }
}
