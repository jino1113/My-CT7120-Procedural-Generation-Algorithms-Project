using UnityEngine;

public class SunCycle : MonoBehaviour
{
    public Transform centerPoint; // จุดศูนย์กลางของโลก / The central point of the earth (e.g., the planet's center)
    public float rotationSpeed = 10f; // ความเร็วในการหมุน / Speed of the rotation around the center

    void Update()
    {
        // หมุนวัตถุรอบจุดศูนย์กลางในแกน X / Rotate the object around the central point along the X-axis
        transform.RotateAround(centerPoint.position, Vector3.right, rotationSpeed * Time.deltaTime);

        // หมุนวัตถุให้ชี้ไปที่จุดศูนย์กลาง / Adjust the rotation of the object to look at the center
        transform.LookAt(centerPoint);
    }
}
