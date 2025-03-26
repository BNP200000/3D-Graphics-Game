using UnityEngine;
using UnityEngine.SceneManagement;
public class GameOverScript : MonoBehaviour
{
    public GameObject gameOverUI;

    public void gameOver(){
        gameOverUI.SetActive(true);

        GameObject player = GameObject.FindGameObjectWithTag("Player");

        if (player != null)
        {
            var rbMovement = player.GetComponent<Player>();
            if (rbMovement != null) rbMovement.enabled = false;

            var charMovement = player.GetComponent<PlayerMovement>();
            if (charMovement != null) charMovement.enabled = false;

            var cameraLook = player.GetComponentInChildren<PlayerCamera>();
            if (cameraLook != null) cameraLook.enabled = false;
        }

        
        Timer timer = FindFirstObjectByType<Timer>();
        if (timer != null)
        {
            timer.PauseTimer();
        }

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
        public void Restart()
    {
        Time.timeScale = 1f; // Reset timescale in case it was paused
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}

