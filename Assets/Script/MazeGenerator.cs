using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Unity.AI.Navigation;

public class MazeGenerator : MonoBehaviour
{
    [SerializeField]
    private MazeCell _mazeCellPrefab;

    [SerializeField]
    private GameObject _playerPrefab;

    [SerializeField]
    private GameObject _exitPrefab;

    [SerializeField]
    private GameObject _enemyBallPrefab;

    [SerializeField]
    private int _mazeWidth;

    [SerializeField]
    private int _mazeDepth;

    [SerializeField]
    private float minimumDistanceFromPlayer = 10.0f; // ระยะห่างขั้นต่ำจากผู้เล่น / Minimum distance from the player

    private MazeCell[,] _mazeGrid;
    private Transform spawnPoint;

    void Start()
    {
        _mazeGrid = new MazeCell[_mazeWidth, _mazeDepth];

        for (int x = 0; x < _mazeWidth; x++)
        {
            for (int z = 0; z < _mazeDepth; z++)
            {
                _mazeGrid[x, z] = Instantiate(_mazeCellPrefab, new Vector3(x, 0, z), Quaternion.identity, transform);
                _mazeGrid[x, z].transform.localPosition = new Vector3(x, 0, z);
            }
        }

        GenerateMaze(null, _mazeGrid[0, 0]);
        GetComponent<NavMeshSurface>().BuildNavMesh();


        spawnPoint = _mazeGrid[0, 0].transform;

        SpawnAndSetCamera(_playerPrefab, spawnPoint);

        Instantiate(_exitPrefab, _mazeGrid[_mazeWidth - 1, _mazeDepth - 1].transform.position, Quaternion.identity);

        SpawnEnemyBall(spawnPoint, minimumDistanceFromPlayer);

    }

    private void SpawnEnemyBall(Transform playerTransform, float customMinimumDistance)
    {
        Vector3 spawnPosition;
        MazeCell randomCell;

        do
        {
            int x = Random.Range(0, _mazeWidth); // สุ่มตำแหน่ง x ภายในขอบเขต
            int z = Random.Range(0, _mazeDepth); // สุ่มตำแหน่ง z ภายในขอบเขต

            randomCell = _mazeGrid[x, z];
            spawnPosition = randomCell.transform.position + Vector3.up * 0.5f; // ตำแหน่งของ MazeCell

        } while (Vector3.Distance(spawnPosition, playerTransform.position) < customMinimumDistance);

        GameObject enemyBall = Instantiate(_enemyBallPrefab, spawnPosition, Quaternion.identity);
        enemyBall.GetComponent<EnemyBall>().player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void GenerateMaze(MazeCell previousCell, MazeCell currentCell)
    {
        currentCell.Visit();
        ClearWalls(previousCell, currentCell);

        MazeCell nextCell;
        do
        {
            nextCell = GetNextUnvisitedCell(currentCell);
            if (nextCell != null)
            {
                GenerateMaze(currentCell, nextCell);
            }
        } while (nextCell != null);
    }

    private MazeCell GetNextUnvisitedCell(MazeCell currentCell)
    {
        var unvisitedCells = GetUnvisitedCells(currentCell);
        return unvisitedCells.OrderBy(_ => Random.Range(1, 10)).FirstOrDefault();
    }

    private IEnumerable<MazeCell> GetUnvisitedCells(MazeCell currentCell)
    {
        int x = (int)currentCell.transform.localPosition.x;
        int z = (int)currentCell.transform.localPosition.z;

        if (x + 1 < _mazeWidth && !_mazeGrid[x + 1, z].IsVisited) yield return _mazeGrid[x + 1, z];
        if (x - 1 >= 0 && !_mazeGrid[x - 1, z].IsVisited) yield return _mazeGrid[x - 1, z];
        if (z + 1 < _mazeDepth && !_mazeGrid[x, z + 1].IsVisited) yield return _mazeGrid[x, z + 1];
        if (z - 1 >= 0 && !_mazeGrid[x, z - 1].IsVisited) yield return _mazeGrid[x, z - 1];
    }

    private void ClearWalls(MazeCell previousCell, MazeCell currentCell)
    {
        if (previousCell == null) return;

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
        Vector3 spawnPosition = spawnPoint.position + Vector3.up * 1.0f;
        GameObject player = Instantiate(playerPrefab, spawnPosition, spawnPoint.rotation);
        player.tag = "Player";

        CinemachineVirtualCamera cinemachineCam = FindObjectOfType<CinemachineVirtualCamera>();
        if (cinemachineCam != null)
        {
            cinemachineCam.Follow = player.transform;
            cinemachineCam.LookAt = player.transform;
        }
    }
}