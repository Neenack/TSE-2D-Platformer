using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Delegates.Utility;



namespace Controllers.Utility.Statistics
{
    public class ActionsTrackerController : MonoBehaviour
    {
       // 0 = most recent, 11 = last
        int[] _actionsPerformed = new int[11] {0, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1};
        int[] _jumpsPerformed = new int[11] {0, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1};
        int[] _itemsUsed = new int[11] {0, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1};
        int[] _attacksPerformed = new int[11] {0, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1};

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
            StatisticsTrackingDelegates.onActionTracking += IncrementActionsUsed;
        }

        void RemoveDelegates()
        {
            StatisticsTrackingDelegates.onActionTracking -= IncrementActionsUsed;
        }


        public void OnZoneCompletion()
        {
            if(_actionsPerformed[0] == -1) _actionsPerformed[0] = 0;
            _actionsPerformed = ShiftRight(_actionsPerformed);

            if(_jumpsPerformed[0] == -1) _jumpsPerformed[0] = 0;
            _jumpsPerformed = ShiftRight(_jumpsPerformed);

            if(_itemsUsed[0] == -1) _itemsUsed[0] = 0;
            _itemsUsed = ShiftRight(_itemsUsed);

            if(_attacksPerformed[0] == -1) _attacksPerformed[0] = 0;
            _attacksPerformed = ShiftRight(_attacksPerformed);
        }

        public void ClearStats()
        {
            _actionsPerformed = new int[11] { 0, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1 };
            _jumpsPerformed = new int[11] { 0, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1 };
            _itemsUsed = new int[11] { 0, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1 };
            _attacksPerformed = new int[11] { 0, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1 };
        }



        // Get all the stats from the previous (upto) 10 stages
        public float GetActionsPerformedAverage(ActionType _aType)
        {
            float avg = 0;

            if(_aType == ActionType.Jump)
            {
                for(int i = 1; i <= 10; i++)
                {
                    if(_jumpsPerformed[i] == -1)
                    {
                        if(i > 1)
                        {
                            avg /= i - 1;
                        }
                        return avg;
                    }

                    avg += _jumpsPerformed[i];
                }
            }

            else if(_aType == ActionType.Item)
            {
                for(int i = 1; i <= 10; i++)
                {
                    if(_itemsUsed[i] == -1)
                    {
                        if(i > 1)
                        {
                            avg /= i - 1;
                        }
                        return avg;
                    }

                    avg += _itemsUsed[i];
                }
            }

            else if(_aType == ActionType.Attack)
            {
                for(int i = 1; i <= 10; i++)
                {
                    if(_attacksPerformed[i] == -1)
                    {
                        if(i > 1)
                        {
                            avg /= i - 1;
                        }
                        return avg;
                    }

                    avg += _attacksPerformed[i];
                }
            }

            // Catch clause for generic, or all actions performed
            else
            {
                for(int i = 1; i <= 10; i++)
                {
                    if(_actionsPerformed[i] == -1)
                    {
                        if(i > 1)
                        {
                            avg /= i - 1;
                        }
                        return avg;
                    }

                    avg += _actionsPerformed[i];
                }
            }
            

            

            avg /= 10;
            return avg;
        }


        // Add 1 every enemy kill
        // Add 2 first time due to -1 being the check amount
        void IncrementActionsUsed(ActionType aType)
        {
            _actionsPerformed[0]++;

            if(aType == ActionType.Jump)
            {
                _jumpsPerformed[0]++;
            }

            else if(aType == ActionType.Item)
            {
                _itemsUsed[0]++;
            }

            else if(aType == ActionType.Attack)
            {
                _attacksPerformed[0]++;
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
    }

    public enum ActionType
    {
        Generic,
        Jump,
        Item,
        Attack
    };
}