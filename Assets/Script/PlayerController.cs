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
        inputActions.Player.OnMove.canceled += OnMoveCanceled; // ������èѴ�������ͻ���»���
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
        moveInput.y = 0; // ��駤�� Y �� 0
    }

    private void OnMoveCanceled(InputAction.CallbackContext context)
    {
        moveInput = Vector3.zero; // ��ش�������͹�������ͻ���»���
    }


    private void Move()
    {
        // ����͹�������Ңͧ moveInput ����� Y ����
        Vector3 movement = new Vector3(moveInput.x, moveInput.y, moveInput.z) * moveSpeed * Time.deltaTime;

        // �� characterController 㹡������͹���
        characterController.Move(movement);
    }

    private void Update()
    {
        Move(); // ���¡�ѧ��ѹ Move() ���ӡ������͹���
    }
}
