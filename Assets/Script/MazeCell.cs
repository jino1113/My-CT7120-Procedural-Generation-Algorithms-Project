using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeCell : MonoBehaviour
{
    // References to the walls of the maze cell / อ้างอิงถึงกำแพงต่างๆ ของเซลล์ในเขาวงกต
    [SerializeField]
    private GameObject _leftWall; // Left wall of the cell / กำแพงด้านซ้ายของเซลล์

    [SerializeField]
    private GameObject _rightWall; // Right wall of the cell / กำแพงด้านขวาของเซลล์

    [SerializeField]
    private GameObject _frontWall; // Front wall of the cell / กำแพงด้านหน้าของเซลล์

    [SerializeField]
    private GameObject _backWall; // Back wall of the cell / กำแพงด้านหลังของเซลล์

    [SerializeField]
    private GameObject _unvisitedBlock; // Block that indicates the cell has not been visited / บล็อกที่แสดงว่าเซลล์ยังไม่ได้เยี่ยมชม

    // Variable to indicate whether the cell has been visited / ตัวแปรบอกสถานะว่าเซลล์ถูกเยี่ยมชมแล้วหรือยัง
    public bool IsVisited { get; private set; }

    // Function called when the cell is visited / ฟังก์ชันที่ถูกเรียกเมื่อเซลล์ถูกเยี่ยมชม
    public void Visit()
    {
        IsVisited = true; // Mark the cell as visited / ตั้งสถานะว่าเซลล์ถูกเยี่ยมชมแล้ว
        _unvisitedBlock.SetActive(false); // Disable the unvisited block indicator / ปิดการแสดงผลของบล็อกที่แสดงสถานะ "ยังไม่ได้เยี่ยมชม"
    }

    // Function to remove the left wall / ฟังก์ชันสำหรับลบกำแพงด้านซ้าย
    public void ClearLeftWall()
    {
        _leftWall.SetActive(false); // Disable the left wall / ปิดการแสดงผลกำแพงด้านซ้าย
    }

    // Function to remove the right wall / ฟังก์ชันสำหรับลบกำแพงด้านขวา
    public void ClearRightWall()
    {
        _rightWall.SetActive(false); // Disable the right wall / ปิดการแสดงผลกำแพงด้านขวา
    }

    // Function to remove the front wall / ฟังก์ชันสำหรับลบกำแพงด้านหน้า
    public void ClearFrontWall()
    {
        _frontWall.SetActive(false); // Disable the front wall / ปิดการแสดงผลกำแพงด้านหน้า
    }

    // Function to remove the back wall / ฟังก์ชันสำหรับลบกำแพงด้านหลัง
    public void ClearBackWall()
    {
        _backWall.SetActive(false); // Disable the back wall / ปิดการแสดงผลกำแพงด้านหลัง
    }
}
