using UnityEngine;

public class RotateCube : MonoBehaviour
{
    public float rotationSpeed = 1f; // ความเร็วในการหมุน
    private Quaternion targetRotation; // เป้าหมายการหมุน

    void Start()
    {
        SetNewTargetRotation(); // ตั้งเป้าหมายการหมุนเริ่มต้น
    }

    void Update()
    {
        // หมุนลูกบาศก์ไปยังเป้าหมายแบบ Slerp
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);

        // ถ้าหมุนใกล้เป้าหมายแล้ว ให้สุ่มเป้าหมายใหม่
        if (Quaternion.Angle(transform.rotation, targetRotation) < 0.1f)
        {
            SetNewTargetRotation(); // ตั้งเป้าหมายการหมุนใหม่แบบสุ่ม
        }
    }

    private void SetNewTargetRotation()
    {
        // ใช้ Random.Range เพื่อสุ่มมุมในแกน X, Y, Z
        float randomX = Random.Range(-360f, 360f); // สุ่มแกน X
        float randomY = Random.Range(-360f, 360f); // สุ่มแกน Y
        float randomZ = Random.Range(-360f, 360f); // สุ่มแกน Z

        // กำหนดเป้าหมายการหมุน
        targetRotation = Quaternion.Euler(randomX, randomY, randomZ);
    }
}
