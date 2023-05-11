using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Delegates.Actors.EnemyNS;
using Actors.EnemyNS;

namespace Controllers.Utility.Statistics
{
    public class EnemySpawnerTrackerController : MonoBehaviour
    {
        // 0 = most recent, 11 = last
        int[] _enemiesSpawned = new int[11] { 0, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1 };
        int[] _trapsSpawned = new int[11] { 0, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1 };
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
            EnemySpawningDelegates.onEnemySpawn += IncrementEnemySpawn;
            EnemySpawningDelegates.onTrapSpawn += IncrementTrapSpawn;
        }

        void RemoveDelegates()
        {
            EnemySpawningDelegates.onEnemySpawn -= IncrementEnemySpawn;
            EnemySpawningDelegates.onTrapSpawn -= IncrementTrapSpawn;
        }


        public void OnZoneCompletion()
        {
            if (_enemiesSpawned[0] == -1) _enemiesSpawned[0] = 0;
            _enemiesSpawned = ShiftRight(_enemiesSpawned);

            if (_trapsSpawned[0] == -1) _trapsSpawned[0] = 0;
            _trapsSpawned = ShiftRight(_trapsSpawned);

        }

        public void ClearStats()
        {
            _enemiesSpawned = new int[11] { 0, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1 };
            _trapsSpawned = new int[11] { 0, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1 };
        }


        // Get all the stats from the previous (upto) 10 stages
        public float GetEnemiesSpawnedAverage()
        {
            float avg = 0;

            for (int i = 1; i <= 10; i++)
            {
                if (_enemiesSpawned[i] == -1)
                {
                    if (i > 1)
                    {
                        avg /= i - 1;
                    }
                    return avg;
                }

                avg += _enemiesSpawned[i];
            }

            avg /= 10;
            return avg;
        }

        // Get all the stats from the previous (upto) 10 stages
        public float GetTrapsSpawnedAverage()
        {
            float avg = 0;

            for (int i = 1; i <= 10; i++)
            {
                if (_trapsSpawned[i] == -1)
                {
                    if (i > 1)
                    {
                        avg /= i - 1;
                    }
                    return avg;
                }

                avg += _trapsSpawned[i];
            }

            avg /= 10;
            return avg;
        }


        // Add 1 every enemy spawn
        void IncrementEnemySpawn(Enemy e)
        {
            _enemiesSpawned[0]++;
        }

        // Add 1 trap enemy spawn
        void IncrementTrapSpawn()
        {
            _trapsSpawned[0]++;
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

        public float[] ShiftRightFloat(float[] arr)
        {
            float[] demo = new float[arr.Length];

            for (int i = 1; i < arr.Length; i++)
            {
                demo[i] = arr[i - 1];
            }

            demo[0] = 0;

            return demo;
        }

        // Used for statistics which do not update continuously
        // For statistics which update at the end of a zone
        public float[] ShiftRightFloatAlt(float[] arr)
        {
            float[] demo = new float[arr.Length];

            for (int i = 1; i < arr.Length; i++)
            {
                demo[i] = arr[i - 1];
            }

            demo[0] = 0;

            return demo;
        }
    }
}