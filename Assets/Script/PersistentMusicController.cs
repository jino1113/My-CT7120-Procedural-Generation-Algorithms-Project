using UnityEngine;

public class PersistentMusicController : MonoBehaviour
{
    private static PersistentMusicController instance;
    private AudioSource audioSource;

    void Awake()
    {
        // Singleton pattern เพื่อหลีกเลี่ยงการมี PersistentMusicController หลายตัว
        if (instance != null && instance != this)
        {
            Destroy(gameObject); // ทำลายตัวซ้ำ
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject); // รักษา Controller ข้าม Scene

        // ตรวจสอบ AudioSource
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            Debug.LogError("No AudioSource found on PersistentMusicController!");
        }
    }

    public void PlayMusic()
    {
        if (audioSource != null && !audioSource.isPlaying)
        {
            audioSource.Play();
            Debug.Log("Music started.");
        }
    }

    public void StopMusic()
    {
        if (audioSource != null && audioSource.isPlaying)
        {
            audioSource.Stop();
            Debug.Log("Music stopped.");
        }
    }

    public bool IsMusicPlaying()
    {
        return audioSource != null && audioSource.isPlaying;
    }
}
