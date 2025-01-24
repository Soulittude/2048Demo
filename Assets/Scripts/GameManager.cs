using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random; //GOLDEN LINE

public class GameManager : MonoBehaviour
{
    [SerializeField] private int width = 4;
    [SerializeField] private int height = 4;

    [SerializeField] private Node nodePrefab;
    [SerializeField] private Block blockPrefab;
    [SerializeField] private SpriteRenderer boardPrefab;
    [SerializeField] private List<BlockType> types;

    private List<Node> nodes;
    private List<Block> blocks;

    private BlockType GetBlockTypeByValue(int val) => types.First(t => t.Value == val);

    void Start()
    {
        GenerateGrid();
    }

    void GenerateGrid()
    {
        nodes = new List<Node>();
        blocks = new List<Block>();
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                var node = Instantiate(nodePrefab, new Vector2(x, y), Quaternion.identity);
                nodes.Add(node);
            }
        }

        var center = new Vector2((float)width / 2 - 0.5f, (float)height / 2 - 0.5f);

        var board = Instantiate(boardPrefab, center, Quaternion.identity);
        board.size = new Vector2(width, height);

        Camera.main.transform.position = new Vector3(center.x, center.y, -10);

        SpawnBlocks(2);
    }

    void SpawnBlocks(int amount)
    {
        var freeNodes = nodes.Where(n => n.occupiedBlock == null).OrderBy(b => Random.value).ToList();

        foreach (var node in freeNodes.Take(amount))
        {
            var block = Instantiate(blockPrefab, node.pos, Quaternion.identity);
            block.Init(GetBlockTypeByValue(Random.value > 0.8f ? 4 : 2));
            //blocks.Add(block);
        }

        if (freeNodes.Count() == 1)
        {
            //Lost game
            return;
        }
    }

}

[Serializable]
public struct BlockType
{
    public int Value;
    public Color Color;
}
