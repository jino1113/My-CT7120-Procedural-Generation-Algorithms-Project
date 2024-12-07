using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class MusicController : MonoBehaviour
{
    public List<AudioSource> musicSources; // รายการเพลงที่ต้องการควบคุม
    public Toggle musicToggle; // ปุ่ม Toggle สำหรับเปิด/ปิดเพลง
    private int currentMusicIndex = 0; // เพลงปัจจุบันที่เล่นอยู่

    void Start()
    {
        // หาก Toggle ยังไม่ได้เชื่อมโยงใน Inspector ให้ลองค้นหาใน Scene
        if (musicToggle == null)
        {
            musicToggle = FindObjectOfType<Toggle>();

            if (musicToggle == null)
            {
                Debug.LogWarning("Music Toggle not found in the new scene!");
                return;
            }

            // ตั้งค่า Listener สำหรับ Toggle ใหม่
            musicToggle.onValueChanged.AddListener(delegate { ToggleMusic(musicToggle.isOn); });
            musicToggle.isOn = musicSources.Count > 0 && musicSources[0].isPlaying; // อัปเดตสถานะ Toggle
        }
    }

    public void ToggleMusic(bool isOn)
    {
        // เปิด/ปิดเพลงทั้งหมดในรายการ
        foreach (var musicSource in musicSources)
        {
            if (musicSource != null)
            {
                if (isOn)
                {
                    musicSource.Play(); // เล่นเพลง
                }
                else
                {
                    musicSource.Pause(); // หยุดเพลง
                }
            }
        }
    }

    public void PlaySpecificMusic(int musicIndex)
    {
        if (musicIndex < 0 || musicIndex >= musicSources.Count) return; // ตรวจสอบ index ให้อยู่ในช่วงที่ถูกต้อง

        // หยุดเพลงปัจจุบันทั้งหมด
        StopAllMusic();

        // เล่นเพลงใหม่ตาม index
        currentMusicIndex = musicIndex;
        musicSources[currentMusicIndex].Play();
    }

    public void StopAllMusic()
    {
        foreach (var musicSource in musicSources)
        {
            if (musicSource.isPlaying)
            {
                musicSource.Stop(); // หยุดเพลง
            }
        }
    }
}
