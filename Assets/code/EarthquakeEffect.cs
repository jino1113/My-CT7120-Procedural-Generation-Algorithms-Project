using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EarthquakeEffect : MonoBehaviour
{
    public float earthquakeIntensity = 0.1f; 
    public float minFrequency = 5.0f; 
    public float maxFrequency = 10.0f; 
    public float shakeDuration = 1.0f; 

    private Vector3 originalPosition;

    void Start()
    {
        originalPosition = transform.position;
        StartCoroutine(StartRandomEarthquake()); // Start Random
    }

    IEnumerator StartRandomEarthquake()
    {
        while (true) // Loop
        {

            float timeUntilNextShake = Random.Range(minFrequency, maxFrequency);

            // Wait until the random time arrives.
            yield return new WaitForSeconds(timeUntilNextShake);

            // เริ่มการสั่น
            StartCoroutine(ShakeGround());
        }
    }

    IEnumerator ShakeGround()
    {
        float elapsedTime = 0.0f;

        // ทำการสั่นจนกว่าจะครบ duration ที่กำหนด
        while (elapsedTime < shakeDuration)
        {
            // สุ่มการสั่นในแกน X และ Z
            float xShake = Random.Range(-1f, 1f) * earthquakeIntensity;
            float zShake = Random.Range(-1f, 1f) * earthquakeIntensity;

            // เปลี่ยนตำแหน่งวัตถุ (พื้นผิว)
            transform.position = new Vector3(originalPosition.x + xShake, originalPosition.y, originalPosition.z + zShake);

            // เพิ่มค่าเวลาที่ผ่านไป
            elapsedTime += Time.deltaTime;

            // รอจนถึงเฟรมถัดไป
            yield return null;
        }

        // หลังจากจบการสั่นแล้วให้กลับไปยังตำแหน่งเริ่มต้น
        transform.position = originalPosition;
    }
}
