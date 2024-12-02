using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CubeScript : MonoBehaviour
{
    public string targetTag = "Cube"; // กำหนดแท็กสำหรับวัตถุเป้าหมาย
    public float maxOffset = 20f; // ความสูงสูงสุดของการเปลี่ยนแปลง (ค่าเริ่มต้น)

    private GameObject[] cubes; // เก็บ GameObjects ที่มีแท็กเป้าหมาย

    [Header("UI Elements")]
    public TMP_InputField maxOffsetInputField; // InputField สำหรับการปรับค่า maxOffset
    public TextMeshProUGUI debugText; // สำหรับแสดงข้อความ debug

    private Coroutine debugCoroutine; // ใช้เก็บสถานะ Coroutine

    void Start()
    {
        // ค้นหา GameObjects ทั้งหมดที่มีแท็กเป้าหมาย
        cubes = GameObject.FindGameObjectsWithTag(targetTag);

        if (cubes.Length == 0)
        {
            Debug.LogWarning($"No objects found with tag '{targetTag}'!");
        }

        // อัปเดตค่า maxOffset จาก InputField เมื่อเริ่มต้น
        if (maxOffsetInputField != null)
        {
            maxOffsetInputField.text = maxOffset.ToString(); // กำหนดค่าเริ่มต้นใน InputField
        }
    }

    void Update()
    {
        foreach (GameObject cube in cubes)
        {
            // คำนวณ noise และปรับความสูงของแต่ละวัตถุ
            float noise = Mathf.PerlinNoise(cube.transform.position.x / 10 + Time.time, cube.transform.position.z / 10 + Time.time);
            cube.transform.localScale = new Vector3(1f, noise * maxOffset, 1f);
        }
    }

    public void UpdateMaxOffset()
    {
        if (maxOffsetInputField != null)
        {
            if (float.TryParse(maxOffsetInputField.text, out float newMaxOffset))
            {
                if (newMaxOffset > 30f) // ถ้าค่าเกิน 30
                {
                    ShowDebugMessage("Value exceeds maximum limit (30). Max Offset remains unchanged.");
                    maxOffsetInputField.text = maxOffset.ToString(); // คืนค่า InputField กลับเป็นค่าเดิม
                }
                else
                {
                    maxOffset = newMaxOffset; // อัปเดตค่า maxOffset
                    ShowDebugMessage($"Max Offset updated to: {maxOffset}");
                }
            }
            else
            {
                ShowDebugMessage("Invalid input! Please enter a valid number.");
                maxOffsetInputField.text = maxOffset.ToString(); // คืนค่า InputField กลับเป็นค่าเดิมเมื่อใส่ค่าที่ไม่ถูกต้อง
            }
        }
    }


    private void ShowDebugMessage(string message)
    {
        if (debugText != null)
        {
            if (debugCoroutine != null)
            {
                StopCoroutine(debugCoroutine);
            }

            debugCoroutine = StartCoroutine(DisplayDebugMessage(message));
        }
        else
        {
            Debug.Log(message); // แสดงใน Console ถ้าไม่มี UI Debug
        }
    }

    private IEnumerator DisplayDebugMessage(string message)
    {
        debugText.text = message;
        debugText.color = new Color(debugText.color.r, debugText.color.g, debugText.color.b, 1);

        yield return new WaitForSeconds(1f); // แสดงข้อความ 1 วินาที

        for (float alpha = 1; alpha > 0; alpha -= Time.deltaTime)
        {
            debugText.color = new Color(debugText.color.r, debugText.color.g, debugText.color.b, alpha);
            yield return null;
        }

        debugText.text = ""; // ลบข้อความ
        debugCoroutine = null;
    }
}
