using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    [SerializeField] private Toggle musicToggle; // อ้างอิงถึง Toggle สำหรับควบคุมเพลง / Reference to the Toggle for controlling music

    private void Start()
    {
        // ค้นหา MusicController ใน Scene / Find the MusicController in the current scene
        MusicController musicController = FindObjectOfType<MusicController>();
        if (musicController != null)
        {
            if (musicToggle != null)
            {
                // อัปเดตการเชื่อมโยง Toggle ใน MusicController / Update the Toggle reference in MusicController
                musicController.UpdateToggleReference(musicToggle);
            }
            else
            {
                Debug.LogWarning("No Toggle assigned to UIController."); // แจ้งเตือนถ้า Toggle ไม่ได้ถูกตั้งค่า / Warn if the Toggle is not assigned
            }
        }
        else
        {
            Debug.LogWarning("MusicController not found in the current scene."); // แจ้งเตือนถ้า MusicController ไม่ถูกพบ / Warn if MusicController is not found
        }
    }
}
