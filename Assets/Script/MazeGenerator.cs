using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Unity.AI.Navigation;
using UnityEngine.AI;

public class MazeGenerator : MonoBehaviour
{
    // Prefab for generating individual maze cells / Prefab สำหรับสร้างเซลล์ของเขาวงกต
    [SerializeField]
    private MazeCell _mazeCellPrefab;

    // Prefab for the player character / Prefab ของตัวผู้เล่น
    [SerializeField]
    private GameObject _playerPrefab;

    // Prefab for the maze exit / Prefab สำหรับจุดออกจากเขาวงกต
    [SerializeField]
    private GameObject _exitPrefab;

    // Prefab for enemy characters / Prefab ของศัตรู (ลูกบอล)
    [SerializeField]
    private GameObject _enemyBallPrefab;

    // Width of the maze grid / ความกว้างของเขาวงกต
    [SerializeField]
    private int _mazeWidth;

    // Depth of the maze grid / ความลึกของเขาวงกต
    [SerializeField]
    private int _mazeDepth;

    // Minimum distance between the player and enemies / ระยะห่างขั้นต่ำของศัตรูจากผู้เล่น
    [SerializeField]
    private float minimumDistanceFromPlayer = 10.0f;

    // The 2D array representing the maze grid / Grid ที่ใช้เก็บเซลล์ของเขาวงกต
    private MazeCell[,] _mazeGrid;

    // Spawn point for the player / จุดเริ่มต้นของผู้เล่นในเขาวงกต
    private Transform spawnPoint;

    // Returns the maze's width / คืนค่าความกว้างของเขาวงกต
    public int GetMazeWidth()
    {
        return _mazeWidth;
    }

    // Returns the maze's depth / คืนค่าความลึกของเขาวงกต
    public int GetMazeDepth()
    {
        return _mazeDepth;
    }

    void Start()
    {
        // Generate the maze grid / สร้างเซลล์ของเขาวงกต
        _mazeGrid = new MazeCell[_mazeWidth, _mazeDepth];
        for (int x = 0; x < _mazeWidth; x++)
        {
            for (int z = 0; z < _mazeDepth; z++)
            {
                // Instantiate and position each cell / สร้างเซลล์และวางในตำแหน่งที่กำหนด
                _mazeGrid[x, z] = Instantiate(_mazeCellPrefab, new Vector3(x, 0, z), Quaternion.identity, transform);
                _mazeGrid[x, z].transform.localPosition = new Vector3(x, 0, z);
            }
        }

        // Generate the maze paths / เริ่มสร้างเขาวงกต
        GenerateMaze(null, _mazeGrid[0, 0]);

        // Build a NavMesh for AI navigation / สร้าง NavMesh สำหรับ AI
        GetComponent<NavMeshSurface>().BuildNavMesh();

        // Set the player's spawn point / กำหนดจุดเกิดของผู้เล่น
        spawnPoint = _mazeGrid[0, 0].transform;

        // Spawn the player and configure the camera / สร้างตัวผู้เล่นและกล้อง
        SpawnAndSetCamera(_playerPrefab, spawnPoint);

        // Spawn the maze exit / สร้างจุดออกจากเขาวงกต
        Instantiate(_exitPrefab, _mazeGrid[_mazeWidth - 1, _mazeDepth - 1].transform.position, Quaternion.identity);

        // Spawn the enemies / สร้างศัตรู
        SpawnEnemyBall(spawnPoint, minimumDistanceFromPlayer);
    }

    private void SpawnEnemyBall(Transform playerTransform, float customMinimumDistance)
    {
        Vector3 spawnPosition;
        MazeCell randomCell;

        // Find a random position far enough from the player / เลือกตำแหน่งสุ่มที่อยู่ห่างจากผู้เล่น
        do
        {
            int x = Random.Range(0, _mazeWidth);
            int z = Random.Range(0, _mazeDepth);

            randomCell = _mazeGrid[x, z];
            spawnPosition = randomCell.transform.position + Vector3.up * 0.5f;
        } while (Vector3.Distance(spawnPosition, playerTransform.position) < customMinimumDistance);

        // Ensure the position is on the NavMesh / ตรวจสอบว่าตำแหน่งนั้นอยู่บน NavMesh หรือไม่
        NavMeshHit hit;
        float searchRadius = 5.0f;

        if (NavMesh.SamplePosition(spawnPosition, out hit, searchRadius, NavMesh.AllAreas))
        {
            // Instantiate the enemy at the valid position / สร้างศัตรูที่ตำแหน่งที่หาได้
            GameObject enemyBall = Instantiate(_enemyBallPrefab, hit.position, Quaternion.identity);

            // Assign the player as the enemy's target / ตั้งค่าผู้เล่นเป็นเป้าหมายของศัตรู
            GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
            if (playerObject != null)
            {
                EnemyBall enemyBallScript = enemyBall.GetComponent<EnemyBall>();
                if (enemyBallScript != null)
                {
                    enemyBallScript.player = playerObject.transform;
                }
                else
                {
                    Debug.LogWarning("EnemyBall prefab does not have an EnemyBall script attached!");
                }
            }
            else
            {
                Debug.LogWarning("No GameObject with Tag 'Player' found in the scene!");
            }
        }
        else
        {
            Debug.LogWarning("Failed to find a valid position on the NavMesh for EnemyBall!");
        }
    }

