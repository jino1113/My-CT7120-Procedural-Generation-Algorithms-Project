using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingWall : MonoBehaviour
{
    public Transform enemy; // ตัวศัตรู / Reference to the enemy object
    private float maxOffset = 5.0f; // ความสูงสูงสุดของกำแพงเมื่อศัตรูอยู่ใกล้ (ปรับรุนแรงขึ้น) / Maximum wall height when the enemy is nearby
    private float minOffset = 0.5f; // ความสูงต่ำสุดของกำแพง / Minimum wall height
    private float detectionRadius = 10f; // ระยะการตรวจจับศัตรู / Radius to detect the enemy
    private float moveSpeed = 5.0f; // ความเร็วในการขยับกำแพง / Speed at which the wall changes height

    private Vector3 originalScale; // ขนาดดั้งเดิมของกำแพง / The original size of the wall

    void Start()
    {
        // บันทึกขนาดดั้งเดิมของกำแพง / Save the original size of the wall
        originalScale = transform.localScale;

        // ตรวจสอบว่าศัตรูมีการตั้งค่าไว้ใน Inspector หรือไม่ / Check if the enemy is assigned in the Inspector
        if (enemy == null)
        {
            // หากไม่พบศัตรูใน Inspector ให้ค้นหาตาม Tag "Enemy" / If not assigned, find the enemy by tag "Enemy"
            GameObject enemyObject = GameObject.FindGameObjectWithTag("Enemy");
            if (enemyObject != null)
            {
                enemy = enemyObject.transform; // กำหนดตัวศัตรู / Assign the found enemy
            }
            else
            {
                Debug.LogError("Enemy object not found! Please assign it in the inspector."); // แสดงข้อความข้อผิดพลาดในกรณีที่ไม่พบ / Show error if no enemy is found
            }
        }
    }

    void Update()
    {
        if (enemy == null) return; // หากไม่มีศัตรูให้ออกจากฟังก์ชัน / Exit the function if no enemy is assigned

        // คำนวณระยะห่างระหว่างศัตรูกับกำแพง / Calculate the distance between the enemy and the wall
        float distanceToEnemy = Vector3.Distance(enemy.position, transform.position);

        // ถ้าศัตรูอยู่ในระยะที่กำหนด / If the enemy is within the detection radius
        if (distanceToEnemy <= detectionRadius)
        {
            Debug.Log($"Enemy is close! Wall at {transform.position} is moving violently."); // แสดงข้อความใน Console เมื่อศัตรูเข้าใกล้ / Log a message when the enemy is near

            // ปรับความสูงของกำแพงแบบรุนแรง / Adjust the wall height violently
            float noise = Mathf.PerlinNoise(transform.position.x + Time.time * moveSpeed, transform.position.z + Time.time * moveSpeed);
            float targetHeight = Mathf.Lerp(minOffset, maxOffset, noise); // ใช้ Perlin Noise ในการปรับความสูง / Use Perlin Noise for dynamic height adjustment
            transform.localScale = new Vector3(originalScale.x, targetHeight, originalScale.z); // ปรับขนาดใหม่ของกำแพง / Apply the new wall size
        }
        else
        {
            // กลับสู่ขนาดเดิม / Gradually restore the wall to its original size
            transform.localScale = Vector3.Lerp(transform.localScale, originalScale, Time.deltaTime * moveSpeed);

            Debug.Log($"Wall at {transform.position} is restoring to original size."); // แสดงข้อความใน Console เมื่อกำแพงกลับสู่ขนาดเดิม / Log a message when the wall restores its size
        }
    }
}
