using UnityEngine;
using UnityEngine.UI;

public class MusicController : MonoBehaviour
{
    private static MusicController instance; // ตัวแปร Singleton เพื่อควบคุม MusicController / Singleton instance to manage MusicController
    [SerializeField] private Toggle musicToggle; // Toggle สำหรับควบคุมเพลง / Toggle for controlling music
    [SerializeField] private AudioSource backgroundMusic; // AudioSource สำหรับเล่นเพลง / AudioSource to play music
    [SerializeField] private AudioClip menuMusic; // เพลงสำหรับเมนู / Music clip for menu
    [SerializeField] private AudioClip gameMusic; // เพลงสำหรับเกม / Music clip for game

    private bool isPlaying = true; // สถานะเพลงกำลังเล่นหรือไม่ / Track whether music is playing

    private void Awake()
    {
        // ใช้ Singleton เพื่อป้องกันการซ้ำซ้อนของ MusicController / Use Singleton to prevent duplicate instances
        if (instance != null && instance != this)
        {
            Destroy(gameObject); // ทำลายตัวซ้ำ / Destroy duplicate instances
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject); // รักษา MusicController ข้าม Scene / Preserve MusicController across scenes
    }

    private void Start()
    {
        SyncToggleState(); // ซิงค์สถานะของ Toggle กับสถานะเพลงปัจจุบัน / Sync toggle with the current music state
    }

    public void PlayMenuMusic()
    {
        if (backgroundMusic.clip != menuMusic) // ตรวจสอบว่าเพลงเมนูกำลังเล่นอยู่หรือไม่ / Check if it's already playing menu music
        {
            backgroundMusic.clip = menuMusic;
            backgroundMusic.time = 0f; // เริ่มจากจุดเริ่มต้น / Start from the beginning
        }
        if (isPlaying) backgroundMusic.Play(); // เล่นเพลงถ้าสถานะเพลงเปิดอยู่ / Only play if music is enabled
        Debug.Log("Playing menu music.");
    }

    public void PlayGameMusic()
    {
        if (backgroundMusic.clip != gameMusic) // ตรวจสอบว่าเพลงเกมกำลังเล่นอยู่หรือไม่ / Check if it's already playing game music
        {
            backgroundMusic.clip = gameMusic;
            backgroundMusic.time = 0f; // เริ่มจากจุดเริ่มต้น / Start from the beginning
        }
        if (isPlaying) backgroundMusic.Play(); // เล่นเพลงถ้าสถานะเพลงเปิดอยู่ / Only play if music is enabled
        Debug.Log("Playing game music.");
    }

    public void StopMusic()
    {
        if (backgroundMusic.isPlaying)
        {
            backgroundMusic.Stop(); // หยุดเพลง / Stop the music
            Debug.Log("Music stopped.");
        }
    }

    public void ToggleMusic(bool isOn)
    {
        isPlaying = isOn;

        if (isOn)
        {
            backgroundMusic.Play(); // เล่นเพลง / Resume playing
            Debug.Log("Music started.");
        }
        else
        {
            backgroundMusic.Pause(); // หยุดชั่วคราว / Pause the music
            Debug.Log("Music paused.");
        }
    }

    public void UpdateToggleReference(Toggle newToggle)
    {
        if (musicToggle != null)
        {
            musicToggle.onValueChanged.RemoveListener(ToggleMusic); // ลบ Listener เก่าที่ผูกกับ Toggle เดิม / Remove old listener
        }

        musicToggle = newToggle;

        if (musicToggle != null)
        {
            musicToggle.onValueChanged.AddListener(ToggleMusic); // เพิ่ม Listener ใหม่ให้กับ Toggle / Add new listener
            SyncToggleState(); // ซิงค์สถานะของ Toggle กับสถานะเพลง / Sync toggle state with music status
        }
    }

    public void SyncToggleState()
    {
        if (musicToggle != null)
        {
            musicToggle.isOn = isPlaying; // อัปเดตสถานะของ Toggle ให้ตรงกับสถานะเพลง / Update toggle's visual state
        }
    }
}
