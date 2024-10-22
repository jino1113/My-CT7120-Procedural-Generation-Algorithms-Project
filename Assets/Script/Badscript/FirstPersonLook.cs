using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstPersonLook : MonoBehaviour
{
    public float mouseSensitivity = 100f;
    public Transform playerBody;

    private float xRotation = 0f;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;  // ล็อคเคอร์เซอร์ให้อยู่กลางจอ
    }

    void Update()
    {
        // รับค่า Input จากเมาส์
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        // หมุนตัวละครในแนวแกน Y
        playerBody.Rotate(Vector3.up * mouseX);

        // หมุนกล้องในแนวแกน X (แนวตั้ง)
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);  // จำกัดมุมมองแนวตั้งไม่ให้หมุนเกิน 90 องศา
        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
    }
}
