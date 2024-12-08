using UnityEngine;
using UnityEngine.SceneManagement;

public class MusicFader : MonoBehaviour
{
    public AudioSource audioSource; // ตัวควบคุม AudioSource / The AudioSource component to control music
    public AudioClip backgroundMusic; // เพลงพื้นหลังในเกม / The background music clip
    public AudioClip enemyMusic; // เพลงของศัตรู / The enemy music clip
    public float fadeDuration = 1f; // ระยะเวลา fade / Duration for fading between tracks

    private bool isActive = false; // บอกว่า MusicFader กำลังทำงานอยู่หรือไม่ / Indicates if MusicFader is active
    private Coroutine currentFade; // เก็บข้อมูล Coroutine ของการ fade เพลง / Stores the current fade Coroutine
    private static MusicFader instance; // Singleton / Singleton instance

    private void Awake()
    {
        // ทำให้ MusicFader เป็น Singleton / Ensure MusicFader is a singleton
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject); // ทำให้ MusicFader คงอยู่ข้าม Scene / Preserve MusicFader across scenes
    }

    private void Start()
    {
        // ตรวจสอบ Scene เมื่อเริ่มต้น / Check scene when the game starts
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded; // ยกเลิกการสมัคร Event / Unsubscribe from scene-loaded events
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "GameplayMazeScene")
        {
            ActivateMusicFader(); // ทำให้ MusicFader เริ่มทำงานใน GameplayMazeScene / Activate MusicFader in GameplayMazeScene
        }
        else
        {
            DeactivateMusicFader(); // หยุด MusicFader ใน Scene อื่น / Deactivate MusicFader in other scenes
        }
    }

    private void ActivateMusicFader()
    {
        if (!isActive)
        {
            isActive = true;

            // เริ่มเล่นเพลงพื้นหลังของ GameplayMazeScene / Start playing background music for GameplayMazeScene
            if (audioSource != null && backgroundMusic != null)
            {
                audioSource.clip = backgroundMusic;
                audioSource.volume = 1f; // ตั้งค่าเสียงเต็ม / Set volume to full
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

            // หยุดเพลงและทำให้ MusicFader หยุดทำงาน / Stop music and deactivate MusicFader
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
            StartFade(enemyMusic); // เปลี่ยนไปเล่นเพลงศัตรู / Switch to enemy music
        }
    }

    public void SwitchToBackgroundMusic()
    {
        if (isActive && audioSource != null && audioSource.clip != backgroundMusic)
        {
            StartFade(backgroundMusic); // เปลี่ยนกลับไปเพลงพื้นหลัง / Switch back to background music
        }
    }

    private void StartFade(AudioClip newClip)
    {
        if (currentFade != null)
        {
            StopCoroutine(currentFade); // หยุด Coroutine ก่อนหน้าถ้ามี / Stop any ongoing fade Coroutine
        }

        currentFade = StartCoroutine(FadeToNewClip(newClip));
    }

    private System.Collections.IEnumerator FadeToNewClip(AudioClip newClip)
    {
        if (audioSource.isPlaying)
        {
            // ลดเสียงเพลงปัจจุบัน / Fade out the current music
            float timer = 0f;
            float startVolume = audioSource.volume;

            while (timer < fadeDuration)
            {
                timer += Time.deltaTime;
                audioSource.volume = Mathf.Lerp(startVolume, 0f, timer / fadeDuration);
                yield return null;
            }

            audioSource.Stop(); // หยุดเพลงเดิม / Stop the current music
        }

        // เล่นเพลงใหม่และเพิ่มเสียง / Play new music and fade in
        audioSource.clip = newClip;
        audioSource.Play();

        float fadeInTimer = 0f;
        while (fadeInTimer < fadeDuration)
        {
            fadeInTimer += Time.deltaTime;
            audioSource.volume = Mathf.Lerp(0f, 1f, fadeInTimer / fadeDuration);
            yield return null;
        }

        audioSource.volume = 1f; // ตั้งค่าเสียงเต็ม / Set volume to full
        currentFade = null; // รีเซ็ต Coroutine / Reset the current fade Coroutine
    }
}
