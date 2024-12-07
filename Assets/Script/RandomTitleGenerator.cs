using UnityEngine;
using TMPro; // ใช้ TextMeshPro / Import TextMeshPro for UI text management
using System.Collections.Generic;

public class RandomTitleGenerator : MonoBehaviour
{
    public TMP_Text titleText; // อ้างอิงถึง TextMeshPro สำหรับแสดงชื่อ / Reference to TextMeshPro component for displaying title
    private List<string> names = new List<string>(); // รายการชื่อ / List of possible title names
    private List<float> probabilities = new List<float>(); // รายการเปอร์เซ็นต์ของแต่ละชื่อ / List of probabilities for each name

    void Start()
    {
        // กำหนดชื่อและโอกาส / Define names and their associated probabilities
        names.Add("The scary random maze"); probabilities.Add(60f); // ชื่อและโอกาสที่จะแสดงผล / Title with 90% chance
        names.Add("No Title"); probabilities.Add(20f); // ชื่อและโอกาสที่จะแสดงผล / Title with 10% chance
        names.Add("SadSad"); probabilities.Add(10f); // ชื่อและโอกาสที่จะแสดงผล / Title with 3% chance
        names.Add("Behind You"); probabilities.Add(10f); // ชื่อและโอกาสที่จะแสดงผล / Title with 2% chance

        // สุ่มชื่อและตั้งค่า / Randomly select a name and set it to the text component
        string randomName = GetRandomName();

        // กำหนดฟอนต์และข้อความ / Set the text and adjust its font size based on the name
        titleText.text = randomName; // แสดงชื่อใน TextMeshPro / Display the randomly chosen name
    }

    private string GetRandomName()
    {
        // คำนวณโอกาสรวม / Calculate total probability sum
        float totalProbability = 0f;
        foreach (float prob in probabilities)
        {
            totalProbability += prob; // รวมโอกาสของแต่ละชื่อ / Sum the probabilities
        }

        // สุ่มตัวเลขในช่วงของโอกาสรวม / Generate a random value within the range of total probability
        float randomValue = Random.Range(0, totalProbability);
        float cumulativeProbability = 0f;

        // หา "ชื่อ" ตามเปอร์เซ็นต์ / Select a name based on cumulative probabilities
        for (int i = 0; i < names.Count; i++)
        {
            cumulativeProbability += probabilities[i];
            if (randomValue <= cumulativeProbability)
            {
                return names[i]; // คืนค่าชื่อที่สุ่มได้ / Return the selected name
            }
        }

        return names[0]; // ค่าเริ่มต้น (กรณีผิดพลาด) / Default value in case of error
    }
}
