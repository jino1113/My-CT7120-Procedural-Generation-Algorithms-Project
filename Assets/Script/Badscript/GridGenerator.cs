using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridGenerator : MonoBehaviour
{
    public int width = 10;  // ความกว้างของ Grid
    public int height = 10; // ความสูงของ Grid
    public List<GameObject> tilePrefabs; // List สำหรับเก็บ prefab หลายตัว
    public GameObject playerPrefab; // Prefab สำหรับผู้เล่น
    public GameObject exitPrefab; // Prefab สำหรับทางออก
    public Vector2Int playerPosition = new Vector2Int(0, 0); // ตำแหน่งผู้เล่น
    public Vector2Int exitPosition; // ตำแหน่งทางออก
    public float tileSpacing = 1.1f; // ระยะห่างระหว่างแต่ละ Tile

    void Start()
    {
        exitPosition = new Vector2Int(width - 1, height - 1); // กำหนดตำแหน่งทางออก
        GenerateGrid();
        PlacePlayerAndExit();
    }

    void GenerateGrid()
    {
        // ลูปตามจำนวนกว้างและสูงของ Grid
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                // ตรวจสอบว่าตำแหน่งนี้เป็นของผู้เล่นหรือทางออกหรือไม่
                if ((x == playerPosition.x && y == playerPosition.y) || (x == exitPosition.x && y == exitPosition.y))
                {
                    continue; // ข้ามการสร้าง Tile ในตำแหน่งของผู้เล่นหรือทางออก
                }

                // เลือก prefab แบบสุ่มจาก List
                GameObject randomPrefab = tilePrefabs[Random.Range(0, tilePrefabs.Count)];

                // คำนวณตำแหน่งที่จะวาง prefab
                Vector3 position = new Vector3(x * tileSpacing, 0, y * tileSpacing);

                // สร้าง Tile จาก prefab แบบสุ่มในตำแหน่งที่คำนวณได้
                Instantiate(randomPrefab, position, Quaternion.identity);
            }
        }
    }

    void PlacePlayerAndExit()
    {
        // สร้างผู้เล่นที่ตำแหน่งเริ่มต้น
        Instantiate(playerPrefab, new Vector3(playerPosition.x * tileSpacing, 0, playerPosition.y * tileSpacing), Quaternion.identity);

        // สร้างทางออกที่ตำแหน่งปลาย
        Instantiate(exitPrefab, new Vector3(exitPosition.x * tileSpacing, 0, exitPosition.y * tileSpacing), Quaternion.identity);
    }
}
