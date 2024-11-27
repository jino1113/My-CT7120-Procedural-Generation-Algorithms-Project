using UnityEngine;

public class RotateCube : MonoBehaviour
{
    public float rotationSpeed = 1f; // ความเร็วในการหมุน / Speed of rotation
    private Quaternion targetRotation; // เป้าหมายการหมุน / Target rotation

    void Start()
    {
        SetNewTargetRotation(); // ตั้งเป้าหมายการหมุนเริ่มต้น / Set the initial target rotation
    }

    void Update()
    {
        // หมุนลูกบาศก์ไปยังเป้าหมายแบบ Slerp
        // Rotate the cube towards the target rotation using Slerp
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);

        // ถ้าหมุนใกล้เป้าหมายแล้ว ให้สุ่มเป้าหมายใหม่
        // If the cube is near the target rotation, set a new random target
        if (Quaternion.Angle(transform.rotation, targetRotation) < 0.1f)
        {
            SetNewTargetRotation(); // ตั้งเป้าหมายการหมุนใหม่แบบสุ่ม / Set a new random target rotation
        }
    }

    private void SetNewTargetRotation()
    {
        // ใช้ Random.Range เพื่อสุ่มมุมในแกน X, Y, Z
        // Use Random.Range to generate random angles for X, Y, and Z axes
        float randomX = Random.Range(-360f, 360f); // สุ่มแกน X / Randomize X axis
        float randomY = Random.Range(-360f, 360f); // สุ่มแกน Y / Randomize Y axis
        float randomZ = Random.Range(-360f, 360f); // สุ่มแกน Z / Randomize Z axis

        // กำหนดเป้าหมายการหมุน
        // Set the target rotation
        targetRotation = Quaternion.Euler(randomX, randomY, randomZ);
    }
}
