using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoamingObject : MonoBehaviour
{
    public float speed = 2.0f;
    public float raycastDistance = 0.5f;
    public LayerMask wallLayer;

    public bool facingRight = true;


    void Update()
    {
        // Calculate the new x position of the object based on its current direction and speed
        float newX = transform.position.x + (facingRight ? speed : -speed) * Time.deltaTime;
        transform.localScale = (facingRight ? new Vector3(1, 1, 1) : new Vector3(-1, 1, 1));

        // Cast a ray in the direction the object is moving to detect walls
        Vector2 raycastOrigin = facingRight ? transform.position + Vector3.right * 0.5f : transform.position - Vector3.right * 0.5f;
        RaycastHit2D hit = Physics2D.Raycast(raycastOrigin, Vector2.right * (facingRight ? 1 : -1), raycastDistance, wallLayer);

        Vector2 edgeOrigin = facingRight ? transform.position + Vector3.right * 0.5f + Vector3.down * 0.5f : transform.position - Vector3.right * 0.5f + Vector3.down * 0.5f;
        RaycastHit2D edgeHit = Physics2D.Raycast(edgeOrigin, Vector2.down, raycastDistance, wallLayer);

        // If the object has collided with a wall, turn it around
        if (hit.collider != null || edgeHit.collider == null)
        {
            facingRight = !facingRight;
        }


        // Set the object's new position
        transform.position = new Vector3(newX, transform.position.y, transform.position.z);
    }
}
