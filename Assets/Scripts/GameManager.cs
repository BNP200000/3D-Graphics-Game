using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public static bool paused = false; // Check if paused
    public VictoryScreenManager victoryScreenManager;

    [SerializeField] GameObject pauseUI;
    [SerializeField] GameObject gameUI;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        PauseMenu();
    }

    /*public void Reload()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }*/

    public void LoadGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Game");
    }

    void PauseMenu()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            
            if(paused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    public void Resume()
    {
        pauseUI.SetActive(false);
        gameUI.SetActive(true);
        Time.timeScale = 1f;
        paused = false;
    }

    public void Exit()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Menu");
    }

    void Pause()
    {
        Cursor.lockState = CursorLockMode.None;
        pauseUI.SetActive(true);
        gameUI.SetActive(false);
        Time.timeScale = 0f;
        paused = true;
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void PlayerWins()
    {
        victoryScreenManager.ShowVictoryScreen();
    }
}
