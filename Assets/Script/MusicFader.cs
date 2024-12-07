using UnityEngine;
using UnityEngine.SceneManagement;

public class MusicFader : MonoBehaviour
{
    public AudioSource audioSource; // ตัวควบคุม AudioSource
    public AudioClip backgroundMusic; // เพลงพื้นหลังในเกม
    public AudioClip enemyMusic; // เพลงของศัตรู
    public float fadeDuration = 1f; // ระยะเวลา fade

    private bool isActive = false; // บอกว่า MusicFader กำลังทำงานอยู่หรือไม่
    private Coroutine currentFade; // เก็บข้อมูล Coroutine ของการ fade เพลง
    private static MusicFader instance; // Singleton

    private void Awake()
    {
        // ทำให้ MusicFader เป็น Singleton
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject); // ทำให้ MusicFader คงอยู่ข้าม Scene
    }

    private void Start()
    {
        // ตรวจสอบ Scene เมื่อเริ่มต้น
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded; // ยกเลิกการสมัคร Event
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "GameplayMazeScene")
        {
            ActivateMusicFader(); // ทำให้ MusicFader เริ่มทำงานใน GameplayMazeScene
        }
        else
        {
            DeactivateMusicFader(); // หยุด MusicFader ใน Scene อื่น
        }
    }

    private void ActivateMusicFader()
    {
        if (!isActive)
        {
            isActive = true;

            // เริ่มเล่นเพลงพื้นหลังของ GameplayMazeScene
            if (audioSource != null && backgroundMusic != null)
            {
                audioSource.clip = backgroundMusic;
                audioSource.volume = 1f; // ตั้งค่าเสียงเต็ม
                audioSource.Play();
                Debug.Log("MusicFader activated: Playing background music.");
            }
        }
    }

    private void DeactivateMusicFader()
    {
        if (isActive)
        {
            isActive = false;

            // หยุดเพลงและทำให้ MusicFader หยุดทำงาน
            if (audioSource != null)
            {
                audioSource.Stop();
                Debug.Log("MusicFader deactivated: Stopped music.");
            }
        }
    }

    public void SwitchToEnemyMusic()
    {
        if (isActive && audioSource != null && audioSource.clip != enemyMusic)
        {
            StartFade(enemyMusic);
        }
    }

    public void SwitchToBackgroundMusic()
    {
        if (isActive && audioSource != null && audioSource.clip != backgroundMusic)
        {
            StartFade(backgroundMusic);
        }
    }

    private void StartFade(AudioClip newClip)
    {
        if (currentFade != null)
        {
            StopCoroutine(currentFade); // หยุด Coroutine ก่อนหน้าถ้ามี
        }

        currentFade = StartCoroutine(FadeToNewClip(newClip));
    }

    private System.Collections.IEnumerator FadeToNewClip(AudioClip newClip)
    {
        if (audioSource.isPlaying)
        {
            // ลดเสียงเพลงปัจจุบัน
            float timer = 0f;
            float startVolume = audioSource.volume;

            while (timer < fadeDuration)
            {
                timer += Time.deltaTime;
                audioSource.volume = Mathf.Lerp(startVolume, 0f, timer / fadeDuration);
                yield return null;
            }

            audioSource.Stop(); // หยุดเพลงเดิม
        }

        // เล่นเพลงใหม่และเพิ่มเสียง
        audioSource.clip = newClip;
        audioSource.Play();

        float fadeInTimer = 0f;
        while (fadeInTimer < fadeDuration)
        {
            fadeInTimer += Time.deltaTime;
            audioSource.volume = Mathf.Lerp(0f, 1f, fadeInTimer / fadeDuration);
            yield return null;
        }

        audioSource.volume = 1f; // ตั้งค่าเสียงเต็ม
        currentFade = null; // รีเซ็ต Coroutine
    }
}
