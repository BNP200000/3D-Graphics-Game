using UnityEngine;

[System.Serializable]
public class Sound
{
    public string name; // Name of the audio file
    public AudioClip clip; // Reference to the AudioClip component
    [Range(0f, 1f)] public float volume; // Volume of the audio
    [Range(.1f, 3f)] public float pitch; // Pitch of the audio
    [HideInInspector] public AudioSource source; // The source of the audio
    public bool loop; // Option to allow looping
}
