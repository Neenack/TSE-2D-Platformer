using System.Collections;
using System.Collections.Generic;
using UnityEngine;


using Delegates.Utility;



namespace Actors.EnemyNS
{
    public class EnemyNearMissCollisions : MonoBehaviour
    {
        bool _wait;
        float _waitTime = 1;

        void OnTriggerExit2D(Collider2D col)
        {
            if(col.gameObject.tag == "Player" && !_wait)
            {
                if(StatisticsTrackingDelegates.onEnemyNearMiss != null)
                {
                    StatisticsTrackingDelegates.onEnemyNearMiss();
                }

                _wait = true;
                StartCoroutine(Wait());
            }
        }

        IEnumerator Wait()
        {
            float timeWaited = 0;

            while(_wait)
            {
                timeWaited += Time.deltaTime;

                if(timeWaited > _waitTime)
                {
                    _wait = false;
                }

                yield return null;
            }
        }
    }
}