using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridGenerator : MonoBehaviour
{
    public int width = 10;  // �������ҧ�ͧ Grid
    public int height = 10; // �����٧�ͧ Grid
    public List<GameObject> tilePrefabs; // List ����Ѻ�� prefab ���µ��
    public GameObject playerPrefab; // Prefab ����Ѻ������
    public GameObject exitPrefab; // Prefab ����Ѻ�ҧ�͡
    public Vector2Int playerPosition = new Vector2Int(0, 0); // ���˹觼�����
    public Vector2Int exitPosition; // ���˹觷ҧ�͡
    public float tileSpacing = 1.1f; // ������ҧ�����ҧ���� Tile

    void Start()
    {
        exitPosition = new Vector2Int(width - 1, height - 1); // ��˹����˹觷ҧ�͡
        GenerateGrid();
        PlacePlayerAndExit();
    }

    void GenerateGrid()
    {
        // �ٻ����ӹǹ���ҧ����٧�ͧ Grid
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                // ��Ǩ�ͺ��ҵ��˹觹���繢ͧ���������ͷҧ�͡�������
                if ((x == playerPosition.x && y == playerPosition.y) || (x == exitPosition.x && y == exitPosition.y))
                {
                    continue; // ����������ҧ Tile 㹵��˹觢ͧ���������ͷҧ�͡
                }

                // ���͡ prefab Ẻ�����ҡ List
                GameObject randomPrefab = tilePrefabs[Random.Range(0, tilePrefabs.Count)];

                // �ӹǳ���˹觷����ҧ prefab
                Vector3 position = new Vector3(x * tileSpacing, 0, y * tileSpacing);

                // ���ҧ Tile �ҡ prefab Ẻ����㹵��˹觷��ӹǳ��
                Instantiate(randomPrefab, position, Quaternion.identity);
            }
        }
    }

    void PlacePlayerAndExit()
    {
        // ���ҧ�����蹷����˹��������
        Instantiate(playerPrefab, new Vector3(playerPosition.x * tileSpacing, 0, playerPosition.y * tileSpacing), Quaternion.identity);

        // ���ҧ�ҧ�͡�����˹觻���
        Instantiate(exitPrefab, new Vector3(exitPosition.x * tileSpacing, 0, exitPosition.y * tileSpacing), Quaternion.identity);
    }
}
