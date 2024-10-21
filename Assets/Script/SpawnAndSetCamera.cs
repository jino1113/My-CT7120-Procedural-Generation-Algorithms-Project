using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class SpawnAndSetCamera : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera virtualCamera;

    // �ѧ��ѹ����Ѻ�Ѻ��Ҽ����蹷��١ Instantiate
    public void SetPlayer(GameObject player)
    {
        // ��駤�������ͧ���������
        virtualCamera.Follow = player.transform;
        virtualCamera.LookAt = player.transform;
    }
}

