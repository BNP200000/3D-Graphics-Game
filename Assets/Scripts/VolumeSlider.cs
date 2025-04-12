using UnityEngine;
using UnityEngine.UI;

public class VolumeSlider : MonoBehaviour
{
    [SerializeField] Slider sfxSlider, bgmSlider; // Slider UI elements

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    // Set the slider value to the same value stored in the PlayerPrefs
    // Default the value if the PlayerPrefs could not find the saved preferences
    void Start()
    {
        sfxSlider.value = PlayerPrefs.GetFloat("SFX_Volume", 0.6f);
        bgmSlider.value = PlayerPrefs.GetFloat("BGM_Volume", 0.4f);
    }
}
