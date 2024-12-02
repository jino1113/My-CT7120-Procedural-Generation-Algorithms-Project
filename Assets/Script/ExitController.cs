using UnityEngine;

public class ExitController : MonoBehaviour
{
    // Name of the UI GameObject in the Canvas / ชื่อของ UI GameObject ใน Canvas
    public string uiName = "winUI";

    private GameObject winUI; // Reference to the Win UI / ตัวอ้างอิงถึง Win UI

    private void Start()
    {
        // Find the UI GameObject by name / ค้นหา UI GameObject ผ่านชื่อ
        winUI = GameObject.Find(uiName);

        if (winUI == null)
        {
            // Display error if the UI is not found / แสดงข้อผิดพลาดถ้าไม่พบ UI
            Debug.LogError($"GameObject with name '{uiName}' not found in the scene.");
        }
        else
        {
            // Disable the UI at the start of the game / ปิด UI ตั้งแต่เริ่มต้น
            winUI.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // Check if the player collided with the Exit Prefab / ตรวจสอบว่าผู้เล่นชนกับ Exit Prefab หรือไม่
        if (other.CompareTag("Player"))
        {
            if (winUI != null)
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                // Enable the Win UI / เปิด UI เมื่อผู้เล่นชนกับ Exit Prefab
                winUI.SetActive(true);

                // Pause the game / หยุดเกม
                Time.timeScale = 0f;

                // Log the action for debugging / บันทึกการกระทำสำหรับตรวจสอบ
                Debug.Log("Player reached the exit. Game paused and UI displayed!");
            }
        }
    }

    // Function to resume the game when the UI is closed / ฟังก์ชันสำหรับ Resume เกมเมื่อปิด UI
    public void ResumeGame()
    {
        if (winUI != null)
        {
            // Disable the Win UI / ปิด UI
            winUI.SetActive(false);

            // Resume the game by resetting time scale / เริ่มเกมใหม่โดยตั้งค่า Time.timeScale กลับเป็น 1
            Time.timeScale = 1f;

            // Log the action for debugging / บันทึกการกระทำสำหรับตรวจสอบ
            Debug.Log("Game resumed.");
        }
    }
}
