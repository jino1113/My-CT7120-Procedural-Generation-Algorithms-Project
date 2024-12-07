using UnityEngine;
using UnityEngine.SceneManagement;

public class PersistentMusicController : MonoBehaviour
{
    private static PersistentMusicController instance;
    private AudioSource audioSource;

    void Awake()
    {
        // ตรวจสอบ Singleton
        if (instance != null && instance != this)
        {
            Destroy(gameObject); // ทำลายตัวซ้ำ
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject); // รักษา BackgroundMusic ข้าม Scene

        // ตรวจสอบ AudioSource
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            Debug.LogError("No AudioSource found on PersistentMusicController! Please add an AudioSource component.");
        }

        SceneManager.sceneLoaded += OnSceneLoaded; // สมัคร Event เมื่อเปลี่ยน Scene
    }

    void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded; // ยกเลิก Event เมื่อ Object ถูกทำลาย
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        string sceneName = scene.name;
        Debug.Log($"Scene loaded: {sceneName}");

        // ถ้าเป็น GameplayMazeScene ให้ทำลาย BackgroundMusic
        if (sceneName == "GameplayMazeScene")
        {
            Destroy(gameObject); // ทำลาย PersistentMusicController ตัวนี้
            Debug.Log("Destroyed BackgroundMusic in GameplayMazeScene.");
        }
    }

    public void PlayMusic(AudioClip clip)
    {
        if (audioSource != null)
        {
            // ถ้าเพลงที่ต้องการเล่นเหมือนกับเพลงที่กำลังเล่นอยู่ ให้ข้ามการเริ่มใหม่
            if (clip != null && audioSource.clip == clip && audioSource.isPlaying)
            {
                Debug.Log("Music is already playing: " + clip.name);
                return;
            }

            // เริ่มเพลงใหม่
            audioSource.Stop();
            audioSource.clip = clip;
            audioSource.Play();
            Debug.Log("Playing music: " + clip.name);
        }
        else
        {
            Debug.LogError("No AudioSource component found on this GameObject. Cannot play music.");
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
}
