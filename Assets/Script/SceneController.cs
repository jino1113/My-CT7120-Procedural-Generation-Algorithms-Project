using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    [SerializeField] private AudioClip menuMusic;
    [SerializeField] private AudioClip previewMusic;

    [SerializeField] private GameObject settingWindow; // อ้างอิงถึง Setting UI
    [SerializeField] private GameObject howToPlayWindow; // อ้างอิงถึง HowToPlay UI

    private PersistentMusicController musicController;

    void Start()
    {
        musicController = FindObjectOfType<PersistentMusicController>();

        string currentScene = SceneManager.GetActiveScene().name;

        // กำหนดเพลงตาม Scene
        if (currentScene == "MenuScene")
        {
            musicController?.PlayMusic(menuMusic);
        }
        else if (currentScene == "MazeGenPreviewScene" || currentScene == "PerlinNoiseScene")
        {
            musicController?.PlayMusic(previewMusic);
        }

        Time.timeScale = 1f; // คืนค่าเวลาให้ปกติเมื่อเริ่ม Scene ใหม่
    }

    public void LoadScene(string sceneName)
    {
        string currentScene = SceneManager.GetActiveScene().name;

        // ถ้าเปลี่ยนไป GameplayMazeScene ให้ทำลาย BackgroundMusic
        if (sceneName == "GameplayMazeScene" && musicController != null)
        {
            Destroy(musicController.gameObject); // ลบ PersistentMusicController
        }

        // คืนค่าเวลาให้ปกติก่อนเปลี่ยน Scene
        Time.timeScale = 1f;

        // เปลี่ยน Scene
        SceneManager.LoadScene(sceneName);

        // กำหนดเพลงสำหรับ Scene ใหม่
        if (sceneName == "MenuScene")
        {
            musicController?.PlayMusic(menuMusic);
        }
        else if (sceneName == "MazeGenPreviewScene" || sceneName == "PerlinNoiseScene")
        {
            musicController?.PlayMusic(previewMusic);
        }
    }

    // แสดง Setting UI
    public void ShowSetting()
    {
        if (settingWindow != null)
        {
            settingWindow.SetActive(true);
            Time.timeScale = 0f; // หยุดเวลาในเกม
        }
        else
        {
            Debug.LogError("Setting Window is not assigned in the Inspector!");
        }
    }

    // ปิด Setting UI
    public void CloseSetting()
    {
        if (settingWindow != null)
        {
            settingWindow.SetActive(false);
            Time.timeScale = 1f; // กลับมาเล่นเกมตามปกติ
        }
        else
        {
            Debug.LogError("Setting Window is not assigned in the Inspector!");
        }
    }

    // แสดง HowToPlay UI
    public void ShowHowToPlay()
    {
        if (howToPlayWindow != null)
        {
            howToPlayWindow.SetActive(true);
            Time.timeScale = 0f; // หยุดเวลาในเกม
        }
        else
        {
            Debug.LogError("HowToPlay Window is not assigned in the Inspector!");
        }
    }

    // ปิด HowToPlay UI
    public void CloseHowToPlay()
    {
        if (howToPlayWindow != null)
        {
            howToPlayWindow.SetActive(false);
            Time.timeScale = 1f; // กลับมาเล่นเกมตามปกติ
        }
        else
        {
            Debug.LogError("HowToPlay Window is not assigned in the Inspector!");
        }
    }

    public void PauseGame()
    {
        Time.timeScale = 0f; // หยุดเวลาในเกม
        Debug.Log("Game Paused.");
    }

    public void ResumeGame()
    {
        Time.timeScale = 1f; // คืนค่าเวลาให้ปกติ
        Debug.Log("Game Resumed.");
    }

    public void QuitGame()
    {
        Debug.Log("Game is exiting...");
        Time.timeScale = 1f; // คืนค่าเวลาให้ปกติก่อนออกจากเกม
        Application.Quit();
    }
}
