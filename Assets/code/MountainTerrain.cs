using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MountainTerrain : MonoBehaviour
{
    public float scale = 5.0f; // ขนาดความนูนของภูเขา
    public float heightMultiplier = 2.0f; // ความสูงของความนูน
    public float detail = 10.0f; // ระดับความละเอียด

    private Mesh mesh;
    private Vector3[] vertices;

    void Start()
    {
        mesh = GetComponent<MeshFilter>().mesh;
        vertices = mesh.vertices;

        GenerateTerrain();
        UpdateMesh();
    }

    void GenerateTerrain()
    {
        for (int i = 0; i < vertices.Length; i++)
        {
            float xCoord = vertices[i].x * scale;
            float zCoord = vertices[i].z * scale;

            float y = Mathf.PerlinNoise(xCoord / detail, zCoord / detail) * heightMultiplier;
            vertices[i] = new Vector3(vertices[i].x, y, vertices[i].z);
        }
    }

    void UpdateMesh()
    {
        mesh.vertices = vertices;
        mesh.RecalculateNormals();
    }
}
