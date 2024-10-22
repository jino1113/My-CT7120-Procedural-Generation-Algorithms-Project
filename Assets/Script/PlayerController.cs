using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private PlayerControl inputActions;
    private Vector3 moveInput;
    private CharacterController characterController;

    public float moveSpeed = 5f;

    private void Awake()
    {
        inputActions = new PlayerControl();
        characterController = GetComponent<CharacterController>();
    }

    private void OnEnable()
    {
        inputActions.Player.Enable();
        inputActions.Player.OnMove.performed += OnMove;
        inputActions.Player.OnMove.canceled += OnMoveCanceled; // เพิ่มการจัดการเมื่อปล่อยปุ่ม
    }

    private void OnDisable()
    {
        inputActions.Player.Disable();
    }

    private void OnMove(InputAction.CallbackContext context)
    {
        Vector2 input = context.ReadValue<Vector2>();
        moveInput = new Vector3(input.x, 0, input.y);
        moveInput = Camera.main.transform.TransformDirection(moveInput);
        moveInput.y = 0; // ตั้งค่า Y เป็น 0
    }

    private void OnMoveCanceled(InputAction.CallbackContext context)
    {
        moveInput = Vector3.zero; // หยุดการเคลื่อนที่เมื่อปล่อยปุ่ม
    }


    private void Move()
    {
        // เคลื่อนที่ตามค่าของ moveInput ที่มี Y อยู่
        Vector3 movement = new Vector3(moveInput.x, moveInput.y, moveInput.z) * moveSpeed * Time.deltaTime;

        // ใช้ characterController ในการเคลื่อนที่
        characterController.Move(movement);
    }

    private void Update()
    {
        Move(); // เรียกฟังก์ชัน Move() ที่ทำการเคลื่อนที่
    }
}
