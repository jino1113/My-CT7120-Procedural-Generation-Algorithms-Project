using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomTexture : MonoBehaviour
{
    public Material targetMaterial; // ��˹� Material �ͧ GameObject // Assign the Material of the GameObject
    public int textureWidth = 256; // �������ҧ�ͧ Texture // Width of the Texture
    public int textureHeight = 256; // �����٧�ͧ Texture // Height of the Texture
    public float scale = 20f; // ��ҷ����㹡�û�Ѻ��Ҵ�ͧ Perlin noise // Value used to scale Perlin noise
    public float speed = 0.1f; // ��������㹡�â�Ѻ Texture // Speed at which the Texture moves
    public float directionChangeInterval = 2f; // �������ҷ�������¹��ȷҧ // Time interval to change direction

    private Vector2 offset; // ���������Ѻ�Ѵ�纵��˹� offset // Variable to store the offset position
    private Vector2 direction; // ���������Ѻ��ȷҧ��â�Ѻ // Variable to store the movement direction

    private void Start()
    {
        if (targetMaterial != null)
        {
            GenerateTexture(0f, 0f); // ���� Texture ����͵���Ф������ // Generate texture at the start
        }

        // ��駤�������������Ѻ offset ��з�ȷҧ // Set initial offset and direction
        offset = new Vector2(0f, 0f);
        ChangeDirection(); // ���¡��ѧ��ѹ��������¹��ȷҧ // Call the function to change direction
    }

    private void Update()
    {
        // �Ѿഷ Texture ������˹觢ͧ������ // Update texture based on player's position
        offset += direction * speed * Time.deltaTime; // ��Ѻ offset �����ȷҧ // Adjust offset based on direction
        GenerateTexture(offset.x, offset.y); // ���� Texture ����ء frame // Generate new texture every frame

        // ����¹��ȷҧ����������ҷ���˹� // Change direction based on the specified interval
        if (Time.time % directionChangeInterval < speed * Time.deltaTime)
        {
            ChangeDirection(); // ����¹��ȷҧ // Change direction
        }
    }

    private void GenerateTexture(float offsetX, float offsetY)
    {
        Texture2D texture = new Texture2D(textureWidth, textureHeight); // ���ҧ Texture ���� // Create a new Texture

        for (int x = 0; x < textureWidth; x++)
        {
            for (int y = 0; y < textureHeight; y++)
            {
                float xCoord = ((float)x / textureWidth + offsetX) * scale; // �� offset ����Ѻ��Ѻ // Use offset to shift
                float yCoord = ((float)y / textureHeight + offsetY) * scale; // �� offset ����Ѻ��Ѻ // Use offset to shift

                float sample = Mathf.PerlinNoise(xCoord, yCoord); // ������� Perlin noise // Sample Perlin noise value
                Color color = new Color(sample, sample, sample); // ���ҧ�ըҡ��� Perlin noise // Create color from Perlin noise value
                texture.SetPixel(x, y, color); // ��駤�� Pixel �ͧ Texture // Set the Texture's Pixel
            }
        }

        texture.Apply(); // �Ѿഷ Texture // Apply the Texture
        targetMaterial.mainTexture = texture; // ��駤�� Texture ���Ѻ Material // Assign the Texture to the Material
    }

    private void ChangeDirection()
    {
        // ����¹��ȷҧ���� // Randomly change direction
        direction = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized; // ������ȷҧ���� // Generate new random direction
    }
}
