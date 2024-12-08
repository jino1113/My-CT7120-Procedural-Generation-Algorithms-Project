using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CubeScript : MonoBehaviour
{
    public string targetTag = "Cube"; // Tag for target objects / แท็กสำหรับวัตถุเป้าหมาย
    public float maxOffset = 20f; // Maximum height offset for noise scaling / ความสูงสูงสุดของการเปลี่ยนแปลง (ค่าเริ่มต้น)

    private GameObject[] cubes; // Array to store GameObjects with the target tag / เก็บ GameObjects ที่มีแท็กเป้าหมาย

    [Header("UI Elements")]
    public TMP_InputField maxOffsetInputField; // InputField for adjusting maxOffset / InputField สำหรับการปรับค่า maxOffset
    public TextMeshProUGUI debugText; // TextMeshPro element for displaying debug messages / สำหรับแสดงข้อความ debug

    private Coroutine debugCoroutine; // Reference to the debug Coroutine / ใช้เก็บสถานะ Coroutine

    void Start()
    {
        // Find all GameObjects with the specified target tag / ค้นหา GameObjects ทั้งหมดที่มีแท็กเป้าหมาย
        cubes = GameObject.FindGameObjectsWithTag(targetTag);

        if (cubes.Length == 0)
        {
            Debug.LogWarning($"No objects found with tag '{targetTag}'!"); // Warning if no objects are found / แจ้งเตือนถ้าไม่มีวัตถุที่มีแท็กนี้
        }

        // Update the maxOffset value from the InputField at the start / อัปเดตค่า maxOffset จาก InputField เมื่อเริ่มต้น
        if (maxOffsetInputField != null)
        {
            maxOffsetInputField.text = maxOffset.ToString(); // Set the default value in the InputField / กำหนดค่าเริ่มต้นใน InputField
        }
    }

    void Update()
    {
        foreach (GameObject cube in cubes)
        {
            // Calculate Perlin noise and adjust the height of each object / คำนวณ noise และปรับความสูงของแต่ละวัตถุ
            float noise = Mathf.PerlinNoise(cube.transform.position.x / 10 + Time.time, cube.transform.position.z / 10 + Time.time);
            cube.transform.localScale = new Vector3(1f, noise * maxOffset, 1f); // Adjust the Y scale based on noise / ปรับขนาดแกน Y ตามค่า noise
        }
    }

    public void UpdateMaxOffset()
    {
        if (maxOffsetInputField != null)
        {
            if (float.TryParse(maxOffsetInputField.text, out float newMaxOffset))
            {
                if (newMaxOffset > 30f) // Limit maxOffset to 30 / ถ้าค่าเกิน 30
                {
                    ShowDebugMessage("Value exceeds maximum limit (30). Max Offset remains unchanged."); // Display error message / แสดงข้อความแจ้งข้อผิดพลาด
                    maxOffsetInputField.text = maxOffset.ToString(); // Revert InputField to the current value / คืนค่า InputField กลับเป็นค่าเดิม
                }
                else
                {
                    maxOffset = newMaxOffset; // Update maxOffset value / อัปเดตค่า maxOffset
                    ShowDebugMessage($"Max Offset updated to: {maxOffset}"); // Show success message / แสดงข้อความยืนยัน
                }
            }
            else
            {
                ShowDebugMessage("Invalid input! Please enter a valid number."); // Error for invalid input / แสดงข้อความข้อผิดพลาดสำหรับค่าไม่ถูกต้อง
                maxOffsetInputField.text = maxOffset.ToString(); // Revert InputField to the current value / คืนค่า InputField กลับเป็นค่าเดิม
            }
        }
    }

    private void ShowDebugMessage(string message)
    {
        if (debugText != null)
        {
            if (debugCoroutine != null)
            {
                StopCoroutine(debugCoroutine); // Stop any existing debug message / หยุดข้อความ debug ที่แสดงอยู่
            }

            debugCoroutine = StartCoroutine(DisplayDebugMessage(message)); // Start new debug message / เริ่มข้อความ debug ใหม่
        }
        else
        {
            Debug.Log(message); // Log message in the console if no UI debug is available / แสดงข้อความใน Console ถ้าไม่มี UI Debug
        }
    }

    private IEnumerator DisplayDebugMessage(string message)
    {
        debugText.text = message; // Display the debug message in the UI / แสดงข้อความ debug ใน UI
        debugText.color = new Color(debugText.color.r, debugText.color.g, debugText.color.b, 1);

        yield return new WaitForSeconds(1f); // Show the message for 1 second / แสดงข้อความ 1 วินาที

        for (float alpha = 1; alpha > 0; alpha -= Time.deltaTime)
        {
            debugText.color = new Color(debugText.color.r, debugText.color.g, debugText.color.b, alpha); // Fade out the text / ทำข้อความจางหายไป
            yield return null;
        }

        debugText.text = ""; // Clear the debug message / ลบข้อความ
        debugCoroutine = null; // Reset the coroutine reference / รีเซ็ตการอ้างอิง Coroutine
    }
}
