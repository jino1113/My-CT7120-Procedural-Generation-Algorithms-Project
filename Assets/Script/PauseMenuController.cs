using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenuController : MonoBehaviour
{
    [SerializeField] private GameObject pauseMenuUI; // Reference to the Pause Menu UI / ตัว UI ของ Pause Menu
    private bool isGamePaused = false; // Tracks the game's pause state / ตรวจสอบสถานะของเกม

    void Update()
    {
        // Toggle Pause/Resume when the ESC key is pressed / กดปุ่ม ESC เพื่อสลับ Pause/Resume
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isGamePaused)
            {
                ResumeGame(); // Resume the game / กลับมาเล่นเกมต่อ
            }
            else
            {
                PauseGame(); // Pause the game / หยุดเกม
            }
        }
    }

    public void PauseGame()
    {
        pauseMenuUI.SetActive(true); // Show the Pause Menu / แสดงเมนู Pause
        Time.timeScale = 0f; // Stop in-game time / หยุดเวลาในเกม
        isGamePaused = true; // Update the game state to paused / อัปเดตสถานะเป็น Pause
    }

    public void ResumeGame()
    {
        pauseMenuUI.SetActive(false); // Hide the Pause Menu / ซ่อนเมนู Pause
        Time.timeScale = 1f; // Resume in-game time / เริ่มเวลาในเกม
        isGamePaused = false; // Update the game state to resumed / อัปเดตสถานะเป็น Resume
    }
}
