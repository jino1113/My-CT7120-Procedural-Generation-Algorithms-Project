using UnityEngine;
using UnityEngine.AI;

public class EnemyBall : MonoBehaviour
{
    public Transform player; // ตัวผู้เล่น
    private NavMeshAgent agent; // NavMeshAgent ของลูกบอล

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
        // ตรวจสอบว่าตัวผู้เล่นถูกกำหนดและลูกบอลอยู่บน NavMesh
        if (player != null && agent != null && agent.isOnNavMesh)
        {
            // ตั้งค่าจุดหมายให้ลูกบอลไปที่ผู้เล่น
            agent.SetDestination(player.position);
        }
        else
        {
            // ถ้าลูกบอลไม่อยู่บน NavMesh จะมีข้อความเตือน
            if (agent != null && !agent.isOnNavMesh)
            {
                Debug.LogWarning("EnemyBall is not on the NavMesh!");
            }
        }
    }
}
