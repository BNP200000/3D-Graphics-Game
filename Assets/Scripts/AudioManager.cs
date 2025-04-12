using UnityEngine;
using System;

public class AudioManager : MonoBehaviour
{
    [SerializeField] Sound[] sounds; // A list of sound used in the game
    public static AudioManager instance; // Instance of the AudioManager for singleton use

    // Create a singleton of the AudioManager and load
    // all of the sounds used for the game
    void Awake()
    {
        MakeSingleton();
        
        foreach(Sound s in sounds) 
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
        }   

        
    }

    // Play the main theme
    void Start()
    {
        Play("Theme");
    }

    // Create a singleton object that ensures only one 
    // instance of AudioManager exists at a time
    void MakeSingleton() 
    {
        if(instance == null) 
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    // Play the sound effect passed in the parameter
    public void Play(string name) 
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if(s == null || s.source == null) {
            Debug.LogWarning("Sound: " + name + " not found");
            return;
        }
        s.source.Play();
    }

    // Update the BGM and SFX volume to match the one
    // stored in the PlayerPrefs
    void UpdateSound()
    {
        sounds[0].source.volume = PlayerPrefs.GetFloat("BGM_Volume", 0.4f);

        for(int i = 1; i < sounds.Length; i++)
        {
            sounds[i].source.volume = PlayerPrefs.GetFloat("SFX_Volume", 0.6f);
        }
    }

    // Subscribe to the event when the volume changes from SettingManager
    // when the object is enabled
    void OnEnable()
    {
        SettingManager.OnVolumeChanged += UpdateSound;
    }

    // Unsubscribe to the event when the volume changes from SettingManager
    // when the object is disabled
    void OnDisable()
    {
        SettingManager.OnVolumeChanged -= UpdateSound;
    }
}
