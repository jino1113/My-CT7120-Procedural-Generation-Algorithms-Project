using UnityEngine;

public class ExitController : MonoBehaviour
{
    public string uiName = "winUI"; // ชื่อของ UI GameObject ใน Canvas

    private GameObject winUI;

    private void Start()
    {
        // ค้นหา UI GameObject ผ่านชื่อ
        winUI = GameObject.Find(uiName);

        if (winUI == null)
        {
            Debug.LogError($"GameObject with name '{uiName}' not found in the scene.");
        }
        else
        {
            winUI.SetActive(false); // ปิด UI ตั้งแต่เริ่มต้น
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) // ตรวจสอบว่าผู้เล่นชนหรือไม่
        {
            if (winUI != null)
            {
                winUI.SetActive(true); // เปิด UI เมื่อผู้เล่นชนกับ Exit Prefab
                Time.timeScale = 0f;  // หยุดเกม
                Debug.Log("Player reached the exit. Game paused and UI displayed!");
            }
        }
    }

    // ฟังก์ชันสำหรับ Resume เกมเมื่อปิด UI
    public void ResumeGame()
    {
        if (winUI != null)
        {
            winUI.SetActive(false); // ปิด UI
            Time.timeScale = 1f;    // เริ่มเกมใหม่
            Debug.Log("Game resumed.");
        }
    }
}
