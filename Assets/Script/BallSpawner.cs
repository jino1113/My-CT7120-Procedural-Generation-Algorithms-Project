using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallSpawner : MonoBehaviour
{
    [Header("Ball Settings")]
    public GameObject ballPrefab; // Prefab ของลูกบอล / Prefab of the ball
    public Transform spawnPoint; // จุดที่ลูกบอลจะเกิด (อ้างอิงจาก GameObject) / Spawn point of the ball (referenced from GameObject)
    public float destroyDelay = 4f; // เวลาที่ลูกบอลจะทำลายตัวเอง / Time before the ball destroys itself
    public int ballCount = 10; // จำนวนลูกบอลที่เกิด / Number of balls to spawn

    [Header("Spawn Settings")]
    public float spawnRadius = 1f; // รัศมีการเกิดรอบจุด GameObject / Spawn radius around the GameObject
    public float spawnInterval = 1f; // ช่วงเวลาที่ลูกบอลเกิดใหม่ / Time interval between ball spawns

    private bool isSpawning = true; // ควบคุมการเกิดลูกบอล / Control whether spawning is active
    private GameObject[] ballPooling = new GameObject[34]; // ระบบ Object Pooling สำหรับลูกบอล / Object Pooling for balls

    void Start()
    {
        // เตรียม Object Pooling สำหรับลูกบอล / Prepare object pooling for the balls
        for (int i = 0; i < ballPooling.Length; i++)
        {
            ballPooling[i] = Instantiate(ballPrefab);
            ballPooling[i].SetActive(false); // ปิดการใช้งานลูกบอลทั้งหมดเริ่มต้น / Deactivate all balls initially
        }

        // เริ่มการ Spawn ลูกบอล / Start spawning balls
        StartCoroutine(SpawnBallsTwo());
    }

    private IEnumerator SpawnBallsTwo()
    {
        while (isSpawning)
        {
            // วนลูปหาลูกบอลใน Object Pool ที่ไม่ได้ใช้งาน / Iterate through the object pool for inactive balls
            for (int i = 0; i < ballPooling.Length; i++)
            {
                if (!ballPooling[i].activeInHierarchy)
                {
                    // เปิดใช้งานลูกบอลที่ไม่ได้ใช้งาน / Activate the inactive ball
                    ballPooling[i].SetActive(true);

                    // สุ่มตำแหน่งภายในรัศมีรอบ GameObject (spawnPoint) / Generate a random position within the spawn radius
                    Vector3 randomOffset = Random.insideUnitSphere * spawnRadius;
                    randomOffset.y = Mathf.Abs(randomOffset.y); // ป้องกันลูกบอลเกิดต่ำกว่าจุดอ้างอิง / Ensure the ball spawns above the reference point
                    Vector3 spawnPosition = spawnPoint.position + randomOffset;

                    // ตั้งค่าตำแหน่งให้กับลูกบอล / Set the ball's position
                    ballPooling[i].transform.position = spawnPosition;

                    // กำหนดความเร็วให้ลูกบอล / Assign velocity to the ball
                    ballPooling[i].GetComponent<Rigidbody>().velocity = new Vector3(0, -100, 0);

                    break; // หยุดการค้นหาหลังจาก Spawn ลูกบอลแล้ว / Stop searching after spawning the ball
                }
            }
            yield return new WaitForSeconds(spawnInterval); // รอเวลาตาม spawnInterval ก่อนเริ่มรอบถัดไป / Wait for the spawn interval before continuing
        }
    }
}
