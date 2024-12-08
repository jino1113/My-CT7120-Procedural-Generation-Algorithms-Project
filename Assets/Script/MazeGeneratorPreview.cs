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
    // ใช้กำหนด Prefab ของเซลล์ในเขาวงกต ซึ่ง Prefab นี้จะถูก Instantiate (สร้างใหม่) ระหว่างการสร้างเขาวงกต / 
    // This defines the prefab of each cell in the maze, which will be instantiated during maze generation.

    [SerializeField] private int _mazeWidth = 10;
    // ความกว้างของเขาวงกตในจำนวนเซลล์ / Number of cells in the width of the maze.

    [SerializeField] private int _mazeDepth = 10;
    // ความลึกของเขาวงกตในจำนวนเซลล์ / Number of cells in the depth of the maze.

    private MazeCell[,] _mazeGrid;
    // ตัวแปรสำหรับเก็บเซลล์ทั้งหมดในรูปแบบ 2 มิติ / 2D array to store all maze cells.

    [Header("UI")]
    [SerializeField] private TMP_InputField mazeWidthInput;
    // Input field ที่ให้ผู้ใช้งานสามารถใส่ค่าความกว้างของเขาวงกตได้ / Input field for the user to input maze width.

    [SerializeField] private TMP_InputField mazeDepthInput;
    // Input field ที่ให้ผู้ใช้งานสามารถใส่ค่าความลึกของเขาวงกตได้ / Input field for the user to input maze depth.

    [SerializeField] private Button regenerateButton;
    // ปุ่มที่ใช้สำหรับสร้างเขาวงกตใหม่ / Button to regenerate the maze.

    [SerializeField] private TextMeshProUGUI debugText;
    // Text ที่ใช้แสดงข้อความแจ้งเตือนหรือสถานะปัจจุบันของการสร้างเขาวงกต / Text for displaying debug messages or the current status of the maze generation.

    private void Start()
    {
        // ตั้งค่าเริ่มต้นใน InputField โดยใช้ค่าของ `_mazeWidth` และ `_mazeDepth` / Set the initial values of input fields with `_mazeWidth` and `_mazeDepth`.
        if (mazeWidthInput != null) mazeWidthInput.text = _mazeWidth.ToString();
        if (mazeDepthInput != null) mazeDepthInput.text = _mazeDepth.ToString();

        // เพิ่ม Listener ให้ปุ่ม regenerateButton เพื่อเรียก `OnRegenerateButtonClicked` เมื่อคลิก / Add a listener to the regenerate button to call `OnRegenerateButtonClicked`.
        regenerateButton.onClick.AddListener(OnRegenerateButtonClicked);

        // เรียกฟังก์ชันเพื่อสร้างเขาวงกตเริ่มต้น / Generate the initial maze.
        GenerateMazeGrid();
    }

    private void OnRegenerateButtonClicked()
    {
        // เมื่อคลิกปุ่มให้ตรวจสอบค่าความกว้างที่ใส่ใน InputField และอัปเดต `_mazeWidth` / On button click, validate and update `_mazeWidth` based on user input.
        if (int.TryParse(mazeWidthInput.text, out int newWidth))
        {
            if (newWidth >= 1 && newWidth <= 20)
            {
                _mazeWidth = newWidth; // ใช้ค่าที่กำหนดถ้าอยู่ในช่วงที่อนุญาต / Update width if within allowed range.
            }
            else
            {
                ShowDebugMessage("Maze width must be between 1 and 20.");
                mazeWidthInput.text = Mathf.Clamp(newWidth, 1, 20).ToString();
                // ถ้าเกินช่วงจะ Clamp (จำกัด) ค่าให้อยู่ในขอบเขตที่กำหนด / Clamp the input to the allowed range.
            }
        }
        else
        {
            ShowDebugMessage("Invalid input! Please enter a valid number for Maze Width.");
        }

        // ทำเช่นเดียวกันกับค่าความลึก / Same process for maze depth.
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

        // เคลียร์เขาวงกตเก่าออกก่อนสร้างใหม่ / Clear the previous maze before generating a new one.
        ClearMaze();
        GenerateMazeGrid();
    }

    private void GenerateMazeGrid()
    {
        _mazeGrid = new MazeCell[_mazeWidth, _mazeDepth];
        // สร้างตารางเซลล์ 2 มิติ ขนาดตามที่กำหนดไว้ใน `_mazeWidth` และ `_mazeDepth` / Create a 2D grid of maze cells based on the specified width and depth.

        for (int x = 0; x < _mazeWidth; x++)
        {
            for (int z = 0; z < _mazeDepth; z++)
            {
                // สร้างเซลล์ในตำแหน่ง (x, z) โดยใช้ Prefab `_mazeCellPrefab` / Instantiate cells at (x, z) using the `_mazeCellPrefab`.
                _mazeGrid[x, z] = Instantiate(_mazeCellPrefab, new Vector3(x, 0, z), Quaternion.identity, transform);
            }
        }

        // เริ่มกระบวนการสร้างเขาวงกตโดยเริ่มจากเซลล์แรก / Start maze generation from the first cell.
        StartCoroutine(GenerateMaze(null, _mazeGrid[0, 0]));
    }

    private IEnumerator GenerateMaze(MazeCell previousCell, MazeCell currentCell)
    {
        if (currentCell == null) yield break;

        currentCell.Visit();
        // ทำเครื่องหมายว่าเซลล์นี้ถูกเยี่ยมชมแล้ว / Mark this cell as visited.

        if (previousCell != null) ClearWalls(previousCell, currentCell);
        // ลบกำแพงระหว่างเซลล์ก่อนหน้าและเซลล์ปัจจุบัน / Remove walls between the previous and current cell.

        yield return new WaitForSeconds(0.05f);
        // รอเล็กน้อยเพื่อเพิ่มความชัดเจนในกระบวนการสร้าง / Wait to visually represent the generation process.

        var nextCell = GetNextUnvisitedCell(currentCell);
        // ค้นหาเซลล์ที่ยังไม่ได้เยี่ยมชมรอบ ๆ เซลล์ปัจจุบัน / Find unvisited cells around the current cell.

        while (nextCell != null)
        {
            yield return GenerateMaze(currentCell, nextCell);
            // เรียกใช้กระบวนการสร้างซ้ำสำหรับเซลล์ถัดไป / Recursively call the maze generation process for the next cell.
            nextCell = GetNextUnvisitedCell(currentCell);
        }
    }

    private MazeCell GetNextUnvisitedCell(MazeCell currentCell)
    {
        var unvisitedCells = GetUnvisitedCells(currentCell).ToList();
        // ดึงเซลล์ที่ยังไม่ได้เยี่ยมชมมาในรูปแบบ List / Retrieve unvisited cells as a list.

        if (unvisitedCells.Count == 0) return null;
        // ถ้าไม่มีเซลล์ที่ยังไม่ได้เยี่ยมชม ให้หยุด / Stop if no unvisited cells are available.

        return unvisitedCells[Random.Range(0, unvisitedCells.Count)];
        // เลือกเซลล์ที่ยังไม่ได้เยี่ยมชมแบบสุ่ม / Randomly pick an unvisited cell.
    }

    private IEnumerable<MazeCell> GetUnvisitedCells(MazeCell currentCell)
    {
        int x = (int)currentCell.transform.position.x;
        int z = (int)currentCell.transform.position.z;

        // ตรวจสอบเซลล์รอบ ๆ (ด้านขวา ซ้าย หน้า หลัง) และคืนค่าเซลล์ที่ยังไม่ได้เยี่ยมชม / Check surrounding cells and return unvisited ones.
        if (x + 1 < _mazeWidth && !_mazeGrid[x + 1, z].IsVisited) yield return _mazeGrid[x + 1, z];
        if (x - 1 >= 0 && !_mazeGrid[x - 1, z].IsVisited) yield return _mazeGrid[x - 1, z];
        if (z + 1 < _mazeDepth && !_mazeGrid[x, z + 1].IsVisited) yield return _mazeGrid[x, z + 1];
        if (z - 1 >= 0 && !_mazeGrid[x, z - 1].IsVisited) yield return _mazeGrid[x, z - 1];
    }

    private void ClearWalls(MazeCell previousCell, MazeCell currentCell)
    {
        // ลบกำแพงระหว่างสองเซลล์ตามทิศทางการเคลื่อนที่ / Remove walls between two cells based on movement direction.
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
        // ทำลายเซลล์ทั้งหมดในเขาวงกต / Destroy all cells in the maze.
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
        // แสดงข้อความแจ้งเตือนใน debugText หรือ Console / Display debug messages in `debugText` or console.
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
        // ทำให้ข้อความ debug ค่อย ๆ จางลง / Gradually fade out the debug message.
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
