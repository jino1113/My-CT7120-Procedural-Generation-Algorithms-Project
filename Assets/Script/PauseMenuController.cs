using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class PauseMenuController : MonoBehaviour
{
    [SerializeField] private GameObject pauseMenuUI; // อ้างอิงถึง UI ของ Pause Menu / Reference to the Pause Menu UI
    private CinemachineBrain cinemachineBrain; // อ้างอิงถึง Cinemachine Brain / Reference to Cinemachine Brain
    private bool isGamePaused = false; // ติดตามสถานะการหยุดเกม / Tracks the game's pause state

    void Start()
    {
        // ค้นหา Cinemachine Brain ใน Scene / Find Cinemachine Brain in the scene
        cinemachineBrain = FindObjectOfType<CinemachineBrain>();
        if (cinemachineBrain == null)
        {
            // Debug.LogWarning("Cinemachine Brain not found in the scene.");
        }
    }

    void Update()
    {
        // กดปุ่ม ESC เพื่อสลับสถานะ Pause/Resume / Toggle Pause/Resume when the ESC key is pressed
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isGamePaused)
            {
                ResumeGame(); // กลับมาเล่นเกมต่อ / Resume the game
            }
            else
            {
                PauseGame(); // หยุดเกม / Pause the game
            }
        }
    }

    public void PauseGame()
    {
        pauseMenuUI.SetActive(true); // แสดง Pause Menu / Show the Pause Menu
        Time.timeScale = 0f; // หยุดเวลาของเกม / Stop in-game time
        isGamePaused = true; // อัปเดตสถานะเกมเป็นหยุดชั่วคราว / Update the game state to paused

        // ปลดล็อกและแสดงเคอร์เซอร์ / Unlock and show the cursor
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        if (cinemachineBrain != null)
        {
            cinemachineBrain.enabled = false; // ปิดการทำงานของ Cinemachine Brain / Disable Cinemachine Brain
        }
    }

    public void ResumeGame()
    {
        pauseMenuUI.SetActive(false); // ซ่อน Pause Menu / Hide the Pause Menu
        Time.timeScale = 1f; // เริ่มเวลาในเกมใหม่ / Resume in-game time
        isGamePaused = false; // อัปเดตสถานะเกมเป็นเล่นต่อ / Update the game state to resumed

        // ล็อกและซ่อนเคอร์เซอร์ / Lock and hide the cursor
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        if (cinemachineBrain != null)
        {
            cinemachineBrain.enabled = true; // เปิดการทำงานของ Cinemachine Brain / Enable Cinemachine Brain
        }
    }

    public void PausePreviewGame()
    {
        pauseMenuUI.SetActive(true); // แสดง Pause Menu / Show the Pause Menu
        Time.timeScale = 0f; // หยุดเวลาของเกม / Stop in-game time
        isGamePaused = true; // อัปเดตสถานะเกมเป็นหยุดชั่วคราว / Update the game state to paused
    }

    public void ResumePreviewGame()
    {
        pauseMenuUI.SetActive(false); // ซ่อน Pause Menu / Hide the Pause Menu
        Time.timeScale = 1f; // เริ่มเวลาในเกมใหม่ / Resume in-game time
        isGamePaused = false; // อัปเดตสถานะเกมเป็นเล่นต่อ / Update the game state to resumed
    }
}
