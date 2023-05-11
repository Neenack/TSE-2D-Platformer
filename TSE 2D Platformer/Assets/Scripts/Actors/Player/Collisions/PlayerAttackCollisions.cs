using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Actors.EnemyNS;

using Delegates.Actors.Player;
using Delegates.Actors.EnemyNS;



namespace Actors.Player.Collisions
{
    public class PlayerAttackCollisions : MonoBehaviour
    {
        List<Enemy> enemiesInRange;
        List<ItemVaseController> vasesInRange;

        public void BeginSelf()
        {
            enemiesInRange = new List<Enemy>();
            vasesInRange = new List<ItemVaseController>();

            SetupDelegates();
        }

        void OnDisable()
        {
            RemoveDelegates();
        }

        void SetupDelegates()
        {
            EnemyStatsDelegates.onEnemyDeath += RemoveFromInRange;
            PlayerActionsDelegates.onVaseInRange += RemoveVaseFromInRange;
        }

        void RemoveDelegates()
        {
            EnemyStatsDelegates.onEnemyDeath -= RemoveFromInRange;
            PlayerActionsDelegates.onVaseInRange -= RemoveVaseFromInRange;
        }


        void OnTriggerEnter2D(Collider2D col)
        {
            if(col.tag == "Player") return;

            if (col.GetComponent<Enemy>() != null)
            {
                enemiesInRange.Add(col.GetComponent<Enemy>());
                return;
            }  

            if (col.GetComponent<ItemVaseController>() != null)
            {
                vasesInRange.Add(col.GetComponent<ItemVaseController>());
                return;
            }    
        }

        void OnTriggerExit2D(Collider2D col)
        {
            if(col.tag == "Player") return;

            if (col.GetComponent<Enemy>() != null && enemiesInRange.Contains(col.GetComponent<Enemy>()))
            {
                enemiesInRange.Remove(col.GetComponent<Enemy>());
                return;
            }

            if (col.GetComponent<ItemVaseController>() != null && vasesInRange.Contains(col.GetComponent<ItemVaseController>()))
            {
                vasesInRange.Remove(col.GetComponent<ItemVaseController>());
                return;
            }
        }


        public List<Enemy> GetEnemiesList()
        {
            return enemiesInRange;
        }

        public List<ItemVaseController> GetVasesList()
        {
            return vasesInRange;
        }


        void RemoveFromInRange(Enemy e)
        {
            if(enemiesInRange.Contains(e))
            {
                enemiesInRange.Remove(e);
            }
        }

        void RemoveVaseFromInRange(ItemVaseController v)
        {
            if(vasesInRange.Contains(v))
            {
                vasesInRange.Remove(v);
            }
        }


        public void ClearInRange()
        {
            enemiesInRange.Clear();
        }
    }
}