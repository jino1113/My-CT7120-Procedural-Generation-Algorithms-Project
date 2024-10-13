using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallGenerator : MonoBehaviour
{
    public int X = 10; // ความกว้างของ Maze
    public int Z = 10; // ความสูงของ Maze
    public float tileSpacing = 1.0f; // ระยะห่างระหว่าง tiles
    public GameObject wallPrefab; // Prefab สำหรับกำแพง
    public float boundarySpacing = 0.01f; // ระยะห่างของกำแพง
    public float wallHeight = 3.0f; // ความสูงของกำแพง
    public float wallThickness = 0.5f; // ความหนาของกำแพง
    public GameObject exitPrefab; // Prefab สำหรับจุดทางออก
    public bool[,] grid; // ตัวแปรสำหรับเก็บสถานะของ Maze

    private GameObject player; // ตัวแปรสำหรับเก็บผู้เล่น
    private int exitX; // ตัวแปรสำหรับตำแหน่ง X ของ exit
    private int exitY; // ตัวแปรสำหรับตำแหน่ง Y ของ exit

    void Start()
    {
        grid = new bool[X, Z]; // กำหนดขนาดของ grid
        CreateBoundaryWalls();
        CreateExit(); // เรียกใช้ฟังก์ชันสร้างจุดทางออก
    }

    void DrawMaze()
    {
        for (int x = 0; x < X; x++)
        {
            for (int y = 0; y < Z; y++)
            {
                if (!grid[x, y])
                {
                    Vector3 tilePosition = new Vector3(x * tileSpacing, 0, y * tileSpacing);
                    // ตรวจสอบว่าตำแหน่งนี้เป็นตำแหน่ง exit หรือไม่
                    if (!(x == exitX && y == exitY)) // ตรวจสอบไม่ให้สร้างทับตำแหน่ง exit
                    {
                        Instantiate(wallPrefab, tilePosition, Quaternion.identity);
                    }
                }
            }
        }
    }

    void CreateExit()
    {
        grid = new bool[X, Z]; // กำหนดค่า grid ก่อน 
                               // ... โค้ดสำหรับสร้าง maze ...

        // ตั้งค่าให้ exitX และ exitY เป็นค่าเริ่มต้น
        exitX = Random.Range(0, X);
        exitY = Random.Range(0, Z);

        // ตรวจสอบตำแหน่งจนกว่าจะได้ตำแหน่งที่ว่าง
        while (grid[exitX, exitY]) // ถ้าตำแหน่งนี้ไม่ว่าง ให้สุ่มใหม่
        {
            exitX = Random.Range(0, X);
            exitY = Random.Range(0, Z);
        }

        Instantiate(exitPrefab, new Vector3(exitX * tileSpacing, 0, exitY * tileSpacing), Quaternion.identity);
    }

    void CreateBoundaryWalls()
    {
        // สร้างกำแพงด้านล่างและด้านบน
        for (int x = -1; x <= X; x++)
        {
            // สร้างกำแพงด้านล่าง
            GameObject wallBottom = Instantiate(wallPrefab, new Vector3(x * tileSpacing, wallHeight / 2, (-1 * tileSpacing) - boundarySpacing), Quaternion.identity);
            wallBottom.transform.localScale = new Vector3(wallThickness, wallHeight, 1); // ปรับความหนาและความสูง

            // สร้างกำแพงด้านบน
            GameObject wallTop = Instantiate(wallPrefab, new Vector3(x * tileSpacing, wallHeight / 2, (Z * tileSpacing) + boundarySpacing), Quaternion.identity);
            wallTop.transform.localScale = new Vector3(wallThickness, wallHeight, 1); // ปรับความหนาและความสูง
        }

        // สร้างกำแพงด้านซ้ายและด้านขวา
        for (int y = 0; y < Z; y++)
        {
            // สร้างกำแพงด้านซ้าย (หมุน 90 องศา)
            GameObject wallLeft = Instantiate(wallPrefab, new Vector3((-1 * tileSpacing) - boundarySpacing, wallHeight / 2, y * tileSpacing), Quaternion.Euler(0, 90, 0));
            wallLeft.transform.localScale = new Vector3(wallThickness, wallHeight, 1); // ปรับความหนาและความสูง

            // สร้างกำแพงด้านขวา (หมุน 90 องศา)
            GameObject wallRight = Instantiate(wallPrefab, new Vector3((X * tileSpacing) + boundarySpacing, wallHeight / 2, y * tileSpacing), Quaternion.Euler(0, 90, 0));
            wallRight.transform.localScale = new Vector3(wallThickness, wallHeight, 1); // ปรับความหนาและความสูง
        }
    }
}
