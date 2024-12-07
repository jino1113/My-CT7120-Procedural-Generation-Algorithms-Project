using UnityEngine;

public class MusicFader : MonoBehaviour
{
    public AudioSource audioSource; // AudioSource ตัวเดียว
    public AudioClip backgroundMusic; // เพลงพื้นหลัง
    public AudioClip enemyMusic; // เพลงศัตรู
    public float fadeDuration = 1f; // ระยะเวลา fade

    private Coroutine currentFade; // เก็บข้อมูล fade ที่กำลังทำงานอยู่

    void Start()
    {
        // เริ่มต้นเล่นเพลงพื้นหลัง
        if (audioSource != null && backgroundMusic != null)
        {
            audioSource.clip = backgroundMusic;
            audioSource.Play();
        }
    }

    // ฟังก์ชันเปลี่ยนเพลงเป็นเพลงศัตรู
    public void SwitchToEnemyMusic()
    {
        if (audioSource != null && audioSource.clip != enemyMusic)
        {
            StartFade(enemyMusic);
        }
    }

    // ฟังก์ชันเปลี่ยนกลับไปเพลงพื้นหลัง
    public void SwitchToBackgroundMusic()
    {
        if (audioSource != null && audioSource.clip != backgroundMusic)
        {
            StartFade(backgroundMusic);
        }
    }

    // ฟังก์ชันเริ่ม fade เพลงใหม่
    private void StartFade(AudioClip newClip)
    {
        if (currentFade != null)
        {
            StopCoroutine(currentFade); // หยุด fade ก่อนหน้าถ้ายังทำงานอยู่
        }

        currentFade = StartCoroutine(FadeToNewClip(newClip));
    }

    // Coroutine สำหรับ fade ระหว่างเพลง
    private System.Collections.IEnumerator FadeToNewClip(AudioClip newClip)
    {
        if (audioSource.isPlaying)
        {
            // ค่อยๆ ลดเสียงเพลงปัจจุบัน
            float timer = 0f;
            float startVolume = audioSource.volume;

            while (timer < fadeDuration)
            {
                timer += Time.deltaTime;
                audioSource.volume = Mathf.Lerp(startVolume, 0f, timer / fadeDuration);
                yield return null;
            }

            // เปลี่ยนเพลงใหม่
            audioSource.Stop();
        }

        audioSource.clip = newClip;
        audioSource.Play();

        // ค่อยๆ เพิ่มเสียงเพลงใหม่
        float fadeInTimer = 0f;
        while (fadeInTimer < fadeDuration)
        {
            fadeInTimer += Time.deltaTime;
            audioSource.volume = Mathf.Lerp(0f, 1f, fadeInTimer / fadeDuration);
            yield return null;
        }

        audioSource.volume = 1f;
        currentFade = null; // รีเซ็ต currentFade
    }
}
