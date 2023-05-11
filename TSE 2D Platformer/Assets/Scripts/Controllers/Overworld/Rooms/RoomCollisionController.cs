using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Actors.Player;

using Delegates.Utility;



namespace Controllers.Overworld.Rooms
{
    public class RoomCollisionController : MonoBehaviour
    {
        BoxCollider2D _collider;

        float _timeInRoom;

        bool _explored;

        void Awake()
        {
            _collider = GetComponent<BoxCollider2D>();
            _timeInRoom = 0f;
            _explored = false;
        }

        void OnTriggerStay2D(Collider2D col)
        {
            if (col.gameObject.tag == "Player")
            {
                if(col.GetComponent<PlayerMovement>().GetTimeNotMoving() < 1)
                {
                    _timeInRoom += Time.deltaTime;
                }
                
                if(_timeInRoom >= 2 && !_explored)
                {
                    if(StatisticsTrackingDelegates.onRoomExplored != null)
                    {
                        StatisticsTrackingDelegates.onRoomExplored();
                        _explored = true;
                    }
                }
            }   
        }


        public float GetTimeInRoom()
        {
            return _timeInRoom;
        }
    }
}