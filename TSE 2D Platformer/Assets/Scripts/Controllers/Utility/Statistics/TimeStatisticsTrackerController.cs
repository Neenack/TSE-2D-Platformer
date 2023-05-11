using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Delegates.Utility;



namespace Controllers.Utility.Statistics
{
    public class TimeStatisticsTrackerController : MonoBehaviour
    {
        // 0 = most recent, 11 = last
        float[] _levelCompletionTime = new float[11] {0, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1};

        Coroutine _timer = null;

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
            ZoneDelegates.onZoneGenerationFinish += OnZoneStart;
        }

        void RemoveDelegates()
        {
            ZoneDelegates.onZoneGenerationFinish -= OnZoneStart;
        }

        public void OnZoneStart()
        {
            TimerControl(true);
        }


        public void OnZoneCompletion()
        {
            TimerControl(false);

            if(_timer != null)
            {   

                if(_levelCompletionTime[0] == -1) _levelCompletionTime[0] = 0;
                _levelCompletionTime = ShiftRightFloat(_levelCompletionTime);
            }   
        }

        public void ClearStats()
        {
            _levelCompletionTime = new float[11] { 0, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1 };
        }


        // Get all the stats from the previous (upto) 10 stages
        public float GetZoneTimeAverage()
        {
            float avg = 0;

            for(int i = 1; i <= 10; i++)
            {
                if(_levelCompletionTime[i] == -1)
                {
                    if(i > 1)
                    {
                        avg /= i - 1;
                    }
                    return avg;
                }

                avg += _levelCompletionTime[i];
            }

            avg /= 10;
            return avg;
        }


        void TimerControl(bool start)
        {
            if(start)
            {
                _timer = StartCoroutine(ZoneTimer());
            }
            else
            {
                if(_timer != null)
                {
                    StopCoroutine(_timer);
                }
            }
        }
        IEnumerator ZoneTimer()
        {
            while(1 == 1)
            {
                _levelCompletionTime[0] += Time.deltaTime;
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