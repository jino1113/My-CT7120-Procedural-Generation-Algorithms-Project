using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MazeGeneratorPreview : MonoBehaviour
{
    [Header("Maze Settings")]
    [SerializeField] private MazeCell _mazeCellPrefab;
    [SerializeField] private int _mazeWidth = 10;
    [SerializeField] private int _mazeDepth = 10;

    private MazeCell[,] _mazeGrid;

    [Header("UI")]
    [SerializeField] private TMP_InputField mazeWidthInput;
    [SerializeField] private TMP_InputField mazeDepthInput;
    [SerializeField] private Button regenerateButton;
    [SerializeField] private TextMeshProUGUI debugText;

    private void Start()
    {
        // ตั้งค่าเริ่มต้นใน InputField
        if (mazeWidthInput != null) mazeWidthInput.text = _mazeWidth.ToString();
        if (mazeDepthInput != null) mazeDepthInput.text = _mazeDepth.ToString();

        regenerateButton.onClick.AddListener(OnRegenerateButtonClicked);

        GenerateMazeGrid();
    }

    private void OnRegenerateButtonClicked()
    {
        // ตรวจสอบและอัปเดตค่า Maze Width
        if (int.TryParse(mazeWidthInput.text, out int newWidth))
        {
            if (newWidth >= 1 && newWidth <= 20)
            {
                _mazeWidth = newWidth;
            }
            else
            {
                ShowDebugMessage("Maze width must be between 1 and 20.");
                mazeWidthInput.text = Mathf.Clamp(newWidth, 1, 20).ToString();
            }
        }
        else
        {
            ShowDebugMessage("Invalid input! Please enter a valid number for Maze Width.");
        }

        // ตรวจสอบและอัปเดตค่า Maze Depth
        if (int.TryParse(mazeDepthInput.text, out int newDepth))
        {
            if (newDepth >= 1 && newDepth <= 20)
            {
                _mazeDepth = newDepth;
            }
            else
            {
                ShowDebugMessage("Maze depth must be between 1 and 20.");
                mazeDepthInput.text = Mathf.Clamp(newDepth, 1, 20).ToString();
            }
        }
        else
        {
            ShowDebugMessage("Invalid input! Please enter a valid number for Maze Depth.");
        }

        // สร้างเขาวงกตใหม่
        ClearMaze();
        GenerateMazeGrid();
    }

    private void GenerateMazeGrid()
    {
        _mazeGrid = new MazeCell[_mazeWidth, _mazeDepth];

        for (int x = 0; x < _mazeWidth; x++)
        {
            for (int z = 0; z < _mazeDepth; z++)
            {
                _mazeGrid[x, z] = Instantiate(_mazeCellPrefab, new Vector3(x, 0, z), Quaternion.identity, transform);
            }
        }

        StartCoroutine(GenerateMaze(null, _mazeGrid[0, 0]));
    }

    private IEnumerator GenerateMaze(MazeCell previousCell, MazeCell currentCell)
    {
        if (currentCell == null) yield break;

        currentCell.Visit();
        if (previousCell != null) ClearWalls(previousCell, currentCell);

        yield return new WaitForSeconds(0.05f);

        var nextCell = GetNextUnvisitedCell(currentCell);
        while (nextCell != null)
        {
            yield return GenerateMaze(currentCell, nextCell);
            nextCell = GetNextUnvisitedCell(currentCell);
        }
    }

    private MazeCell GetNextUnvisitedCell(MazeCell currentCell)
    {
        var unvisitedCells = GetUnvisitedCells(currentCell).ToList();
        if (unvisitedCells.Count == 0)
        {
            ShowDebugMessage("No cells available, find a new way.");
            return null;
        }

        return unvisitedCells[Random.Range(0, unvisitedCells.Count)];
    }

    private IEnumerable<MazeCell> GetUnvisitedCells(MazeCell currentCell)
    {
        int x = (int)currentCell.transform.position.x;
        int z = (int)currentCell.transform.position.z;

        if (x + 1 < _mazeWidth && !_mazeGrid[x + 1, z].IsVisited) yield return _mazeGrid[x + 1, z];
        if (x - 1 >= 0 && !_mazeGrid[x - 1, z].IsVisited) yield return _mazeGrid[x - 1, z];
        if (z + 1 < _mazeDepth && !_mazeGrid[x, z + 1].IsVisited) yield return _mazeGrid[x, z + 1];
        if (z - 1 >= 0 && !_mazeGrid[x, z - 1].IsVisited) yield return _mazeGrid[x, z - 1];
    }

    private void ClearWalls(MazeCell previousCell, MazeCell currentCell)
    {
        if (previousCell == null) return;

        if (previousCell.transform.position.x < currentCell.transform.position.x)
        {
            previousCell.ClearRightWall();
            currentCell.ClearLeftWall();
        }
        else if (previousCell.transform.position.x > currentCell.transform.position.x)
        {
            previousCell.ClearLeftWall();
            currentCell.ClearRightWall();
        }
        else if (previousCell.transform.position.z < currentCell.transform.position.z)
        {
            previousCell.ClearFrontWall();
            currentCell.ClearBackWall();
        }
        else if (previousCell.transform.position.z > currentCell.transform.position.z)
        {
            previousCell.ClearBackWall();
            currentCell.ClearFrontWall();
        }
    }

    private void ClearMaze()
    {
        if (_mazeGrid != null)
        {
            foreach (var cell in _mazeGrid)
            {
                if (cell != null) Destroy(cell.gameObject);
            }
        }
    }

    private void ShowDebugMessage(string message)
    {
        if (debugText != null)
        {
            debugText.text = message;
            StartCoroutine(FadeDebugMessage());
        }
        else
        {
            Debug.Log(message);
        }
    }

    private IEnumerator FadeDebugMessage()
    {
        debugText.color = new Color(debugText.color.r, debugText.color.g, debugText.color.b, 1);

        yield return new WaitForSeconds(0.05f);

        for (float alpha = 1; alpha > 0; alpha -= Time.deltaTime)
        {
            debugText.color = new Color(debugText.color.r, debugText.color.g, debugText.color.b, alpha);
            yield return null;
        }

        debugText.text = "";
    }
}
