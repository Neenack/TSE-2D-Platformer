using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class RandomBGTIle : MonoBehaviour
{

    public Sprite[] tiles;
    public SpriteRenderer spriteRenderer;

    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer.sprite = tiles[Random.Range(0, tiles.Length - 1)];
    }
}
