using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private PlayerControl inputActions; // อินพุตคอนโทรลที่สร้างจาก Input System / Input actions generated from the Input System
    private Vector3 moveInput; // ค่าอินพุตการเคลื่อนที่ / Movement input vector
    private CharacterController characterController; // ตัวควบคุมการเคลื่อนที่ของตัวละคร / Character controller component

    public float moveSpeed = 5f; // ความเร็วในการเคลื่อนที่ / Movement speed
    public float gravity = -9.8f; // ค่าความเร่งโน้มถ่วง / Gravity value
    private float verticalVelocity; // ความเร็วในแกน Y สำหรับแรงโน้มถ่วง / Vertical velocity for gravity

    private void Awake()
    {
        // กำหนดค่าเริ่มต้นสำหรับอินพุตและตัวควบคุมตัวละคร / Initialize input actions and character controller
        inputActions = new PlayerControl();
        characterController = GetComponent<CharacterController>();
    }

    private void OnEnable()
    {
        // เปิดใช้งานการควบคุมผู้เล่น / Enable player controls
        inputActions.Player.Enable();
        inputActions.Player.OnMove.performed += OnMove; // เพิ่ม Listener เมื่อมีการเคลื่อนที่ / Add listener for movement input
        inputActions.Player.OnMove.canceled += OnMoveCanceled; // เพิ่ม Listener เมื่อหยุดเคลื่อนที่ / Add listener for canceled movement input
    }

    private void OnDisable()
    {
        // ปิดใช้งานการควบคุมผู้เล่น / Disable player controls
        inputActions.Player.Disable();
    }

    private void OnMove(InputAction.CallbackContext context)
    {
        // อ่านค่า Vector2 จากอินพุตและเปลี่ยนเป็น Vector3 / Read Vector2 input and convert to Vector3
        Vector2 input = context.ReadValue<Vector2>();
        moveInput = new Vector3(input.x, 0, input.y);

        // แปลงทิศทางการเคลื่อนที่ให้สัมพันธ์กับกล้อง / Transform movement direction relative to the camera
        moveInput = Camera.main.transform.TransformDirection(moveInput);
        moveInput.y = 0; // รีเซ็ตค่าแกน Y เพื่อให้เคลื่อนที่ในแนวราบเท่านั้น / Reset Y to ensure horizontal movement only
    }

    private void OnMoveCanceled(InputAction.CallbackContext context)
    {
        // หยุดการเคลื่อนที่เมื่อไม่มีอินพุต / Stop movement when input is canceled
        moveInput = Vector3.zero;
    }

    private void Move()
    {
        // คำนวณการเคลื่อนที่ / Calculate movement vector
        Vector3 movement = new Vector3(moveInput.x, moveInput.y, moveInput.z) * moveSpeed * Time.deltaTime;

        // ใช้ CharacterController ในการเคลื่อนที่ / Use CharacterController to move the character
        characterController.Move(movement);
    }

    private void Update()
    {
        Move(); // เรียกฟังก์ชัน Move() เพื่อจัดการการเคลื่อนที่ / Call Move() to handle movement
    }
}
