using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    // ฟังก์ชันสำหรับเปลี่ยน Scene
    public void LoadScene(string sceneName)
    {
        // โหลด Scene ตามชื่อที่ส่งมา
        SceneManager.LoadScene(sceneName);
    }

    // ฟังก์ชันสำหรับออกจากเกม
    public void QuitGame()
    {
        Debug.Log("Game is exiting..."); // แสดงข้อความใน Console (จะเห็นผลเฉพาะใน Editor)
        Application.Quit(); // ออกจากเกม
    }
}
