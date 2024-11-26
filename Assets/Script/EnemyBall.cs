using UnityEngine;
using UnityEngine.AI;

public class EnemyBall : MonoBehaviour
{
    public Transform player; // ตัวผู้เล่น
    private GameObject lossUI; // UI ที่จะแสดงเมื่อผู้เล่นแพ้
    private NavMeshAgent agent; // NavMeshAgent ของลูกบอล

    private float timer = 0f; // ตัวจับเวลา
    public float stopDuration = 2f; // ระยะเวลาที่หยุด
    public float wanderDuration = 5f; // ระยะเวลาที่เดิน
    private bool isWalking = true; // สถานะการเดิน

    public float wanderRadius = 10f; // รัศมีการเดินแบบสุ่ม
    public float detectionRadius = 2f; // ระยะที่ศัตรูตรวจจับผู้เล่น
    private bool isGameOver = false; // สถานะเกมจบ

    void Start()
    {
        // รับ NavMeshAgent ที่แนบอยู่กับลูกบอล
        agent = GetComponent<NavMeshAgent>();

        if (agent == null)
        {
            Debug.LogError("NavMeshAgent is missing from EnemyBall!");
        }

        // ค้นหา lossUI ในตอนเริ่มเกม
        lossUI = GameObject.Find("lossUI");
        if (lossUI != null)
        {
            lossUI.SetActive(false); // ซ่อน UI ตอนเริ่มเกม
        }
        else
        {
            Debug.LogError("Loss UI not found in the scene. Make sure it's named correctly!");
        }

        // ค้นหา Player หากไม่ได้ตั้งไว้ใน Inspector
        if (player == null)
        {
            GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
            if (playerObject != null)
            {
                player = playerObject.transform;
            }
            else
            {
                Debug.LogError("Player object not found! Please assign it in the inspector or ensure it has the 'Player' tag.");
            }
        }
    }

    void Update()
    {
        if (isGameOver) return; // ถ้าเกมจบแล้วไม่ต้องทำอะไรต่อ

        if (agent != null && agent.isOnNavMesh)
        {
            // ตรวจจับระยะระหว่างผู้เล่นและศัตรู
            if (player != null && Vector3.Distance(transform.position, player.position) <= detectionRadius)
            {
                GameOver(); // เรียกฟังก์ชันจบเกม
                return;
            }

            timer += Time.deltaTime;

            if (isWalking && timer >= wanderDuration)
            {
                // เปลี่ยนสถานะเป็นหยุดเดิน
                isWalking = false;
                timer = 0f;
                agent.isStopped = true; // หยุดการเคลื่อนที่
            }
            else if (!isWalking && timer >= stopDuration)
            {
                // เปลี่ยนสถานะเป็นเดินอีกครั้ง
                isWalking = true;
                timer = 0f;
                agent.isStopped = false; // เริ่มการเคลื่อนที่

                // ตั้งค่าจุดหมายปลายทางแบบสุ่ม
                Vector3 randomDestination = GetRandomPoint(transform.position, wanderRadius);
                agent.SetDestination(randomDestination);
            }
        }
        else
        {
            if (agent != null && !agent.isOnNavMesh)
            {
                Debug.LogWarning("EnemyBall is not on the NavMesh!");
            }
        }
    }

    private void GameOver()
    {
        // แสดง UI แพ้
        if (lossUI != null)
        {
            lossUI.SetActive(true);
        }

        // หยุดเวลาในเกม
        Time.timeScale = 0f;

        // ตั้งสถานะเกมจบ
        isGameOver = true;

        Debug.Log("Game Over! Player has been caught.");
    }

    // ฟังก์ชันสำหรับสร้างจุดหมายปลายทางแบบสุ่ม
    private Vector3 GetRandomPoint(Vector3 center, float radius)
    {
        Vector3 randomPos = center + new Vector3(Random.Range(-radius, radius), 0, Random.Range(-radius, radius));

        NavMeshHit hit;
        if (NavMesh.SamplePosition(randomPos, out hit, radius, NavMesh.AllAreas))
        {
            return hit.position;
        }

        return center; // หากไม่พบจุดที่เหมาะสม ให้ใช้จุดเริ่มต้น
    }
}
