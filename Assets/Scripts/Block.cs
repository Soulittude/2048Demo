using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Block : MonoBehaviour
{
    public int value;
    [SerializeField] private SpriteRenderer sRenderer;
    [SerializeField] private TextMeshPro _text;
    public void Init(BlockType type)
    {
        value = type.Value;
        sRenderer.color = type.Color;
        _text.text = type.Value.ToString();
    }
}
