using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomSpawner : MonoBehaviour
{
    public GameObject prefab;
    public int spawnerCount = 10;

    private void Update()
    {
        for (int i=0; i < spawnerCount; i++ )
        {
            Vector3 randomPosition = new Vector3(Random.Range(-1, 50), Random.Range(7, 90), Random.Range(7, 90));
            Vector3 randomPosition2 = new Vector3(Random.Range(0, 10), Random.Range(7, 90), Random.Range(4, 10));
            Instantiate(prefab, randomPosition, Quaternion.identity);
            Instantiate(prefab, randomPosition2, Quaternion.identity);
        }

    }
}
