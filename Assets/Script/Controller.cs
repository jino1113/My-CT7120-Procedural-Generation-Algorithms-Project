using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Controller : MonoBehaviour
{
    public float speed;
    public float rotationSpeed;

    void Update()
    {

        var rotaion = this.transform.rotation.eulerAngles;
        rotaion += new Vector3(

            Input.GetAxis("Pitch") * rotationSpeed * Time.deltaTime,
            Input.GetAxis("Yaw") * rotationSpeed * Time.deltaTime,
            Input.GetAxis("Roll") * rotationSpeed * Time.deltaTime);
        this.transform.rotation = Quaternion.Euler(rotaion);

        //Old input 
        this.transform.position += this.transform.forward * Input.GetAxis("Horizontal") * speed * Time.deltaTime;
        this.transform.position += this.transform.right * Input.GetAxis("Vertical") * speed * Time.deltaTime;
    }
}
