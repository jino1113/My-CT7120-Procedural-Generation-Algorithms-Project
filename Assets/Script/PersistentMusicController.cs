using UnityEngine;

public class PersistentMusicController : MonoBehaviour
{
    private static PersistentMusicController instance;
    private AudioSource audioSource;

    public AudioClip defaultMenuMusic; // เพลงเมนูเริ่มต้น

    void Awake()
    {
        // ป้องกันการสร้างตัวซ้ำ
        if (instance != null && instance != this)
        {
            Destroy(gameObject); // ลบตัวซ้ำ
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);

        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            Debug.LogError("No AudioSource found on PersistentMusicController!");
        }
    }

    void Start()
    {
        // เล่นเพลงเมนูเริ่มต้น
        if (audioSource != null && defaultMenuMusic != null && !audioSource.isPlaying)
        {
            audioSource.clip = defaultMenuMusic;
            audioSource.Play();
            Debug.Log("Playing default menu music: " + defaultMenuMusic.name);
        }
    }

    public void StopMusic()
    {
        if (audioSource != null && audioSource.isPlaying)
        {
            Debug.Log("Stopping music: " + audioSource.clip?.name);
            audioSource.Stop();
        }
    }

    public void PlayMusic(AudioClip clip)
    {
        if (audioSource != null)
        {
            if (clip != null)
            {
                audioSource.Stop(); // หยุดเพลงที่กำลังเล่นอยู่
                audioSource.clip = clip;
                audioSource.Play();
                Debug.Log("Playing music: " + clip.name);
            }
        }
        else
        {
            Debug.LogError("No AudioSource component found on this GameObject.");
        }
    }

    public void DestroyBackgroundMusicInScene(string sceneName)
    {
        GameObject bgMusic = GameObject.Find("BackgroundMusic");
        if (bgMusic != null)
        {
            Debug.Log($"Destroying BackgroundMusic in {sceneName}.");
            Destroy(bgMusic);
        }
    }
}
