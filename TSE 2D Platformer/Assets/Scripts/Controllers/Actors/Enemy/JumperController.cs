using Actors.Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using static UnityEditor.PlayerSettings;

public class JumperController : MonoBehaviour
{
    public float maxJumpForce, minJumpForce;
    public float jumpInterval = 2f;
    public LayerMask blockLayer;
    public Transform groundCheck;
    public bool grounded;
    public GameObject player;

    private Rigidbody2D rb;
    private bool isJumping = false;
    private float jumpTimer = 0f;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (!isJumping)
        {
            // If the jump timer has expired, jump in a random direction
            if (jumpTimer <= 0f)
            {
                float randomDirection = Random.Range(-3f, 3f);

                //If next to a wall, go the opposite direction
                if (Physics2D.OverlapCircle(transform.position + Vector3.right, 0.1f, blockLayer) != null) randomDirection = Random.Range(-3f, 0);
                if (Physics2D.OverlapCircle(transform.position - Vector3.right, 0.1f, blockLayer) != null) randomDirection = Random.Range(0, 3f);

                rb.velocity = new Vector2(randomDirection, Random.Range(minJumpForce, maxJumpForce));
                isJumping = true;
                jumpTimer = jumpInterval;
            }
            else
            {
                jumpTimer -= Time.deltaTime;
            }
        }

        if (onGround())
        {
            grounded = true;
            isJumping = false;
        }
        else grounded = false;
    }

    private bool onGround()
    {
        return Physics2D.OverlapBox(groundCheck.position, new Vector2(0.757864f, 0.1f), 0);
    }
}
