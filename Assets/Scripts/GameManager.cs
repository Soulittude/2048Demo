using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
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
    [SerializeField] private float travelTime = 0.2f;

    private List<Node> nodes;
    private List<Block> blocks;
    private GameState state;
    private int round;

    private BlockType GetBlockTypeByValue(int val) => types.First(t => t.Value == val);

    void Start()
    {
        ChangeState(GameState.GenerateLevel);
    }

    private void ChangeState(GameState newState)
    {
        state = newState;

        switch (newState)
        {
            case GameState.GenerateLevel:
                GenerateGrid();
                break;
            case GameState.SpawningBlocks:
                SpawnBlocks(round++ == 0 ? 2 : 1);
                break;
            case GameState.WaitingInputs:
                break;
            case GameState.Moving:
                break;
            case GameState.Win:
                break;
            case GameState.Lose:
                break;
        }
    }

    void Update()
    {
        if (state != GameState.WaitingInputs)
            return;

        if (Input.GetKeyDown(KeyCode.LeftArrow))
            ShiftBlocks(Vector2.left);

        if (Input.GetKeyDown(KeyCode.RightArrow))
            ShiftBlocks(Vector2.right);

        if (Input.GetKeyDown(KeyCode.DownArrow))
            ShiftBlocks(Vector2.down);

        if (Input.GetKeyDown(KeyCode.UpArrow))
            ShiftBlocks(Vector2.up);
    }

    void GenerateGrid()
    {
        round = 0;
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

        ChangeState(GameState.SpawningBlocks);
    }

    void SpawnBlocks(int amount)
    {
        var freeNodes = nodes.Where(n => n.occupiedBlock == null).OrderBy(b => Random.value).ToList();

        foreach (var node in freeNodes.Take(amount))
        {
            var block = Instantiate(blockPrefab, node.pos, Quaternion.identity);
            block.Init(GetBlockTypeByValue(Random.value > 0.8f ? 4 : 2));
            block.SetBlock(node);
            blocks.Add(block);
        }

        if (freeNodes.Count() == 1)
        {
            //Lost game
            return;
        }

        ChangeState(GameState.WaitingInputs);
    }

    void ShiftBlocks(Vector2 dir)
    {
        var orderedBlocks = blocks.OrderBy(b => b.pos.x).ThenBy(b => b.pos.y).ToList();

        if (dir == Vector2.right || dir == Vector2.up)
            orderedBlocks.Reverse();

        foreach (var block in orderedBlocks)
        {
            var next = block._node;
            do
            {
                block.SetBlock(next);

                var possibleNode = GetNodeAtPosition(next.pos + dir);
                if (possibleNode != null)
                {
                    if (possibleNode.occupiedBlock == null)
                        next = possibleNode;
                }

            } while (next != block._node);

            block.transform.DOMove(block._node.pos, travelTime);
        }


    }

    Node GetNodeAtPosition(Vector2 pos)
    {
        return nodes.FirstOrDefault(n => n.pos == pos);
    }

}

[Serializable]
public struct BlockType
{
    public int Value;
    public Color Color;
}

public enum GameState
{
    GenerateLevel,
    SpawningBlocks,
    WaitingInputs,
    Moving,
    Win,
    Lose
}