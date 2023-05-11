using System.Collections;
using System.Collections.Generic;
using UnityEngine;


using Actors.EnemyNS;

using Delegates.Utility;


namespace Actors.Player.Collisions
{
    public class PlayerDetectionCollisions : MonoBehaviour
    {
        void OnTriggerEnter2D(Collider2D col)
        {
            if(col.tag == "Player") return;

            if(col.gameObject.tag == "Enemy")
            {
                col.gameObject.GetComponent<Enemy>().SetDetected();
            }
        }

        void OnTriggerExit2D(Collider2D col)
        {
            if(col.tag == "Player") return;
        }
    }
}