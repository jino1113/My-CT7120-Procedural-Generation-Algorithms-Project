using UnityEngine;
using UnityEngine.AI;

public class EnemyBall : MonoBehaviour
{
    // Reference to the player / ตัวผู้เล่น
    public Transform player;

    // Reference to the UI that displays when the player loses / UI ที่จะแสดงเมื่อผู้เล่นแพ้
    private GameObject lossUI;

    // NavMeshAgent component for controlling movement / NavMeshAgent ของลูกบอล
    private NavMeshAgent agent;

    // Timer for controlling walking and stopping behavior / ตัวจับเวลา
    private float timer = 0f;

    // Duration to stop walking / ระยะเวลาที่หยุด
    public float stopDuration = 2f;

    // Duration to wander before stopping / ระยะเวลาที่เดิน
    public float wanderDuration = 5f;

    // Status for walking / สถานะการเดิน
    private bool isWalking = true;

    // Radius for wandering behavior / รัศมีการเดินแบบสุ่ม
    public float wanderRadius = 10f;

    // Detection radius for the player / ระยะที่ศัตรูตรวจจับผู้เล่น
    public float detectionRadius = 10f;

    // Chase radius for the player / ระยะที่ศัตรูยังคงตามล่าผู้เล่น
    public float chaseRadius = 20f;

    // Status for chasing / สถานะการตามล่า
    private bool isChasing = false;

    // Status to check if the game is over / สถานะเกมจบ
    private bool isGameOver = false;



    [SerializeField]private MusicFader musicFader; // อ้างอิง MusicFader
    private bool isPlayerInRange = false;

    void Start()
    {
        // Get the NavMeshAgent component / รับ NavMeshAgent ที่แนบอยู่กับลูกบอล
        agent = GetComponent<NavMeshAgent>();

        if (agent == null)
        {
            Debug.LogError("NavMeshAgent is missing from EnemyBall!"); // Log error if NavMeshAgent is missing / แจ้งเตือนถ้าไม่มี NavMeshAgent
        }

        // Find the lossUI GameObject at the start of the game / ค้นหา lossUI ในตอนเริ่มเกม
        lossUI = GameObject.Find("lossUI");
        if (lossUI != null)
        {
            lossUI.SetActive(false); // Hide the loss UI at the start / ซ่อน UI ตอนเริ่มเกม
        }
        else
        {
            Debug.LogError("Loss UI not found in the scene. Make sure it's named correctly!"); // Log error if lossUI is not found / แจ้งเตือนถ้า lossUI ไม่ถูกพบ
        }

        // Find the player if it has not been assigned in the Inspector / ค้นหา Player หากไม่ได้ตั้งไว้ใน Inspector
        if (player == null)
        {
            GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
            if (playerObject != null)
            {
                player = playerObject.transform; // Assign the player's transform / กำหนดตำแหน่งของผู้เล่น
            }
            else
            {
                Debug.LogError("Player object not found! Please assign it in the inspector or ensure it has the 'Player' tag."); // Log error if the player is not found / แจ้งเตือนถ้าผู้เล่นไม่ถูกพบ
            }
        }

        if (musicFader == null)
        {
            musicFader = FindObjectOfType<MusicFader>();
            if (musicFader == null)
            {
                Debug.LogError("MusicFader script not found in the scene!");
            }
        }
    }

