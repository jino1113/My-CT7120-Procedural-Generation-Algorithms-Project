using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{
    public GameObject playerPrefab; // Prefab ของผู้เล่น
    public WallGenerator WallGenerator; // อ้างอิง
    public float tileSpacing = 1.0f;

    private GameObject player; // ตัวแปรสำหรับเก็บผู้เล่น

    void Start()
    {
        CreatePlayer(); // สร้างผู้เล่น
    }


     void CreatePlayer()
    {
        int playerX, playerY;

        // สุ่มตำแหน่งผู้เล่นในกริดที่ว่าง
        do
        {
            playerX = Random.Range(0, WallGenerator.X);
            playerY = Random.Range(0, WallGenerator.Z);
        } while (WallGenerator.grid[playerX, playerY]); // ตรวจสอบว่าตำแหน่งนี้มีวอลล์อยู่หรือไม่

        player = Instantiate(playerPrefab, new Vector3(playerX * tileSpacing, 0, playerY * tileSpacing), Quaternion.identity);
    }
}
