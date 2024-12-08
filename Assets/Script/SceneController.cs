using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    [SerializeField] private GameObject settingWindow; // อ้างอิงถึง UI การตั้งค่า / Reference to Setting UI
    [SerializeField] private GameObject howToPlayWindow; // อ้างอิงถึง UI วิธีเล่น / Reference to HowToPlay UI
    private MusicController musicController; // อ้างอิงถึง MusicController / Reference to MusicController

    private void Start()
    {
        // ค้นหา MusicController หากยังไม่ได้เชื่อมโยงใน Inspector / Find MusicController if not linked in Inspector
        musicController = FindObjectOfType<MusicController>();
        if (musicController == null)
        {
            //Debug.LogWarning("MusicController not found in the current scene."); // แจ้งเตือนหากไม่พบ MusicController / Log warning if MusicController not found
        }

        // จัดการเพลงตาม Scene ปัจจุบัน / Handle music based on the current scene
        string currentScene = SceneManager.GetActiveScene().name;

        if (currentScene == "MenuScene")
        {
            musicController?.PlayMenuMusic(); // เล่นเพลงเมนู / Play menu music
        }
        else if (currentScene == "GameplayMazeScene")
        {
            musicController?.PlayGameMusic(); // เล่นเพลงเกม / Play game music
        }

        Time.timeScale = 1f; // รีเซ็ตเวลาให้เป็นปกติ / Reset time scale
    }

    public void LoadScene(string sceneName)
    {
        // จัดการเพลงหรือหยุดเพลงตาม Scene เป้าหมาย / Handle music or stop music based on the target scene
        if (sceneName == "MenuScene")
        {
            musicController?.PlayMenuMusic(); // เล่นเพลงเมนู / Play menu music
        }
        else if (sceneName == "GameplayMazeScene")
        {
            musicController?.PlayGameMusic(); // เล่นเพลงเกม / Play game music
        }

        SceneManager.LoadScene(sceneName); // โหลด Scene เป้าหมาย / Load the target scene
    }

    public void ShowSetting()
    {
        ToggleUI(settingWindow, true); // แสดงหน้าต่างตั้งค่า / Show the setting window
    }

    public void CloseSetting()
    {
        ToggleUI(settingWindow, false); // ปิดหน้าต่างตั้งค่า / Close the setting window
    }

    public void ShowHowToPlay()
    {
        ToggleUI(howToPlayWindow, true); // แสดงหน้าต่างวิธีเล่น / Show the HowToPlay window
    }

    public void CloseHowToPlay()
    {
        ToggleUI(howToPlayWindow, false); // ปิดหน้าต่างวิธีเล่น / Close the HowToPlay window
    }

    private void ToggleUI(GameObject uiWindow, bool isActive)
    {
        if (uiWindow != null)
        {
            uiWindow.SetActive(isActive); // เปิดหรือปิด UI / Show or hide the UI
            Time.timeScale = isActive ? 0f : 1f; // หยุดหรือคืนเวลาในเกม / Pause or resume game time
        }
        else
        {
            //Debug.LogError("UI Window is not assigned."); // แจ้งเตือนหากไม่ได้ตั้งค่า UI / Log error if UI is not assigned
        }
    }

    public void PauseGame()
    {
        Time.timeScale = 0f; // หยุดเวลาในเกม / Pause the game time
        Debug.Log("Game Paused."); // แจ้งข้อความว่าเกมถูกหยุด / Log game paused message
    }

    public void ResumeGame()
    {
        Time.timeScale = 1f; // คืนค่าเวลาในเกมให้ปกติ / Resume the game time
        //Debug.Log("Game Resumed."); // แจ้งข้อความว่าเกมดำเนินต่อ / Log game resumed message
    }

    public void QuitGame()
    {
        //Debug.Log("Game is exiting..."); // แจ้งข้อความว่าเกมกำลังออก / Log game exiting message
        Time.timeScale = 1f; // คืนค่าเวลาในเกมให้ปกติ / Reset time scale
        Application.Quit(); // ออกจากเกม / Quit the application
    }
}
