using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    //[SerializeField] GameObject[] hearts;
    //public int lives { get; set; }
    [SerializeField] Slider slider;
    [SerializeField] Gradient gradient;
    [SerializeField] Image fill;

    public void SetMaxHealth(int health) 
    {
        slider.maxValue = health;
        slider.value = health;
        fill.color = gradient.Evaluate(1f);
    }

    public void SetHealth(int health)
    {
        slider.value = health;
        fill.color = gradient.Evaluate(slider.normalizedValue);
    }

    void Start()
    {
        //lives = hearts.Length;
    }

    void Update()
    {
        /*for(int i = 0; i < hearts.Length; i++) 
        {
            hearts[i].SetActive(i < lives);
        }

        if (lives <= 0)
        {
            GameManager.instance.GameOver();
        }*/
    }
}
