using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ValidPosition : MonoBehaviour
{
    public LayerMask blockLayer;

    void Update()
    {
        if (Physics2D.OverlapCircle(transform.position, 0.1f, blockLayer))
        {
            //Destroy(this.gameObject);
            transform.position += new Vector3(Random.Range(-1, 1), Random.Range(-1, 1));
        }
    }
}
