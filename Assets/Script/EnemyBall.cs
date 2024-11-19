using UnityEngine;
using UnityEngine.AI;

public class EnemyBall : MonoBehaviour
{
    public Transform player; // ตัวผู้เล่น (ไม่จำเป็นต้องใช้ในโหมดเดินแบบสุ่ม)

    private NavMeshAgent agent; // NavMeshAgent ของลูกบอล
    private float timer = 0f; // ตัวจับเวลา
    public float stopDuration = 2f; // ระยะเวลาที่หยุด
    public float wanderDuration = 5f; // ระยะเวลาที่เดิน
    private bool isWalking = true; // สถานะการเดิน

    public float wanderRadius = 10f; // รัศมีการเดินแบบสุ่ม

    void Start()
    {
        // รับ NavMeshAgent ที่แนบอยู่กับลูกบอล
        agent = GetComponent<NavMeshAgent>();

        if (agent == null)
        {
            Debug.LogError("NavMeshAgent is missing from EnemyBall!");
        }
    }

    void Update()
    {
        if (agent != null && agent.isOnNavMesh)
        {
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
