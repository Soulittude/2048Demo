using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private int width = 4;
    [SerializeField] private int height = 4;
    [SerializeField] private Node nodePrefab;
    [SerializeField] private SpriteRenderer boardPrefab;

    void Start()
    {
        GenerateGrid();
    }

    void GenerateGrid()
    {
        for(int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                var node = Instantiate(nodePrefab, new Vector2(x,y), Quaternion.identity);
            }
        }

        var center = new Vector2((float)width/2 - 0.5f, (float)height/2 - 0.5f);

        var board = Instantiate(boardPrefab, center, Quaternion.identity);

    }
}
