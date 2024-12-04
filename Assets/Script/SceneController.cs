using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    [SerializeField] private GameObject howToPlayWindow;

    public void ShowHowToPlay()
    {
        howToPlayWindow.SetActive(true); // แสดงหน้าต่าง
    }

    public void CloseHowToPlay()
    {
        howToPlayWindow.SetActive(false); // ปิดหน้าต่าง
    }

    private void Update()
    {
        // ตรวจสอบการกดปุ่ม ESC
        if (howToPlayWindow.activeSelf && Input.GetKeyDown(KeyCode.Escape))
        {
            CloseHowToPlay(); // ปิดหน้าต่างเมื่อกด ESC
        }
    }


    // ฟังก์ชันสำหรับเปลี่ยน Scene
    // Function to switch between scenes
    public void LoadScene(string sceneName)
    {
        // ตั้งค่า Time.timeScale เป็น 1 เพื่อให้เวลาในเกมทำงานปกติ
        // Set Time.timeScale to 1 to ensure game time runs normally
        Time.timeScale = 1f;

        // โหลด Scene ตามชื่อที่ส่งมา
        // Load the scene specified by the sceneName parameter
        SceneManager.LoadScene(sceneName);
    }

    // ฟังก์ชันสำหรับออกจากเกม
    // Function to quit the game
    public void QuitGame()
    {
        // แสดงข้อความใน Console (จะเห็นผลเฉพาะใน Unity Editor)
        // Display a log message in the Console (visible only in the Unity Editor)
        Debug.Log("Game is exiting...");

        // ออกจากเกม (จะทำงานเฉพาะใน Build)
        // Exit the game (works only in the build version)
        Application.Quit();
    }
}
