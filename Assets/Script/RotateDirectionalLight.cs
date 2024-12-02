using UnityEngine;

public class RotateDirectionalLight : MonoBehaviour
{
    public float rotationSpeed = 1f; // ความเร็วในการหมุน / Speed of rotation
    private Quaternion targetRotation; // เป้าหมายการหมุน / Target rotation

    void Start()
    {
        SetNewTargetRotation(); // ตั้งเป้าหมายการหมุนเริ่มต้น / Set the initial target rotation
    }

    void Update()
    {
        // หมุน Directional Light ไปยังเป้าหมายแบบ Slerp
        // Rotate the Directional Light towards the target rotation using Slerp
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);

        // ถ้าหมุนใกล้เป้าหมายแล้ว ให้สุ่มเป้าหมายใหม่
        // If the light is near the target rotation, set a new random target
        if (Quaternion.Angle(transform.rotation, targetRotation) < 0.1f)
        {
            SetNewTargetRotation(); // ตั้งเป้าหมายการหมุนใหม่แบบสุ่ม / Set a new random target rotation
        }
    }

    private void SetNewTargetRotation()
    {
        // ใช้ Random.Range เพื่อสุ่มมุมในแกน X, Y
        // Use Random.Range to generate random angles for X and Y axes
        float randomX = Random.Range(-90f, 90f); // สุ่มมุมแกน X / Randomize X axis (for sunrise/sunset effect)
        float randomY = Random.Range(0f, 360f); // สุ่มมุมแกน Y / Randomize Y axis (horizontal rotation)

        // กำหนดเป้าหมายการหมุน
        // Set the target rotation
        targetRotation = Quaternion.Euler(randomX, randomY, 0f);
    }
}
