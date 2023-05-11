using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Actors.EnemyNS;
using Delegates.Actors.EnemyNS;
using Delegates.Utility;



namespace Controllers.Actors.EnemyNS
{
    public class EnemiesController : MonoBehaviour
    {
        List<Enemy> enemies;

        public void BeginSelf()
        {
            enemies = new List<Enemy>();

            SetupDelegates();
        }

        void OnDisable()
        {
            RemoveDelegates();
        }

        void SetupDelegates()
        {
            EnemySpawningDelegates.onEnemySpawn += AddEnemy;
            EnemySpawningDelegates.onEnemyDespawn += EnemyDespawn;

            EnemyStatsDelegates.onEnemyHit += EnemyHit;
            EnemyStatsDelegates.onEnemyDeathCheck += CheckEnemyDeath;
            EnemyStatsDelegates.onEnemyDeath += EnemyDeath;

            ZoneDelegates.onZoneCompletion += DespawnAll;
        }

        void RemoveDelegates()
        {
            EnemySpawningDelegates.onEnemySpawn -= AddEnemy;
            EnemySpawningDelegates.onEnemyDespawn -= EnemyDespawn;

            EnemyStatsDelegates.onEnemyHit -= EnemyHit;
            EnemyStatsDelegates.onEnemyDeathCheck -= CheckEnemyDeath;
            EnemyStatsDelegates.onEnemyDeath -= EnemyDeath;

            ZoneDelegates.onZoneCompletion -= DespawnAll;
        }

        void AddEnemy(Enemy e)
        {
            enemies.Add(e);

            e.BeginSelf();
        }

        void EnemyDespawn(Enemy e)
        {
            enemies.Remove(e);

            Destroy(e.gameObject);
        }

        void DespawnAll()
        {
            foreach(Enemy e in enemies)
            {
                if (e != null)
                {
                    Destroy(e.gameObject);
                }
                
            }

            enemies.Clear();
        }


        void EnemyHit(Enemy e)
        {
            e.GetComponent<EnemyStats>().HpChange(1, false);
        }

        void CheckEnemyDeath(List<Enemy> Es)
        {
            for(int i = 0; i < enemies.Count; i++)
            {
                if(Es.Contains(enemies[i]))
                {
                    EnemyDeath(enemies[i]);
                    i++;
                }
            }
        }

        void EnemyDeath(Enemy e)
        {
            // Here is where tracking stats & any enemy kill rewards go, as well as animations
            if(StatisticsTrackingDelegates.onEnemyDeathTracking != null)
            {
                StatisticsTrackingDelegates.onEnemyDeathTracking();
            }


            enemies.Remove(e);

            Destroy(e.gameObject);
        }
    }
}