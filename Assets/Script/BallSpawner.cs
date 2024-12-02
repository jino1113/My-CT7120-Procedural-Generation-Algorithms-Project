using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallSpawner : MonoBehaviour
{
    [Header("Ball Settings")]
    public GameObject ballPrefab; // Prefab ของลูกบอล
    public Transform spawnPoint; // จุดที่ลูกบอลจะเกิด (อ้างอิงจาก GameObject)
    public float destroyDelay = 4f; // เวลาที่ลูกบอลจะทำลายตัวเอง
    public int ballCount = 10; // จำนวนลูกบอลที่เกิด

    [Header("Spawn Settings")]
    public float spawnRadius = 1f; // รัศมีการเกิดรอบจุด GameObject
    public float spawnInterval = 1f; // ช่วงเวลาที่ลูกบอลเกิดใหม่

    private bool isSpawning = true; // ควบคุมการเกิดลูกบอล

    void Start()
    {
        // เริ่มการ Spawn ลูกบอล
        StartCoroutine(SpawnBalls());
    }

    private IEnumerator SpawnBalls()
    {
        while (isSpawning)
        {
            for (int i = 0; i < ballCount; i++)
            {
                SpawnBall();
            }
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    private void SpawnBall()
    {
        if (ballPrefab != null && spawnPoint != null)
        {
            // สุ่มตำแหน่งภายในรัศมีรอบ GameObject (spawnPoint)
            Vector3 randomOffset = Random.insideUnitSphere * spawnRadius;
            randomOffset.y = Mathf.Abs(randomOffset.y); // ป้องกันลูกบอลเกิดต่ำกว่าจุดอ้างอิง

            Vector3 spawnPosition = spawnPoint.position + randomOffset;

            // สร้างลูกบอลที่ตำแหน่งสุ่มรอบ GameObject
            GameObject ball = Instantiate(ballPrefab, spawnPosition, Quaternion.identity);

            // ทำลายลูกบอลหลังจากเวลาที่กำหนด
            Destroy(ball, destroyDelay);
        }
        else
        {
            Debug.LogWarning("BallPrefab or SpawnPoint is not assigned!");
        }
    }

    public void StopSpawning()
    {
        isSpawning = false;
    }
}
