using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Actors.EnemyNS;

using Delegates.Utility;
using Delegates.Actors.EnemyNS;



namespace Controllers.Utility.Statistics
{
    public class CombatTrackerController : MonoBehaviour
    {
        // 0 = most recent, 11 = last
        int[] _enemiesKilled = new int[11] {0, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1};
        int[] _nearMissesEnemy = new int[11] {0, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1};
        int[] _nearMissesProjectile = new int[11] {0, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1};

        int[] _bombKills = new int[11] {0, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1};

        public void BeginSelf()
        {
            SetupDelegates();
        }

        void OnDisable()
        {
            RemoveDelegates();
        }


        void SetupDelegates()
        {
            StatisticsTrackingDelegates.onEnemyDeathTracking += IncrementEnemiesKilled;
            StatisticsTrackingDelegates.onEnemyNearMiss += IncrementNearMissesEnemy;
            StatisticsTrackingDelegates.onProjectileNearMiss += IncrementNearMissesProjectile;
            StatisticsTrackingDelegates.onBombKill += IncrementBombKills;
        }

        void RemoveDelegates()
        {
            StatisticsTrackingDelegates.onEnemyDeathTracking -= IncrementEnemiesKilled;
            StatisticsTrackingDelegates.onEnemyNearMiss -= IncrementNearMissesEnemy;
            StatisticsTrackingDelegates.onProjectileNearMiss -= IncrementNearMissesProjectile;
            StatisticsTrackingDelegates.onBombKill -= IncrementBombKills;
        }


        public void OnZoneCompletion()
        {
            if(_enemiesKilled[0] == -1) _enemiesKilled[0] = 0;
            _enemiesKilled = ShiftRight(_enemiesKilled);

            if(_nearMissesEnemy[0] == -1) _nearMissesEnemy[0] = 0;
            _nearMissesEnemy = ShiftRight(_nearMissesEnemy);

            if(_nearMissesProjectile[0] == -1) _nearMissesProjectile[0] = 0;
            _nearMissesProjectile = ShiftRight(_nearMissesProjectile);

            if(_bombKills[0] == -1) _bombKills[0] = 0;
            _bombKills = ShiftRight(_bombKills);
        }

        public void ClearStats()
        {
            _enemiesKilled = new int[11] { 0, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1 };
            _nearMissesEnemy = new int[11] { 0, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1 };
            _nearMissesProjectile = new int[11] { 0, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1 };
            _bombKills = new int[11] { 0, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1 };
        }



        // Get all the stats from the previous (upto) 10 stages
        public float GetEnemiesKilledAverage()
        {
            float avg = 0;

            for(int i = 1; i <= 10; i++)
            {
                if(_enemiesKilled[i] == -1)
                {
                    if(i > 1)
                    {
                        avg /= i - 1;
                    }
                    return avg;
                }

                avg += _enemiesKilled[i];
            }

            avg /= 10;
            return avg;
        }

        public float GetNearMissesWithEnemyAverage()
        {
            float avg = 0;

            for(int i = 1; i <= 10; i++)
            {
                if(_nearMissesEnemy[i] == -1)
                {
                    if(i > 1)
                    {
                        avg /= i - 1;
                    }
                    return avg;
                }

                avg += _nearMissesEnemy[i];
            }

            avg /= 10;
            return avg;
        }

        public float GetNearMissesWithProjectileAverage()
        {
            float avg = 0;

            for(int i = 1; i <= 10; i++)
            {
                if(_nearMissesProjectile[i] == -1)
                {
                    if(i > 1)
                    {
                        avg /= i - 1;
                    }
                    return avg;
                }

                avg += _nearMissesProjectile[i];
            }

            avg /= 10;
            return avg;
        }

        public float GetBombKillsAverage()
        {
            float avg = 0;

            for(int i = 1; i <= 10; i++)
            {
                if(_bombKills[i] == -1)
                {
                    if(i > 1)
                    {
                        avg /= i - 1;
                    }
                    return avg;
                }

                avg += _bombKills[i];
            }

            avg /= 10;
            return avg;
        }

        // Add 1 every enemy kill
        // Add 2 first time due to -1 being the check amount
        void IncrementEnemiesKilled()
        {
            _enemiesKilled[0]++;
        }
        
        void IncrementNearMissesEnemy()
        {
            _nearMissesEnemy[0]++;
        }

        void IncrementNearMissesProjectile()
        {
            _nearMissesProjectile[0]++;
        }

        void IncrementBombKills()
        {
            _bombKills[0]++;
        }



        public int[] ShiftRight(int[] arr) 
        {
            int[] demo = new int[arr.Length];

            for (int i = 1; i < arr.Length; i++) 
            {
                demo[i] = arr[i - 1];
            }

            demo[0] = 0;

            return demo;
        }
    }
}