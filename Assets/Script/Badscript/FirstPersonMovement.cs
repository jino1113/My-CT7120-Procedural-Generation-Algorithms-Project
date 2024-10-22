using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstPersonMovement : MonoBehaviour
{
    public CharacterController controller;
    public float speed = 12f;

    Vector3 velocity;

    void Update()
    {

        // รับค่า Input การเคลื่อนไหว
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = transform.right * x + transform.forward * z;

        controller.Move(move * speed * Time.deltaTime);
        controller.Move(velocity * Time.deltaTime);
    }
}
