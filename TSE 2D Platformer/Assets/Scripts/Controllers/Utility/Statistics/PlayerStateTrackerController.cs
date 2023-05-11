using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Actors.EnemyNS;

using Delegates.Actors.Player;
using Delegates.Utility;



namespace Controllers.Utility.Statistics
{
    public class PlayerStateTrackerController : MonoBehaviour
    {
        // 0 = most recent, 11 = last
        float[] _idleTime = new float[11] {0, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1};

        int[] _enemiesDetected = new int[11] {0, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1};

        int[] _deathToAngryBob = new int[11] {0, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1};
        int[] _deathToJumper = new int[11] {0, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1};
        int[] _deathToScreamer = new int[11] {0, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1};

        
        int[] _deathToTrap =  new int[11] {0, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1};

        Coroutine _idleTimeCoroutine = null;

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
            StatisticsTrackingDelegates.onIdleTracking += DoIdleTracking;
            StatisticsTrackingDelegates.onEnemyDetected += IncrementEnemiesDetected;
            StatisticsTrackingDelegates.onPlayerHitByEnemy += IncrementKilledByEnemy;
            StatisticsTrackingDelegates.onPlayerHitByTrap += IncrementDeathByTrap;
        }

        void RemoveDelegates()
        {
            StatisticsTrackingDelegates.onIdleTracking -= DoIdleTracking;
            StatisticsTrackingDelegates.onEnemyDetected -= IncrementEnemiesDetected;
            StatisticsTrackingDelegates.onPlayerHitByEnemy -= IncrementKilledByEnemy;
            StatisticsTrackingDelegates.onPlayerHitByTrap -= IncrementDeathByTrap;
        }


        public void OnZoneCompletion()
        {
            if(_idleTime[0] == -1) _idleTime[0] = 0;
            _idleTime = ShiftRightFloat(_idleTime);

            if(_enemiesDetected[0] == -1) _enemiesDetected[0] = 0;
            _enemiesDetected = ShiftRight(_enemiesDetected);

            if(_deathToAngryBob[0] == -1) _deathToAngryBob[0] = 0;
            _deathToAngryBob = ShiftRight(_deathToAngryBob);

            if(_deathToJumper[0] == -1) _deathToJumper[0] = 0;
            _deathToJumper = ShiftRight(_deathToJumper);

            if(_deathToScreamer[0] == -1) _deathToScreamer[0] = 0;
            _deathToScreamer = ShiftRight(_deathToScreamer);

            if(_deathToTrap[0] == -1) _deathToTrap[0] = 0;
            _deathToTrap = ShiftRight(_deathToTrap);
        }

        public void ClearStats()
        {
            _idleTime = new float[11] { 0, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1 };
            _enemiesDetected = new int[11] { 0, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1 };
            _deathToAngryBob = new int[11] { 0, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1 };
            _deathToJumper = new int[11] { 0, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1 };
            _deathToScreamer = new int[11] { 0, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1 };
            _deathToTrap = new int[11] { 0, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1 };
        }


        // Get all the stats from the previous (upto) 10 stages
        public float GetIdleTimeAverage()
        {
            float avg = 0;

            for(int i = 1; i <= 10; i++)
            {
                if(_idleTime[i] == -1)
                {
                    if(i > 1)
                    {
                        avg /= i - 1;
                    }
                    return avg;
                }

                avg += _idleTime[i];
            }

            avg /= 10;
            return avg;
        }

        public float GetEnemiesDetectedAverage()
        {
            float avg = 0;

            for(int i = 1; i <= 10; i++)
            {
                if(_enemiesDetected[i] == -1)
                {
                    if(i > 1)
                    {
                        avg /= i - 1;
                    }
                    return avg;
                }

                avg += _enemiesDetected[i];
            }

            avg /= 10;
            return avg;
        }

        public float GetDeathToAngryBobAverage()
        {
            float avg = 0;

            for(int i = 1; i <= 10; i++)
            {
                if(_deathToAngryBob[i] == -1)
                {
                    if(i > 1)
                    {
                        avg /= i - 1;
                    }
                    return avg;
                }

                avg += _deathToAngryBob[i];
            }

            avg /= 10;
            return avg;
        }

        public float GetDeathToJumperAverage()
        {
            float avg = 0;

            for(int i = 1; i <= 10; i++)
            {
                if(_deathToJumper[i] == -1)
                {
                    if(i > 1)
                    {
                        avg /= i - 1;
                    }
                    return avg;
                }

                avg += _deathToJumper[i];
            }

            avg /= 10;
            return avg;
        }

        public float GetDeathToScreamerAverage()
        {
            float avg = 0;

            for(int i = 1; i <= 10; i++)
            {
                if(_deathToScreamer[i] == -1)
                {
                    if(i > 1)
                    {
                        avg /= i - 1;
                    }
                    return avg;
                }

                avg += _deathToScreamer[i];
            }

            avg /= 10;
            return avg;
        }

        public float GetDeathToTrap()
        {
            float avg = 0;

            for(int i = 1; i <= 10; i++)
            {
                if(_deathToTrap[i] == -1)
                {
                    if(i > 1)
                    {
                        avg /= i - 1;
                    }
                    return avg;
                }

                avg += _deathToTrap[i];
            }

            avg /= 10;
            return avg;
        }


        // Add 1 every enemy kill
        // Add 2 first time due to -1 being the check amount
        void DoIdleTracking(bool isIdle)
        {
            if(isIdle)
            {
                _idleTimeCoroutine =  StartCoroutine(IdleTimeIncrementor());
            }

            else
            {
                if(_idleTimeCoroutine != null)
                {
                    StopCoroutine(_idleTimeCoroutine);
                }
            }
        }

        void IncrementEnemiesDetected()
        {
            _enemiesDetected[0]++;
        }

        void IncrementKilledByEnemy(EnemyType eT)
        {
            if(eT == EnemyType.AngryBob)
            {
                _deathToAngryBob[0]++;
            }

            else if(eT == EnemyType.Jumper)
            {
                _deathToJumper[0]++;
            }

            else if(eT == EnemyType.Screamer)
            {
                _deathToScreamer[0]++;
            }
        }

        void IncrementDeathByTrap()
        {
            _deathToTrap[0]++;
        }


        IEnumerator IdleTimeIncrementor()
        {
            while(1 == 1)
            {
                _idleTime[0] += Time.deltaTime;
                yield return null;
            }
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
    }
}