    private void GenerateMaze(MazeCell previousCell, MazeCell currentCell)
    {
        // Mark the current cell as visited / ทำเครื่องหมายเซลล์ว่าเคยถูกเยี่ยมชมแล้ว
        currentCell.Visit();

        // Remove walls between the current and previous cell / ลบกำแพงระหว่างเซลล์ปัจจุบันและเซลล์ก่อนหน้า
        ClearWalls(previousCell, currentCell);

        MazeCell nextCell;
        do
        {
            // Find the next unvisited cell / เลือกเซลล์ถัดไปที่ยังไม่ได้เยี่ยมชม
            nextCell = GetNextUnvisitedCell(currentCell);
            if (nextCell != null)
            {
                GenerateMaze(currentCell, nextCell);
            }
        } while (nextCell != null);
    }

    private MazeCell GetNextUnvisitedCell(MazeCell currentCell)
    {
        // Get a list of unvisited neighboring cells / รับเซลล์ที่ยังไม่ถูกเยี่ยมชมรอบๆ
        var unvisitedCells = GetUnvisitedCells(currentCell);
        return unvisitedCells.OrderBy(_ => Random.Range(1, 10)).FirstOrDefault();
    }

    private IEnumerable<MazeCell> GetUnvisitedCells(MazeCell currentCell)
    {
        int x = (int)currentCell.transform.localPosition.x;
        int z = (int)currentCell.transform.localPosition.z;

        // Check for unvisited neighboring cells / ตรวจสอบเซลล์รอบๆ ว่ามีเซลล์ที่ยังไม่ถูกเยี่ยมชมหรือไม่
        if (x + 1 < _mazeWidth && !_mazeGrid[x + 1, z].IsVisited) yield return _mazeGrid[x + 1, z];
        if (x - 1 >= 0 && !_mazeGrid[x - 1, z].IsVisited) yield return _mazeGrid[x - 1, z];
        if (z + 1 < _mazeDepth && !_mazeGrid[x, z + 1].IsVisited) yield return _mazeGrid[x, z + 1];
        if (z - 1 >= 0 && !_mazeGrid[x, z - 1].IsVisited) yield return _mazeGrid[x, z - 1];
    }

    private void ClearWalls(MazeCell previousCell, MazeCell currentCell)
    {
        if (previousCell == null) return;

        // Remove walls between adjacent cells / ลบกำแพงระหว่างเซลล์ที่ติดกัน
        if (previousCell.transform.localPosition.x < currentCell.transform.localPosition.x)
        {
            previousCell.ClearRightWall();
            currentCell.ClearLeftWall();
            return;
        }

        if (previousCell.transform.localPosition.x > currentCell.transform.localPosition.x)
        {
            previousCell.ClearLeftWall();
            currentCell.ClearRightWall();
            return;
        }

        if (previousCell.transform.localPosition.z < currentCell.transform.localPosition.z)
        {
            previousCell.ClearFrontWall();
            currentCell.ClearBackWall();
            return;
        }

        if (previousCell.transform.localPosition.z > currentCell.transform.localPosition.z)
        {
            previousCell.ClearBackWall();
            currentCell.ClearFrontWall();
            return;
        }
    }

    private void SpawnAndSetCamera(GameObject playerPrefab, Transform spawnPoint)
    {
        // Instantiate the player object / สร้างตัวผู้เล่น
        Vector3 spawnPosition = spawnPoint.position + Vector3.up * 1.0f;
        GameObject player = Instantiate(playerPrefab, spawnPosition, spawnPoint.rotation);
        player.tag = "Player";

        // Configure the camera to follow the player / ตั้งค่ากล้องให้ติดตามผู้เล่น
        CinemachineVirtualCamera cinemachineCam = FindObjectOfType<CinemachineVirtualCamera>();
        if (cinemachineCam != null)
        {
            cinemachineCam.Follow = player.transform;
            cinemachineCam.LookAt = player.transform;
        }
    }
}
