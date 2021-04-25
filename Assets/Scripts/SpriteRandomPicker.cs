using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteRandomPicker : MonoBehaviour
{
    [SerializeField] SpriteRenderer spriteRenderer;
    [SerializeField] Sprite[] sprites;

    void Start()
    {
        if (sprites.Length > 0) spriteRenderer.sprite = sprites[Random.Range(0, sprites.Length - 1)];
        else Debug.LogError("No sprites");
    }
}
