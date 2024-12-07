using UnityEngine;
using UnityEngine.UI;

public class MusicController : MonoBehaviour
{
    private static MusicController instance;
    [SerializeField] private Toggle musicToggle;
    [SerializeField] private AudioSource backgroundMusic;
    [SerializeField] private AudioClip menuMusic;
    [SerializeField] private AudioClip gameMusic;

    private bool isPlaying = true; // Track whether music is playing

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject); // Keep MusicController across scenes
    }

    private void Start()
    {
        SyncToggleState(); // Sync toggle with the current music state
    }

    public void PlayMenuMusic()
    {
        if (backgroundMusic.clip != menuMusic) // Check if it's already playing menu music
        {
            backgroundMusic.clip = menuMusic;
            backgroundMusic.time = 0f; // Start from the beginning
        }
        if (isPlaying) backgroundMusic.Play(); // Only play if music is enabled
        Debug.Log("Playing menu music.");
    }

    public void PlayGameMusic()
    {
        if (backgroundMusic.clip != gameMusic) // Check if it's already playing game music
        {
            backgroundMusic.clip = gameMusic;
            backgroundMusic.time = 0f; // Start from the beginning
        }
        if (isPlaying) backgroundMusic.Play(); // Only play if music is enabled
        Debug.Log("Playing game music.");
    }

    public void StopMusic()
    {
        if (backgroundMusic.isPlaying)
        {
            backgroundMusic.Stop(); // Stop the music
            Debug.Log("Music stopped.");
        }
    }

    public void ToggleMusic(bool isOn)
    {
        isPlaying = isOn;

        if (isOn)
        {
            backgroundMusic.Play(); // Resume playing
            Debug.Log("Music started.");
        }
        else
        {
            backgroundMusic.Pause(); // Pause the music
            Debug.Log("Music paused.");
        }
    }

    public void UpdateToggleReference(Toggle newToggle)
    {
        if (musicToggle != null)
        {
            musicToggle.onValueChanged.RemoveListener(ToggleMusic); // Remove old listener
        }

        musicToggle = newToggle;

        if (musicToggle != null)
        {
            musicToggle.onValueChanged.AddListener(ToggleMusic); // Add new listener
            SyncToggleState(); // Sync toggle state with music status
        }
    }

    public void SyncToggleState()
    {
        if (musicToggle != null)
        {
            musicToggle.isOn = isPlaying; // Update toggle's visual state
        }
    }
}
