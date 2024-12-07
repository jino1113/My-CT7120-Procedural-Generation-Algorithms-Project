using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    [SerializeField] private GameObject settingWindow; // Reference to Setting UI
    [SerializeField] private GameObject howToPlayWindow; // Reference to HowToPlay UI
    private MusicController musicController;

    private void Start()
    {
        // Find MusicController if not linked in Inspector
        musicController = FindObjectOfType<MusicController>();
        if (musicController == null)
        {
            Debug.LogWarning("MusicController not found in the current scene.");
        }

        // Handle music based on the current scene
        string currentScene = SceneManager.GetActiveScene().name;

        if (currentScene == "MenuScene")
        {
            musicController?.PlayMenuMusic(); // Play menu music
        }
        else if (currentScene == "GameplayMazeScene")
        {
            musicController?.PlayGameMusic(); // Play game music
        }

        Time.timeScale = 1f; // Reset time scale
    }

    public void LoadScene(string sceneName)
    {
        // Handle music or stop music based on the target scene
        if (sceneName == "MenuScene")
        {
            musicController?.PlayMenuMusic(); // Play menu music
        }
        else if (sceneName == "GameplayMazeScene")
        {
            musicController?.PlayGameMusic(); // Play game music
        }

        SceneManager.LoadScene(sceneName); // Load the target scene
    }

    public void ShowSetting()
    {
        ToggleUI(settingWindow, true); // Show the setting window
    }

    public void CloseSetting()
    {
        ToggleUI(settingWindow, false); // Close the setting window
    }

    public void ShowHowToPlay()
    {
        ToggleUI(howToPlayWindow, true); // Show the HowToPlay window
    }

    public void CloseHowToPlay()
    {
        ToggleUI(howToPlayWindow, false); // Close the HowToPlay window
    }

    private void ToggleUI(GameObject uiWindow, bool isActive)
    {
        if (uiWindow != null)
        {
            uiWindow.SetActive(isActive);
            Time.timeScale = isActive ? 0f : 1f; // Pause or resume game time
        }
        else
        {
            Debug.LogError("UI Window is not assigned.");
        }
    }

    public void PauseGame()
    {
        Time.timeScale = 0f;
        Debug.Log("Game Paused.");
    }

    public void ResumeGame()
    {
        Time.timeScale = 1f;
        Debug.Log("Game Resumed.");
    }

    public void QuitGame()
    {
        Debug.Log("Game is exiting...");
        Time.timeScale = 1f;
        Application.Quit();
    }
}
