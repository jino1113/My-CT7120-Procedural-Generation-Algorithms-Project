using Cinemachine;
using UnityEngine;

public class FirstPersonCamera : MonoBehaviour
{
    public Transform playerBody;
    public float mouseSensitivity = 100f;
    private float xRotation = 0f;
    private CinemachineVirtualCamera virtualCamera;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;

        // หา Virtual Camera ที่เชื่อมกับตัวละคร
        virtualCamera = GetComponent<CinemachineVirtualCamera>();
        if (virtualCamera != null)
        {
            virtualCamera.Follow = playerBody; // เชื่อมกล้องให้ตามตัวละคร
            virtualCamera.LookAt = playerBody;
        }
    }

    void Update()
    {
        // การควบคุมกล้องด้วยเมาส์
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        playerBody.Rotate(Vector3.up * mouseX);
    }
}
