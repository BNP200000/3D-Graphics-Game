using UnityEngine;
using TMPro;
public class Timer : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI timerText;
    public float runningTime {get; private set;}

    // Update is called once per frame
    void Update()
    {
        runningTime += Time.deltaTime;
        int minutes = Mathf.FloorToInt(runningTime / 60);
        int seconds = Mathf.FloorToInt(runningTime % 60);
        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    public string TimeString(float time)
    {
        int minutes = Mathf.FloorToInt(time / 60);
        int seconds = Mathf.FloorToInt(time % 60);   
        return string.Format("{0:00}:{1:00}", minutes, seconds);
    }
}
