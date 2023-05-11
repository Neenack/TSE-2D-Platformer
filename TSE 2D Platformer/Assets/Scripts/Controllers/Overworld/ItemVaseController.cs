using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemVaseController : MonoBehaviour
{
    public LayerMask blockLayer;

    void Update()
    {
        if (Physics2D.OverlapCircle(transform.position - new Vector3(0,1,0), 0.1f, blockLayer) == null)
        {
            transform.position -= new Vector3(0,1,0);
        }
    }

    void OnDestroy()
    {
        int rand = Random.Range(1, 3);
        
        if (rand == 1)
        {
            //Give player bomb
        }
        if (rand == 2)
        {
            //Give player rope
        }
    }
}
