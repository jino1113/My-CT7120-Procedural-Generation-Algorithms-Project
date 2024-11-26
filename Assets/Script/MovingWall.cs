using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingWall : MonoBehaviour
{
    public Transform enemy;  // ตัวศัตรู
    private float maxOffset = 5.0f; // ความสูงสูงสุดของกำแพงเมื่อศัตรูอยู่ใกล้ (ปรับรุนแรงขึ้น)
    private float minOffset = 0.5f; // ความสูงต่ำสุดของกำแพง
    private float detectionRadius = 10f; // ระยะการตรวจจับศัตรู
    private float moveSpeed = 5.0f; // ความเร็วในการขยับกำแพง

    private Vector3 originalScale; // ขนาดดั้งเดิมของกำแพง

    void Start()
    {
        // บันทึกขนาดดั้งเดิมของกำแพง
        originalScale = transform.localScale;

        // ตรวจสอบว่าศัตรูมีการตั้งค่าไว้ใน Inspector หรือไม่
        if (enemy == null)
        {
            GameObject enemyObject = GameObject.FindGameObjectWithTag("Enemy");
            if (enemyObject != null)
            {
                enemy = enemyObject.transform;
            }
            else
            {
                Debug.LogError("Enemy object not found! Please assign it in the inspector.");
            }
        }
    }

    void Update()
    {
        if (enemy == null) return;

        // คำนวณระยะห่างระหว่างศัตรูกับกำแพง
        float distanceToEnemy = Vector3.Distance(enemy.position, transform.position);

        // ถ้าศัตรูอยู่ในระยะที่กำหนด
        if (distanceToEnemy <= detectionRadius)
        {
            Debug.Log($"Enemy is close! Wall at {transform.position} is moving violently.");

            // ปรับความสูงของกำแพงแบบรุนแรง
            float noise = Mathf.PerlinNoise(transform.position.x + Time.time * moveSpeed, transform.position.z + Time.time * moveSpeed);
            float targetHeight = Mathf.Lerp(minOffset, maxOffset, noise);
            transform.localScale = new Vector3(originalScale.x, targetHeight, originalScale.z);
        }
        else
        {
            // กลับสู่ขนาดเดิม
            transform.localScale = Vector3.Lerp(transform.localScale, originalScale, Time.deltaTime * moveSpeed);

            Debug.Log($"Wall at {transform.position} is restoring to original size.");
        }
    }
}
