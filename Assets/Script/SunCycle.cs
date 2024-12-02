using UnityEngine;

public class SunCycle : MonoBehaviour
{
    public Transform centerPoint; // จุดศูนย์กลางของวงโคจร (โลก)
    public float orbitRadius = 50f; // รัศมีวงโคจร
    public float orbitSpeed = 10f; // ความเร็วในการโคจร
    public float startAngle = 0f; // มุมเริ่มต้นของดวงอาทิตย์ (องศา)

    private float currentAngle;

    void Start()
    {
        // ตั้งค่ามุมเริ่มต้น
        currentAngle = startAngle;
    }

    void Update()
    {
        // เพิ่มมุมตามเวลาที่ผ่านไป
        currentAngle += orbitSpeed * Time.deltaTime;

        // แปลงมุมจากองศาเป็นเรเดียน
        float angleInRadians = currentAngle * Mathf.Deg2Rad;

        // คำนวณตำแหน่งใหม่ตามวงโคจร
        float x = centerPoint.position.x + orbitRadius * Mathf.Cos(angleInRadians);
        float z = centerPoint.position.z + orbitRadius * Mathf.Sin(angleInRadians);
        float y = Mathf.Sin(angleInRadians) * orbitRadius * 0.5f; // ให้ความสูงเปลี่ยนตามตำแหน่ง

        // อัปเดตตำแหน่งของ Directional Light
        transform.position = new Vector3(x, y, z);

        // หมุน Directional Light ให้ชี้ไปที่จุดศูนย์กลาง (โลก)
        transform.LookAt(centerPoint);
    }
}
