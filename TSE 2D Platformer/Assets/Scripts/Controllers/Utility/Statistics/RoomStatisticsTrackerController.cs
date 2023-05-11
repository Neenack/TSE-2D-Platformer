using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Controllers.Overworld.Rooms;

using Delegates.Utility;



namespace Controllers.Utility.Statistics
{
    public class RoomStatisticsTrackerController : MonoBehaviour
    {
        // 0 = most recent, 11 = last
        int[] _roomsExplored = new int[11] {0, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1};
        float[] _longestTimeInRoom = new float[11] {-2, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1};

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
            StatisticsTrackingDelegates.onRoomExplored += IncrementRoomsExplored;
        }

        void RemoveDelegates()
        {
            StatisticsTrackingDelegates.onRoomExplored -= IncrementRoomsExplored;
        }


        public void OnZoneCompletion()
        {
            if(_roomsExplored[0] == -1) _roomsExplored[0] = 0;
            _roomsExplored = ShiftRight(_roomsExplored);

            SetLongestTimeInRoom();
            if(_longestTimeInRoom[0] == -2) _longestTimeInRoom[0] = 0;
            _longestTimeInRoom = ShiftRightFloat(_longestTimeInRoom);
        }

        public void ClearStats()
        {
            _roomsExplored = new int[11] { 0, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1 };
            _longestTimeInRoom = new float[11] { -2, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1 };
        }


        // Get all the stats from the previous (upto) 10 stages
        public float GetRoomsExploredAverage()
        {
            float avg = 0;

            for(int i = 1; i <= 10; i++)
            {
                if(_roomsExplored[i] == -1)
                {
                    if(i > 1)
                    {
                        avg /= i - 1;
                    }
                    return avg;
                }

                avg += _roomsExplored[i];
            }

            avg /= 10;
            return avg;
        }

        // Get all the stats from the previous (upto) 10 stages
        public float GetLongestRoomTimeAverage()
        {
            float avg = 0;

            for (int i = 1; i <= 10; i++)
            {
                if (_longestTimeInRoom[i] == -1)
                {
                    if (i > 1)
                    {
                        avg /= i - 1;
                    }
                    return avg;
                }

                avg += _longestTimeInRoom[i];
            }

            avg /= 10;
            return avg;
        }

        // Add 1 every enemy kill
        // Add 2 first time due to -1 being the check amount
        void IncrementRoomsExplored()
        {
            _roomsExplored[0]++;
        }

        void SetLongestTimeInRoom()
        {
            float max = 0;
            foreach(Transform room in GameObject.Find("RoomsHolder").transform)
            {
                if(room.GetComponent<RoomCollisionController>().GetTimeInRoom() > max)
                {
                    max = room.GetComponent<RoomCollisionController>().GetTimeInRoom();
                }
            }

            _longestTimeInRoom[0] = max;
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