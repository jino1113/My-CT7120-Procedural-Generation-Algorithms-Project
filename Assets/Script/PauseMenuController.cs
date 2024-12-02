using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class PauseMenuController : MonoBehaviour
{
    [SerializeField] private GameObject pauseMenuUI; // Reference to the Pause Menu UI
    private CinemachineBrain cinemachineBrain; // Reference to Cinemachine Brain
    private bool isGamePaused = false; // Tracks the game's pause state

    void Start()
    {
        // Find Cinemachine Brain in the scene
        cinemachineBrain = FindObjectOfType<CinemachineBrain>();
        if (cinemachineBrain == null)
        {
            Debug.LogWarning("Cinemachine Brain not found in the scene.");
        }
    }

    void Update()
    {
        // Toggle Pause/Resume when the ESC key is pressed
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isGamePaused)
            {
                ResumeGame(); // Resume the game
            }
            else
            {
                PauseGame(); // Pause the game
            }
        }
    }

    public void PauseGame()
    {
        pauseMenuUI.SetActive(true); // Show the Pause Menu
        Time.timeScale = 0f; // Stop in-game time
        isGamePaused = true; // Update the game state to paused

        // ª≈¥≈ÁÕ§·≈–· ¥ß‡§Õ√Ï‡´Õ√Ï
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        if (cinemachineBrain != null)
        {
            cinemachineBrain.enabled = false; // Disable Cinemachine Brain
        }
    }

    public void ResumeGame()
    {
        pauseMenuUI.SetActive(false); // Hide the Pause Menu
        Time.timeScale = 1f; // Resume in-game time
        isGamePaused = false; // Update the game state to resumed

        // ≈ÁÕ§·≈–´ËÕπ‡§Õ√Ï‡´Õ√Ï
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        if (cinemachineBrain != null)
        {
            cinemachineBrain.enabled = true; // Enable Cinemachine Brain
        }
    }
}
