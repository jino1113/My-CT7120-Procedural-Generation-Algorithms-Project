using UnityEngine;

public class MenuMusic : MonoBehaviour
{
    public AudioClip backgroundMusic; // Drag and drop your audio clip here
    private AudioSource audioSource;

    void Start()
    {
        audioSource = gameObject.AddComponent<AudioSource>(); // Add AudioSource dynamically
        audioSource.clip = backgroundMusic;
        audioSource.loop = true; // Enable looping
        audioSource.playOnAwake = true; // Play on awake
        audioSource.Play(); // Start playing the music
    }
}
