using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    [SerializeField] private Toggle musicToggle;

    private void Start()
    {
        MusicController musicController = FindObjectOfType<MusicController>();
        if (musicController != null)
        {
            if (musicToggle != null)
            {
                musicController.UpdateToggleReference(musicToggle);
            }
            else
            {
                Debug.LogWarning("No Toggle assigned to UIController.");
            }
        }
        else
        {
            Debug.LogWarning("MusicController not found in the current scene.");
        }
    }
}
