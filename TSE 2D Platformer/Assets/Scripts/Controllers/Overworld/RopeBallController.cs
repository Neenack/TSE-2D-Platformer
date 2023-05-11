using Actors.Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using static UnityEditor.PlayerSettings;

public class RopeBallController : MonoBehaviour
{
    public LayerMask blockLayer;
    public GameObject rope, ropeHolder;

    // Update is called once per frame
    void Update()
    {
        if (Physics2D.OverlapCircle(transform.position, 0.25f, blockLayer) != null)
        {
            //float ropeX = transform.position.x - (transform.position.x % 1);
            //float ropeY = transform.position.y - (transform.position.y % 1);

            float ropeX = Mathf.Round(transform.position.x);
            float ropeY = Mathf.Round(transform.position.y);
            GameObject newRope = Instantiate(rope, new Vector3(ropeX, ropeY, 0), Quaternion.identity);
            newRope.GetComponent<RopeController>().PlaySound();
            newRope.transform.SetParent(GameObject.Find("RopeHolder").transform, true);
            Destroy(this.gameObject);
        }

    }
}
