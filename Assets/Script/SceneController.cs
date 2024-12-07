using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    [SerializeField] private GameObject howToPlayWindow;
    [SerializeField] private GameObject settingWindow;

    [SerializeField] private PersistentMusicController musicController;

    void Start()
    {
        // หากไม่มี PersistentMusicController ใน Scene ให้ค้นหา
        if (musicController == null)
        {
            musicController = FindObjectOfType<PersistentMusicController>();
            if (musicController == null)
            {
                Debug.LogError("PersistentMusicController not found in the scene!");
            }
        }

        Time.timeScale = 1f; // คืนค่าเวลาให้ปกติ
    }

    public void ShowHowToPlay()
    {
        if (howToPlayWindow != null)
        {
            howToPlayWindow.SetActive(true);
            Time.timeScale = 0f; // หยุดเวลาในเกม
        }
        else
        {
            Debug.LogError("HowToPlay window not set in Inspector!");
        }
    }

    public void CloseHowToPlay()
    {
        if (howToPlayWindow != null)
        {
            howToPlayWindow.SetActive(false);
            Time.timeScale = 1f; // กลับมาเล่นเกมปกติ
        }
        else
        {
            Debug.LogError("HowToPlay window not set in Inspector!");
        }
    }

    public void ShowSetting()
    {
        if (settingWindow != null)
        {
            settingWindow.SetActive(true);
            Time.timeScale = 0f; // หยุดเวลาในเกม
        }
        else
        {
            Debug.LogError("Setting window not set in Inspector!");
        }
    }

    public void CloseSetting()
    {
        if (settingWindow != null)
        {
            settingWindow.SetActive(false);
            Time.timeScale = 1f; // กลับมาเล่นเกมปกติ
        }
        else
        {
            Debug.LogError("Setting window not set in Inspector!");
        }
    }

    public void LoadScene(string sceneName)
    {
        string currentScene = SceneManager.GetActiveScene().name; // เก็บชื่อ Scene ปัจจุบัน

        // ทำลาย BackgroundMusic ของ Scene ปัจจุบัน
        if (musicController != null)
        {
            musicController.DestroyBackgroundMusicInScene(currentScene);
        }

        Time.timeScale = 1f; // คืนเวลาให้ปกติ
        SceneManager.LoadScene(sceneName);
    }

    public void QuitGame()
    {
        Debug.Log("Game is exiting...");
        Time.timeScale = 1f;
        Application.Quit();
    }
}
