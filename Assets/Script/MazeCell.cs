using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeCell : MonoBehaviour
{
    // อ้างอิงถึงกำแพงต่างๆ ของเซลล์ในเขาวงกต
    [SerializeField]
    private GameObject _leftWall; // กำแพงด้านซ้ายของเซลล์

    [SerializeField]
    private GameObject _rightWall; // กำแพงด้านขวาของเซลล์

    [SerializeField]
    private GameObject _frontWall; // กำแพงด้านหน้าของเซลล์

    [SerializeField]
    private GameObject _backWall; // กำแพงด้านหลังของเซลล์

    [SerializeField]
    private GameObject _unvisitedBlock; // บล็อกที่แสดงว่าเซลล์ยังไม่ได้เยี่ยมชม

    // ตัวแปรบอกสถานะว่าเซลล์ถูกเยี่ยมชมแล้วหรือยัง
    public bool IsVisited { get; private set; }

    // ฟังก์ชันที่ถูกเรียกเมื่อเซลล์ถูกเยี่ยมชม
    public void Visit()
    {
        IsVisited = true; // ตั้งสถานะว่าเซลล์ถูกเยี่ยมชมแล้ว
        _unvisitedBlock.SetActive(false); // ปิดการแสดงผลของบล็อกที่แสดงสถานะ "ยังไม่ได้เยี่ยมชม"
    }

    // ฟังก์ชันสำหรับลบกำแพงด้านซ้าย
    public void ClearLeftWall()
    {
        _leftWall.SetActive(false); // ปิดการแสดงผลกำแพงด้านซ้าย
    }

    // ฟังก์ชันสำหรับลบกำแพงด้านขวา
    public void ClearRightWall()
    {
        _rightWall.SetActive(false); // ปิดการแสดงผลกำแพงด้านขวา
    }

    // ฟังก์ชันสำหรับลบกำแพงด้านหน้า
    public void ClearFrontWall()
    {
        _frontWall.SetActive(false); // ปิดการแสดงผลกำแพงด้านหน้า
    }

    // ฟังก์ชันสำหรับลบกำแพงด้านหลัง
    public void ClearBackWall()
    {
        _backWall.SetActive(false); // ปิดการแสดงผลกำแพงด้านหลัง
    }
}
