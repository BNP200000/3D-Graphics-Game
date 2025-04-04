
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public static bool paused = false;

    [SerializeField] GameObject pauseUI;
    [SerializeField] GameObject gameUI;
    [SerializeField] GameObject winUI;
    [SerializeField] GameObject gameOverUI;

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

    void Update()
    {
        PauseMenu();
    }

    public void Reload()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

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
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void Exit()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Menu");
    }

    void Pause()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        pauseUI.SetActive(true);
        gameUI.SetActive(false);
        Time.timeScale = 0f;
        paused = true;
    }

    public void PlayerWins()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        winUI.SetActive(true);
        gameUI.SetActive(false);
        Time.timeScale = 0f;
        paused = true;
    }

    public void GameOver()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        gameOverUI.SetActive(true);
        gameUI.SetActive(false);
        Time.timeScale = 0f;
        paused = true;
    }

    public void Quit()
    {
        Application.Quit();
    }
}
