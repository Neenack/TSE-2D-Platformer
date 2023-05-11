using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Invoke("Die", 2f);
    }

    // Update is called once per frame
    void Die()
    {
        Destroy(this.gameObject);
    }
}
