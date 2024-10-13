using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridGenerator : MonoBehaviour
{
    public int width = 10;  // ความกว้างของ Grid
    public int height = 10; // ความสูงของ Grid
    public List<GameObject> tilePrefabs; // List สำหรับเก็บ prefab หลายตัว
    public float tileSpacing = 1.1f; // ระยะห่างระหว่างแต่ละ Tile

    void Start()
    {
        GenerateGrid();
    }

    void GenerateGrid()
    {
        // ลูปตามจำนวนกว้างและสูงของ Grid
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                // เลือก prefab แบบสุ่มจาก List
                GameObject randomPrefab = tilePrefabs[Random.Range(0, tilePrefabs.Count)];

                // คำนวณตำแหน่งที่จะวาง prefab
                Vector3 position = new Vector3(x * tileSpacing, 0, y * tileSpacing);

                // สร้าง Tile จาก prefab แบบสุ่มในตำแหน่งที่คำนวณได้
                Instantiate(randomPrefab, position, Quaternion.identity);
            }
        }
    }
}
