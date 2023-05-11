using System.Collections;
using System.Collections.Generic;
using UnityEngine;



namespace Delegates.Actors.Player
{
    public class PlayerMovementDelegates : MonoBehaviour
    {
        public delegate void OnPlayerMoveHorizontal(float horizontal);
        public static OnPlayerMoveHorizontal onPlayerMoveHorizontal;

        public delegate void OnPlayerJump();
        public static OnPlayerJump onPlayerJump;

        public delegate void OnPlayerClimb(bool up);
        public static OnPlayerClimb onPlayerClimb;
    }
}