using System.Collections;
using System.Collections.Generic;
using UnityEngine;



namespace Actors.Player
{
    public class PlayerStats : MonoBehaviour
    {
        [SerializeField]
        float _speed, _jumpPower, _climbSpeed;


        public float GetSpeed()
        {
            return _speed;
        }

        public float GetJumpPower()
        {
            return _jumpPower;
        }

        public float GetClimbSpeed()
        {
            return _climbSpeed;
        }
        

        void SetSpeed(float val)
        {
            _speed = val;
        }

        void SetJumpPower(float val)
        {
            _jumpPower = val;
        }

        void SetClimbSpeed(float val)
        {
            _climbSpeed = val;
        }
    }
}