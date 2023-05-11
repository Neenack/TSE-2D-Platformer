using System.Collections;
using System.Collections.Generic;
using UnityEngine;


using Actors.EnemyNS;
using Delegates.Actors.EnemyNS;
using Delegates.Utility;


public class BombController : MonoBehaviour
{
    public Sprite redBomb;
    public SpriteRenderer sr;
    public GameObject explosion;

    public float explosionRadius = 3f;
    public float explosionTimer;
    bool hasExploded = false;

    // Start is called before the first frame update
    void Start()
    {
        Invoke("Red", explosionTimer - 1);
        Invoke("Explode", explosionTimer);
    }

    void Red()
    {
        sr.sprite = redBomb;
    }

    // Update is called once per frame
    void Explode()
    {
        if (!hasExploded)
        {
            hasExploded = true;

            GameObject newExplosion = Instantiate(explosion, transform.position, Quaternion.identity);


            List<Enemy> enemiesHit = new List<Enemy>();

            // Destroy blocks within explosion radius
            Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, explosionRadius);
            foreach (Collider2D collider in colliders)
            {
                if (collider.gameObject.layer != LayerMask.NameToLayer("Background") && collider.gameObject.tag != "Player" && collider.gameObject.tag != "PlayerChildObjectTag" 
                 && collider.gameObject.layer != LayerMask.NameToLayer("Room") && collider.gameObject.tag != "Door" && collider.gameObject.tag != "Enemy")
                {
                    Destroy(collider.gameObject);
                }

                else if(collider.gameObject.tag == "Enemy")
                {
                    
                    enemiesHit.Add(collider.gameObject.GetComponent<Enemy>());
                }
            }

             foreach(Enemy e in enemiesHit)
            {
                if (StatisticsTrackingDelegates.onBombKill != null)
                {
                    StatisticsTrackingDelegates.onBombKill();
                }


                if(EnemyStatsDelegates.onEnemyHit != null)
                {
                    EnemyStatsDelegates.onEnemyHit(e);
                }
            }

            if (EnemyStatsDelegates.onEnemyDeathCheck != null) EnemyStatsDelegates.onEnemyDeathCheck(enemiesHit);

            // Destroy bomb object
            Destroy(gameObject);
        }
    }
}
