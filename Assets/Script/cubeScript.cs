using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cubeScript : MonoBehaviour
{

    private float maxOffset = 20f;

    // Update is called once per frame
    void Update()
    {
        float noise = Mathf.PerlinNoise(transform.position.x /10 + Time.time, transform.position.z / 10 + Time.time);
        transform.localScale = new Vector3(1f, noise * maxOffset, 1f);
    }
}
