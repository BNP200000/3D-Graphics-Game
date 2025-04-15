using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public static bool paused = false;

    [SerializeField] GameObject pauseUI, gameUI, winUI, loseUI;
    TextMeshProUGUI playTimerText, bestTimerText;
    Timer timer; // Reference to Timer script
    Player player; 

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    void Start()
    {
        timer = FindAnyObjectByType<Timer>();
        player = FindAnyObjectByType<Player>();
    }

    void Update()
    {
        Toggle();
    }

    public void LoadGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Game");
    }

    void Toggle()
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
        Debug.Log(pauseUI);
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

    public void Setting()
    {
        SceneManager.LoadScene("Settings");
    }

    public void Credit()
    {
        SceneManager.LoadScene("Credit");
    }

    void Pause()
    {
        pauseUI.SetActive(true);
        gameUI.SetActive(false);
        Time.timeScale = 0f;
        paused = true;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void PlayerWins()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        winUI.SetActive(true);
        gameUI.SetActive(false);
        Time.timeScale = 0f;
        paused = true;
        SaveData();
    }

    public void GameOver()
    {
        if(player.maxHealth > 0) return;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        loseUI.SetActive(true);
        gameUI.SetActive(false);
        Time.timeScale = 0f;
        paused = true;
        SaveData();
    }

    public void Quit()
    {
        Application.Quit();
    }

    void SaveData()
    {
        if(!(loseUI.activeInHierarchy || winUI.activeInHierarchy)) return;

        Transform panelObj = null;
        if (loseUI.activeInHierarchy)
        {
            panelObj = loseUI.transform.Find("Panel");
        }
        else if (winUI.activeInHierarchy)
        {
            panelObj = winUI.transform.Find("Panel");
        }

        Transform playTimerObj = null;
        Transform bestTimerObj = null;

        foreach(Transform gc in panelObj)
        {
            if(gc.name.Equals("PlayTime"))
            {
                playTimerObj = gc;
            }
            else if(gc.name.Equals("BestTime"))
            {
                bestTimerObj = gc;
            }
        }

        if(playTimerObj == null || bestTimerObj == null) return;

        playTimerText = playTimerObj.GetComponent<TextMeshProUGUI>();
        bestTimerText = bestTimerObj.GetComponent<TextMeshProUGUI>();

        if(playTimerText == null || bestTimerText == null) return;

        // Display the play time and set the best time if it is beaten; game over will not
        // display best time but only N/A.
        playTimerText.text = string.Format("Play Time: {0}", timer.TimeString(timer.runningTime));
        if(timer.runningTime < PlayerPrefs.GetFloat("Best-Time", float.MaxValue) && winUI.activeInHierarchy)
        {
            PlayerPrefs.SetFloat("Best-Time", timer.runningTime);
        }

        float bestTime = PlayerPrefs.GetFloat("Best-Time", float.MaxValue);
        bestTimerText.text = string.Format("Best Time: {0}", (bestTime == float.MaxValue) ? "N/A" : timer.TimeString(bestTime));
    }
}
