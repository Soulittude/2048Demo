using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Block : MonoBehaviour
{
    public int _value;
    public Node _node;
    public Block mergingBlock;
    public bool merging;

    public Vector2 pos => transform.position;

    [SerializeField] private SpriteRenderer sRenderer;
    [SerializeField] private TextMeshPro _text;

    public void Init(BlockType type)
    {
        _value = type.Value;
        sRenderer.color = type.Color;
        _text.text = type.Value.ToString();
    }

    public void SetBlock(Node node)
    {
        if (_node != null)
            _node.occupiedBlock = null;

        _node = node;
        _node.occupiedBlock = this;
    }

    public void MergeBlock(Block blockToMergeWith)
    {
        mergingBlock = blockToMergeWith;

        _node.occupiedBlock = null;

        blockToMergeWith.merging = true;
    }

    public bool CanMerge(int val) => val == _value && !merging && mergingBlock == null;
}
