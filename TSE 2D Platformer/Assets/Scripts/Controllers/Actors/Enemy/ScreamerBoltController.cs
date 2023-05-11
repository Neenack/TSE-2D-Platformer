using System.Collections;
using System.Collections.Generic;
//using UnityEditor.Experimental.GraphView;
using UnityEngine;
//using static UnityEditor.PlayerSettings;
using static UnityEngine.GraphicsBuffer;

public class ScreamerBoltController : MonoBehaviour
{
    public float speed = 0.5f; // The speed at which the bullet moves
    public LayerMask blockLayer;

    private Vector3 direction;

    void Start()
    {

    }

    public void SetDirection(Vector3 dir)
    {
        direction = dir;
        transform.right = -dir;
    }

    public void Update()
    {
        if (Physics2D.OverlapCircle(transform.position, 0.1f, blockLayer) != null) Destroy(this.gameObject);
    }

    private void FixedUpdate()
    {
        transform.position += direction * speed;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        // Check if the bullet has collided with an object on the "blockLayer"
        if (collision.gameObject.layer == blockLayer)
        {
            // Destroy the bullet
            Destroy(gameObject);
        }
    }
}
