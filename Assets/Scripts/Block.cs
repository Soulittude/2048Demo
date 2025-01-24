using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Block : MonoBehaviour
{
    public int _value;
    public Vector2 pos => transform.position;
    public Node _node;

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
        if(_node != null)
            _node.occupiedBlock = null;

        _node = node;
        _node.occupiedBlock = this;
    }
}
