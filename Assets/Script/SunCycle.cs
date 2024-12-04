using UnityEngine;

public class SunCycle : MonoBehaviour
{
    public Transform centerPoint; // จุดศูนย์กลางของโลก
    public float rotationSpeed = 10f; // ความเร็วในการหมุน

    void Update()
    {
        // หมุนวัตถุรอบจุดศูนย์กลางในแกน Y
        transform.RotateAround(centerPoint.position, Vector3.right, rotationSpeed * Time.deltaTime);

        // หมุนวัตถุให้ชี้ไปที่จุดศูนย์กลาง (โลก)
        transform.LookAt(centerPoint);
    }
}
