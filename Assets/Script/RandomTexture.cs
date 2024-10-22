using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomTexture : MonoBehaviour
{
    public Material targetMaterial; // กำหนด Material ของ GameObject // Assign the Material of the GameObject
    public int textureWidth = 256; // ความกว้างของ Texture // Width of the Texture
    public int textureHeight = 256; // ความสูงของ Texture // Height of the Texture
    public float scale = 20f; // ค่าที่ใช้ในการปรับขนาดของ Perlin noise // Value used to scale Perlin noise
    public float speed = 0.1f; // ความเร็วในการขยับ Texture // Speed at which the Texture moves
    public float directionChangeInterval = 2f; // ระยะเวลาที่จะเปลี่ยนทิศทาง // Time interval to change direction

    private Vector2 offset; // ตัวแปรสำหรับจัดเก็บตำแหน่ง offset // Variable to store the offset position
    private Vector2 direction; // ตัวแปรสำหรับทิศทางการขยับ // Variable to store the movement direction

    private void Start()
    {
        if (targetMaterial != null)
        {
            GenerateTexture(0f, 0f); // สุ่ม Texture เมื่อตัวละครเริ่ม // Generate texture at the start
        }

        // ตั้งค่าเริ่มต้นสำหรับ offset และทิศทาง // Set initial offset and direction
        offset = new Vector2(0f, 0f);
        ChangeDirection(); // เรียกใช้ฟังก์ชันเพื่อเปลี่ยนทิศทาง // Call the function to change direction
    }

    private void Update()
    {
        // อัพเดท Texture ตามตำแหน่งของผู้เล่น // Update texture based on player's position
        offset += direction * speed * Time.deltaTime; // ปรับ offset ตามทิศทาง // Adjust offset based on direction
        GenerateTexture(offset.x, offset.y); // สุ่ม Texture ใหม่ทุก frame // Generate new texture every frame

        // เปลี่ยนทิศทางตามระยะเวลาที่กำหนด // Change direction based on the specified interval
        if (Time.time % directionChangeInterval < speed * Time.deltaTime)
        {
            ChangeDirection(); // เปลี่ยนทิศทาง // Change direction
        }
    }

    private void GenerateTexture(float offsetX, float offsetY)
    {
        Texture2D texture = new Texture2D(textureWidth, textureHeight); // สร้าง Texture ใหม่ // Create a new Texture

        for (int x = 0; x < textureWidth; x++)
        {
            for (int y = 0; y < textureHeight; y++)
            {
                float xCoord = ((float)x / textureWidth + offsetX) * scale; // ใช้ offset สำหรับขยับ // Use offset to shift
                float yCoord = ((float)y / textureHeight + offsetY) * scale; // ใช้ offset สำหรับขยับ // Use offset to shift

                float sample = Mathf.PerlinNoise(xCoord, yCoord); // สุ่มค่า Perlin noise // Sample Perlin noise value
                Color color = new Color(sample, sample, sample); // สร้างสีจากค่า Perlin noise // Create color from Perlin noise value
                texture.SetPixel(x, y, color); // ตั้งค่า Pixel ของ Texture // Set the Texture's Pixel
            }
        }

        texture.Apply(); // อัพเดท Texture // Apply the Texture
        targetMaterial.mainTexture = texture; // ตั้งค่า Texture ให้กับ Material // Assign the Texture to the Material
    }

    private void ChangeDirection()
    {
        // เปลี่ยนทิศทางสุ่ม // Randomly change direction
        direction = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized; // สุ่มทิศทางใหม่ // Generate new random direction
    }
}
