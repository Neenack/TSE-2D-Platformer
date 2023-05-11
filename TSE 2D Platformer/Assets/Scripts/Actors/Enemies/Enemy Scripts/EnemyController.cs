using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    private GameObject levelGen;
    public LayerMask blockLayer;
    public float fallSpeed;
    public int type; //0 - Ground   1 - Roof

    // Start is called before the first frame update
    void Start()
    {
        levelGen = GameObject.FindGameObjectWithTag("LevelGenerator");
        type = Random.Range(0, 2);
    }

    // Update is called once per frame
    void Update()
    {
        CollisionCheck();
        Move();
    }

    private void Move() //Moves the enemy
    {
        Collider2D right = Physics2D.OverlapCircle(new Vector2(transform.position.x + 1, transform.position.y), 0.1f, blockLayer);
        Collider2D left = Physics2D.OverlapCircle(new Vector2(transform.position.x - 1, transform.position.y), 0.1f, blockLayer);
    }

    private void CollisionCheck() //Handles gravity and getting unstuck from blocks
    {
        if (levelGen.GetComponent<LevelGeneration>()._levelFinished == true)
        {
            Collider2D insideBlock = Physics2D.OverlapCircle(transform.position, 0.1f, blockLayer);
            if (insideBlock != null) //If inside a block
            {
                transform.position = new Vector2(transform.position.x, transform.position.y - 1);
            }
            else
            {
                if (type == 0)
                {
                    Collider2D onGround = Physics2D.OverlapCircle(new Vector2(transform.position.x, transform.position.y - 0.5f), 0.1f, blockLayer);
                    if (onGround == null) //If enemy is on the ground
                    {
                        transform.position = new Vector2(transform.position.x, transform.position.y - fallSpeed);
                    }
                }

                if (type == 1)
                {
                    Collider2D onGround = Physics2D.OverlapCircle(new Vector2(transform.position.x, transform.position.y + 0.5f), 0.1f, blockLayer);
                    if (onGround == null) //If enemy is on the ground
                    {
                        transform.position = new Vector2(transform.position.x, transform.position.y + fallSpeed);
                    }
                }
            }
        }
    }
}
