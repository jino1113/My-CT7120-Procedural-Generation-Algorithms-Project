using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenuController : MonoBehaviour
{
    [SerializeField] private GameObject pauseMenuUI; // ตัว UI ของ Pause Menu
    private bool isGamePaused = false; // ตรวจสอบสถานะของเกม

    void Update()
    {
        // กดปุ่ม ESC เพื่อสลับ Pause/Resume
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isGamePaused)
            {
                ResumeGame(); // กลับมาเล่นเกมต่อ
            }
            else
            {
                PauseGame(); // หยุดเกม
            }
        }
    }

    public void PauseGame()
    {
        pauseMenuUI.SetActive(true); // แสดงเมนู Pause
        Time.timeScale = 0f; // หยุดเวลาในเกม
        isGamePaused = true; // อัปเดตสถานะเป็น Pause
    }

    public void ResumeGame()
    {
        pauseMenuUI.SetActive(false); // ซ่อนเมนู Pause
        Time.timeScale = 1f; // เริ่มเวลาในเกม
        isGamePaused = false; // อัปเดตสถานะเป็น Resume
    }
}

