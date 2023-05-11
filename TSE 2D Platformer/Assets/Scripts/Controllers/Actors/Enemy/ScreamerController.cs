using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class ScreamerController : MonoBehaviour
{
    public float screamInterval;
    public GameObject screamerBolt;
    public int range;

    public LayerMask blockLayer;
    private GameObject player;

    private float screamTimer = 0f;

    void Start()
    {
        int difficulty = GameObject.Find("Level Generator").GetComponent<LevelGeneration>().difficulty - 8;

        screamInterval = 3f - ((float)difficulty * 0.75f);
    }

    void Update()
    {
        player = GameObject.FindGameObjectWithTag("Player");


        if (player != null)
        {
            float distToPlayer = Vector3.Distance(transform.position, player.transform.position);

            if (distToPlayer <= range)
            {
                Vector3 direction = (player.transform.position - transform.position).normalized;

                // If the jump timer has expired, jump in a random direction
                if (screamTimer <= 0f)
                {
                    //Scream
                    GameObject newBolt = Instantiate(screamerBolt, transform.position, Quaternion.identity);
                    newBolt.transform.SetParent(GameObject.Find("BoltHolder").transform, true);
                    newBolt.GetComponent<ScreamerBoltController>().SetDirection(direction);
                    screamTimer = screamInterval;
                }
                else
                {
                    screamTimer -= Time.deltaTime;
                }

                transform.localScale = (player.transform.position.x < transform.position.x ? new Vector3(1, 1, 1) : new Vector3(-1, 1, 1));

                RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position, direction);

                bool hitGround = false;
                foreach (RaycastHit2D hit in hits) { if (hit.collider.tag == "Walkable") hitGround = true; }
                if (hitGround == false) Debug.Log("HIT");
            }
        }
    }
}
