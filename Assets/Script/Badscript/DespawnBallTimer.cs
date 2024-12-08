using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DespawnBallTimer : MonoBehaviour
{
    float timeUntilDespawn = 10.0f;
    float timer = 0.0f;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= timeUntilDespawn)
        {
            gameObject.SetActive(false);
            timer = 0.0f;
        }
    }
}
