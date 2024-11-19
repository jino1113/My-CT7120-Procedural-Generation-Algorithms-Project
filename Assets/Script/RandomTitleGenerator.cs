using UnityEngine;
using TMPro; // ใช้ TextMeshPro
using System.Collections.Generic;

public class RandomTitleGenerator : MonoBehaviour
{
    public TMP_Text titleText; // อ้างอิงถึง TextMeshPro สำหรับแสดงชื่อ
    private List<string> names = new List<string>(); // รายการชื่อ
    private List<float> probabilities = new List<float>(); // รายการเปอร์เซ็นต์ของแต่ละชื่อ

    void Start()
    {
        // กำหนดชื่อและโอกาส
        names.Add("The scary random maze"); probabilities.Add(90f);
        names.Add("Spooky_randomness"); probabilities.Add(10f);
        names.Add("Home"); probabilities.Add(3f);
        names.Add("Right behind you"); probabilities.Add(2f);

        // สุ่มชื่อและตั้งค่า
        string randomName = GetRandomName();

        // กำหนดฟอนต์และข้อความ
        titleText.text = randomName; // แสดงชื่อใน TextMeshPro
        AdjustFontSize(randomName); // ปรับขนาดฟอนต์ตามข้อความ
    }

    private string GetRandomName()
    {
        // คำนวณโอกาสรวม
        float totalProbability = 0f;
        foreach (float prob in probabilities)
        {
            totalProbability += prob;
        }

        // สุ่มตัวเลขในช่วงของโอกาสรวม
        float randomValue = Random.Range(0, totalProbability);
        float cumulativeProbability = 0f;

        // หา "ชื่อ" ตามเปอร์เซ็นต์
        for (int i = 0; i < names.Count; i++)
        {
            cumulativeProbability += probabilities[i];
            if (randomValue <= cumulativeProbability)
            {
                return names[i];
            }
        }

        return names[0]; // ค่าเริ่มต้น (กรณีผิดพลาด)
    }

    private void AdjustFontSize(string name)
    {
        // ปรับขนาดฟอนต์ตามข้อความ
        switch (name)
        {
            case "The scary random maze":
                titleText.fontSize = 18.85f; // ฟอนต์ขนาด 36
                break;
            case "Spooky_randomness":
                titleText.fontSize = 20.9f; // ฟอนต์ขนาด 30
                break;
            case "Home":
                titleText.fontSize = 44.73f; // ฟอนต์ขนาด 24
                break;
            case "Right behind you":
                titleText.fontSize = 23.24f; // ฟอนต์ขนาด 20
                break;
            default:
                titleText.fontSize = 32; // ฟอนต์ขนาดเริ่มต้น
                break;
        }
    }
}
