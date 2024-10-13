using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProceduralTerrain : MonoBehaviour
{
    public int width = 256;
    public int height = 256;
    public float scale = 20f;
    public float updateInterval = 5f; 
    public float scaleUpdateInterval = 3f; 
    public float bumpUpdateInterval = 2f; 
    public float scaleIncreaseAmount = 5f; 

    void Start()
    {
        InvokeRepeating("UpdateTexture", 0f, updateInterval); 
        InvokeRepeating("UpdateScale", 0f, scaleUpdateInterval); 
        InvokeRepeating("UpdateBump", 0f, bumpUpdateInterval); 
    }

    void UpdateTexture()
    {
        Renderer renderer = GetComponent<Renderer>();
        renderer.material.mainTexture = GenerateTexture(); 
    }

    void UpdateScale()
    {
        scale += scaleIncreaseAmount; 
        Debug.Log("Scale increased to: " + scale); 
    }

    void UpdateBump()
    {
        Renderer renderer = GetComponent<Renderer>();
        Texture2D bumpMap = GenerateBumpMap(); 
        bumpMap.Apply(); 
        renderer.material.SetTexture("_BumpMap", bumpMap); 
        renderer.material.EnableKeyword("_NORMALMAP"); 
        Debug.Log("Bump map updated!"); 
    }

    Texture2D GenerateTexture()
    {
        Texture2D texture = new Texture2D(width, height);

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                float xCoord = (float)x / width * scale;
                float yCoord = (float)y / height * scale;
                float sample = Mathf.PerlinNoise(xCoord, yCoord); //PerlinNoise
                texture.SetPixel(x, y, new Color(sample, sample, sample)); 
            }
        }

        texture.Apply();
        return texture;
    }

    Texture2D GenerateBumpMap()
    {
        Texture2D bumpMap = new Texture2D(width, height);

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                float xCoord = (float)x / width * scale;
                float yCoord = (float)y / height * scale;
                float sample = Mathf.PerlinNoise(xCoord, yCoord); //PerlinNoise
                bumpMap.SetPixel(x, y, new Color(sample, sample, sample)); 
            }
        }

        bumpMap.Apply();
        return bumpMap;
    }
}
