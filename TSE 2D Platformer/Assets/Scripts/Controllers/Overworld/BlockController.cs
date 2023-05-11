using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockController : MonoBehaviour
{
    public Sprite[] blocks;

    // Start is called before the first frame update
    void Start()
    {
        int randSprite = Random.Range(0, blocks.Length);

        GetComponent<SpriteRenderer>().sprite = blocks[randSprite];
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
