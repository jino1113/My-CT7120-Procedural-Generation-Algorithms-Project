using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MazeGenerator : MonoBehaviour
{
    [SerializeField]
    private MazeCell _mazeCellPrefab; // Prefab ของเซลล์ในเขาวงกต / Prefab for a cell in the maze

    [SerializeField]
    private GameObject _playerPrefab; // Prefab ของตัวผู้เล่น / Prefab for the player

    [SerializeField]
    private GameObject _exitPrefab; // Prefab ของทางออก / Prefab for the exit

    [SerializeField]
    private int _mazeWidth; // ความกว้างของเขาวงกต / Width of the maze

    [SerializeField]
    private int _mazeDepth; // ความลึกของเขาวงกต / Depth of the maze

    private MazeCell[,] _mazeGrid; // อาร์เรย์ 2 มิติสำหรับเก็บเซลล์ทั้งหมดในเขาวงกต / 2D array to store all cells in the maze
    private Transform spawnPoint; // ตำแหน่งที่ใช้สำหรับ Spawn ตัวผู้เล่น / Spawn position for the player

    void Start()
    {
        // สร้างอาร์เรย์สำหรับเขาวงกตตามขนาดที่กำหนด / Create an array for the maze with the specified size
        _mazeGrid = new MazeCell[_mazeWidth, _mazeDepth];

        // วนลูปเพื่อสร้างเซลล์แต่ละเซลล์ในตำแหน่งที่ถูกต้อง / Loop to create each cell at the correct position
        for (int x = 0; x < _mazeWidth; x++)
        {
            for (int z = 0; z < _mazeDepth; z++)
            {
                // สร้างเซลล์ใหม่โดยตั้งตำแหน่งตามพิกัด x และ z / Instantiate a new cell at the x and z coordinates
                _mazeGrid[x, z] = Instantiate(_mazeCellPrefab, new Vector3(x, 0, z), Quaternion.identity);
            }
        }

        // เริ่มสร้างเขาวงกตโดยเริ่มที่เซลล์แรก / Start maze generation from the first cell
        GenerateMaze(null, _mazeGrid[0, 0]);

        // ตั้งค่า spawnPoint สำหรับการ spawn ตัวผู้เล่นที่เซลล์แรก / Set spawnPoint at the first cell for player spawn
        spawnPoint = _mazeGrid[0, 0].transform;

        // เรียกฟังก์ชัน SpawnAndSetCamera เพื่อสร้างตัวผู้เล่นและตั้งค่ากล้อง / Call SpawnAndSetCamera to spawn the player and set up the camera
        SpawnAndSetCamera(_playerPrefab, spawnPoint);

        // สร้างทางออกที่ตำแหน่งเซลล์สุดท้ายของเขาวงกต / Place the exit at the last cell in the maze
        Instantiate(_exitPrefab, _mazeGrid[_mazeWidth - 1, _mazeDepth - 1].transform.position, Quaternion.identity);
    }

    private void GenerateMaze(MazeCell previousCell, MazeCell currentCell)
    {
        // กำหนดให้เซลล์ปัจจุบันเป็นเซลล์ที่ถูกเยี่ยมชมแล้ว / Mark the current cell as visited
        currentCell.Visit();

        // ลบกำแพงระหว่างเซลล์ปัจจุบันและเซลล์ก่อนหน้า / Remove walls between current and previous cells
        ClearWalls(previousCell, currentCell);

        MazeCell nextCell;

        // วนลูปเพื่อค้นหาเซลล์ถัดไปที่ยังไม่ถูกเยี่ยมชม / Loop to find the next unvisited cell
        do
        {
            nextCell = GetNextUnvisitedCell(currentCell);

            // ถ้ามีเซลล์ถัดไปที่ยังไม่ถูกเยี่ยมชม ให้เรียกใช้ GenerateMaze เพื่อดำเนินการต่อ / If there is an unvisited cell, continue maze generation
            if (nextCell != null)
            {
                GenerateMaze(currentCell, nextCell);
            }
        } while (nextCell != null);
    }

    private MazeCell GetNextUnvisitedCell(MazeCell currentCell)
    {
        // ดึงรายการเซลล์ที่ยังไม่ถูกเยี่ยมชมที่อยู่ติดกับเซลล์ปัจจุบัน / Get a list of unvisited neighboring cells
        var unvisitedCells = GetUnvisitedCells(currentCell);

        // สุ่มเลือกเซลล์ที่ยังไม่ถูกเยี่ยมชม / Randomly select an unvisited cell
        return unvisitedCells.OrderBy(_ => Random.Range(1, 10)).FirstOrDefault();
    }

    private IEnumerable<MazeCell> GetUnvisitedCells(MazeCell currentCell)
    {
        int x = (int)currentCell.transform.position.x;
        int z = (int)currentCell.transform.position.z;

        // ตรวจสอบเซลล์ด้านขวา / Check the cell on the right
        if (x + 1 < _mazeWidth)
        {
            var cellToRight = _mazeGrid[x + 1, z];
            if (!cellToRight.IsVisited)
            {
                yield return cellToRight;
            }
        }

        // ตรวจสอบเซลล์ด้านซ้าย / Check the cell on the left
        if (x - 1 >= 0)
        {
            var cellToLeft = _mazeGrid[x - 1, z];
            if (!cellToLeft.IsVisited)
            {
                yield return cellToLeft;
            }
        }

        // ตรวจสอบเซลล์ด้านหน้า / Check the cell in front
        if (z + 1 < _mazeDepth)
        {
            var cellToFront = _mazeGrid[x, z + 1];
            if (!cellToFront.IsVisited)
            {
                yield return cellToFront;
            }
        }

        // ตรวจสอบเซลล์ด้านหลัง / Check the cell behind
        if (z - 1 >= 0)
        {
            var cellToBack = _mazeGrid[x, z - 1];
            if (!cellToBack.IsVisited)
            {
                yield return cellToBack;
            }
        }
    }

    private void ClearWalls(MazeCell previousCell, MazeCell currentCell)
    {
        // ถ้าไม่มีเซลล์ก่อนหน้า ให้ออกจากฟังก์ชัน / If there is no previous cell, exit the function
        if (previousCell == null)
        {
            return;
        }

        // ลบกำแพงด้านขวาและซ้ายระหว่างเซลล์ปัจจุบันกับเซลล์ก่อนหน้า / Remove right and left walls between current and previous cells
        if (previousCell.transform.position.x < currentCell.transform.position.x)
        {
            previousCell.ClearRightWall();
            currentCell.ClearLeftWall();
            return;
        }

        if (previousCell.transform.position.x > currentCell.transform.position.x)
        {
            previousCell.ClearLeftWall();
            currentCell.ClearRightWall();
            return;
        }

        // ลบกำแพงด้านหน้าและหลังระหว่างเซลล์ปัจจุบันกับเซลล์ก่อนหน้า / Remove front and back walls between current and previous cells
        if (previousCell.transform.position.z < currentCell.transform.position.z)
        {
            previousCell.ClearFrontWall();
            currentCell.ClearBackWall();
            return;
        }

        if (previousCell.transform.position.z > currentCell.transform.position.z)
        {
            previousCell.ClearBackWall();
            currentCell.ClearFrontWall();
            return;
        }
    }

    // ฟังก์ชันที่เรียกเพื่อ spawn ตัวผู้เล่นและตั้งค่ากล้อง / Function to spawn the player and set up the camera
    private void SpawnAndSetCamera(GameObject playerPrefab, Transform spawnPoint)
    {
        // เพิ่มตำแหน่ง Y ของ spawnPoint ให้สูงขึ้น
        Vector3 spawnPosition = spawnPoint.position + Vector3.up * 1.0f; // เพิ่ม 1 หน่วยในแกน Y

        // สร้างตัวผู้เล่นที่ตำแหน่ง spawnPosition
        GameObject player = Instantiate(playerPrefab, spawnPosition, spawnPoint.rotation);

        // ค้นหา CinemachineVirtualCamera และตั้งค่าการติดตามตัวผู้เล่น
        CinemachineVirtualCamera cinemachineCam = FindObjectOfType<CinemachineVirtualCamera>();
        if (cinemachineCam != null)
        {
            cinemachineCam.Follow = player.transform;
            cinemachineCam.LookAt = player.transform;
        }
    }

}
