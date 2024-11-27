using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class SpawnAndSetCamera : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera virtualCamera;

    // ฟังก์ชันสำหรับรับค่าผู้เล่นที่ถูก Instantiate
    public void SetPlayer(GameObject player)
    {
        // ตั้งค่าให้กล้องตามผู้เล่น
        virtualCamera.Follow = player.transform;
        virtualCamera.LookAt = player.transform;
    }
}

