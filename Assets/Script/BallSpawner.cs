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

    private GameObject[] ballPooling = new GameObject[34];

    void Start()
    {


        for(int i = 0; i < ballPooling.Length; i++)
        {
            ballPooling[i] = Instantiate(ballPrefab);
            ballPooling[i].SetActive(false);
        }

        // เริ่มการ Spawn ลูกบอล
        StartCoroutine(SpawnBallsTwo());
    }


    private IEnumerator SpawnBallsTwo()
    {
        while (isSpawning)
        {
            for (int i = 0; i < ballPooling.Length; i++)
            {
                if (ballPooling[i].gameObject.activeInHierarchy == false)
                {
                    ballPooling[i].SetActive(true);

                    // สุ่มตำแหน่งภายในรัศมีรอบ GameObject (spawnPoint)
                    Vector3 randomOffset = Random.insideUnitSphere * spawnRadius;
                    randomOffset.y = Mathf.Abs(randomOffset.y); // ป้องกันลูกบอลเกิดต่ำกว่าจุดอ้างอิง
                    Vector3 spawnPosition = spawnPoint.position + randomOffset;

                    ballPooling[i].transform.position = spawnPosition;

                    ballPooling[i].GetComponent<Rigidbody>().velocity = new Vector3(0, -100, 0);

                    break;
                }
            }
            yield return new WaitForSeconds(spawnInterval);
        }
    }
}
