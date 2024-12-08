using UnityEngine;

public class PersistentMusicController : MonoBehaviour
{
    private static PersistentMusicController instance; // ตัวแปร Singleton สำหรับควบคุม PersistentMusicController / Singleton instance to manage PersistentMusicController
    private AudioSource audioSource; // อ้างอิงถึง AudioSource / Reference to the AudioSource component

    void Awake()
    {
        // ใช้ Singleton pattern เพื่อป้องกันการมี PersistentMusicController หลายตัว / Use Singleton pattern to avoid multiple PersistentMusicController instances
        if (instance != null && instance != this)
        {
            Destroy(gameObject); // ทำลายตัวซ้ำ / Destroy duplicate instances
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject); // รักษา Controller ข้าม Scene / Preserve this GameObject across scenes

        // ตรวจสอบว่า AudioSource ถูกเพิ่มเข้ามาหรือไม่ / Check if AudioSource is attached
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            Debug.LogError("No AudioSource found on PersistentMusicController!"); // แจ้งเตือนถ้าไม่มี AudioSource / Log an error if no AudioSource is found
        }
    }

    public void PlayMusic()
    {
        // เริ่มเล่นเพลงถ้ายังไม่ได้เล่นอยู่ / Play music if not already playing
        if (audioSource != null && !audioSource.isPlaying)
        {
            audioSource.Play();
            Debug.Log("Music started."); // แจ้งว่าดนตรีเริ่มเล่น / Log that music has started
        }
    }

    public void StopMusic()
    {
        // หยุดเพลงถ้ากำลังเล่นอยู่ / Stop music if it is currently playing
        if (audioSource != null && audioSource.isPlaying)
        {
            audioSource.Stop();
            Debug.Log("Music stopped."); // แจ้งว่าดนตรีหยุดเล่น / Log that music has stopped
        }
    }

    public bool IsMusicPlaying()
    {
        // ตรวจสอบว่าเพลงกำลังเล่นอยู่หรือไม่ / Check if music is currently playing
        return audioSource != null && audioSource.isPlaying;
    }
}