    void Update()
    {
        // If the game is over, stop any further actions / ถ้าเกมจบแล้วไม่ต้องทำอะไรต่อ
        if (isGameOver) return;

        if (agent != null && agent.isOnNavMesh)
        {
            // Check the distance between the enemy and the player / ตรวจจับระยะระหว่างผู้เล่นและศัตรู
            float distanceToPlayer = Vector3.Distance(transform.position, player.position);

            if (distanceToPlayer <= detectionRadius && !isPlayerInRange)
            {
                isPlayerInRange = true;
                musicFader.SwitchToEnemyMusic(); // เปลี่ยนไปเล่นเพลงศัตรู
            }
            else if (distanceToPlayer > detectionRadius && isPlayerInRange)
            {
                isPlayerInRange = false;
                musicFader.SwitchToBackgroundMusic(); // เปลี่ยนกลับไปเพลงพื้นหลัง
            }

            if (isChasing)
            {
                if (distanceToPlayer > chaseRadius)
                {
                    // Stop chasing and return to wandering / หยุดการตามล่าและกลับสู่โหมดเดินสุ่ม
                    isChasing = false;
                    StartWandering();
                }
                else
                {
                    // Keep chasing the player / ยังคงตามล่าผู้เล่น
                    agent.SetDestination(player.position);
                }
            }
            else
            {
                if (distanceToPlayer <= detectionRadius)
                {
                    // Start chasing the player / เริ่มการตามล่าผู้เล่น
                    isChasing = true;
                    agent.isStopped = false;
                }
                else
                {
                    // Wandering behavior / พฤติกรรมเดินสุ่ม
                    timer += Time.deltaTime;

                    if (isWalking && timer >= wanderDuration)
                    {
                        // Change status to stop walking / เปลี่ยนสถานะเป็นหยุดเดิน
                        isWalking = false;
                        timer = 0f;
                        agent.isStopped = true; // Stop the agent / หยุดการเคลื่อนที่
                    }
                    else if (!isWalking && timer >= stopDuration)
                    {
                        // Change status to walking again / เปลี่ยนสถานะเป็นเดินอีกครั้ง
                        isWalking = true;
                        timer = 0f;
                        agent.isStopped = false; // Resume movement / เริ่มการเคลื่อนที่

                        // Set a random destination within the wander radius / ตั้งค่าจุดหมายปลายทางแบบสุ่ม
                        Vector3 randomDestination = GetRandomPoint(transform.position, wanderRadius);
                        agent.SetDestination(randomDestination);
                    }
                }
            }
        }
        else
        {
            if (agent != null && !agent.isOnNavMesh)
            {
                Debug.LogWarning("EnemyBall is not on the NavMesh!"); // Log warning if the enemy is not on the NavMesh / แจ้งเตือนถ้าศัตรูไม่ได้อยู่บน NavMesh
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Check collision with the player / ตรวจสอบการชนกับผู้เล่น
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("Player collided with EnemyBall!");
            GameOver();
        }
    }

    private void GameOver()
    {
        // Show the loss UI / แสดง UI แพ้
        if (lossUI != null)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            lossUI.SetActive(true); // แสดง UI
        }

        // Pause the game by stopping time / หยุดเวลาในเกม
        Time.timeScale = 0f;

        // Set the game-over status / ตั้งสถานะเกมจบ
        isGameOver = true;

        Debug.Log("Game Over! Player has been caught."); // Log the game-over message / แจ้งข้อความว่าเกมจบ
    }

    // Function to generate a random destination point / ฟังก์ชันสำหรับสร้างจุดหมายปลายทางแบบสุ่ม
    private Vector3 GetRandomPoint(Vector3 center, float radius)
    {
        Vector3 randomPos = center + new Vector3(Random.Range(-radius, radius), 0, Random.Range(-radius, radius));

        NavMeshHit hit;
        if (NavMesh.SamplePosition(randomPos, out hit, radius, NavMesh.AllAreas))
        {
            return hit.position; // Return the valid position on the NavMesh / คืนตำแหน่งที่ถูกต้องบน NavMesh
        }

        return center; // If no valid position is found, return the center point / ถ้าไม่พบตำแหน่งที่เหมาะสม ให้คืนจุดเริ่มต้น
    }

    private void StartWandering()
    {
        // Start wandering behavior / เริ่มพฤติกรรมเดินสุ่ม
        isWalking = true;
        timer = 0f;
        agent.isStopped = false;
        Vector3 randomDestination = GetRandomPoint(transform.position, wanderRadius);
        agent.SetDestination(randomDestination);
    }

    // Draw detection and chase radius in the scene view / วาดระยะตรวจจับและตามล่าใน Scene View
    private void OnDrawGizmosSelected()
    {
        // Draw detection radius / วาดระยะตรวจจับ
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);

        // Draw chase radius / วาดระยะตามล่า
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, chaseRadius);
    }
}
