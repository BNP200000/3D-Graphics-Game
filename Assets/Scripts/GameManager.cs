using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public static bool paused; // Check if paused
    public static bool inGame; // Check if in-game

    void Awake()
    {
        MakeSingleton();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        GameFlow();
    }

    public void Load()
    {
        SceneManager.LoadScene("Game");
        Time.timeScale = 1;
        paused = false;
        inGame = true;
    }

    public void Exit()
    {
        Application.Quit();
    }

    void GameFlow()
    {
        if(Input.GetKeyDown(KeyCode.Escape) && inGame) 
        {
            if(paused)
            {
                Time.timeScale = 1;
                paused = false;
                inGame = true;
            } 
            else
            {
                Time.timeScale = 0;
                paused = true;
                inGame = true;
            }
        }
    }

    void MakeSingleton()
    {
        if(instance != null)
        {
            Destroy(gameObject);
        }
        else 
        {
            instance = this;
        }
    }
}
