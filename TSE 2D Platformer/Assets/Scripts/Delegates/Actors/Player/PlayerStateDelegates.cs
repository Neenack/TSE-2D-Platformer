using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Actors.Player;


namespace Delegates.Actors.Player
{
    public class PlayerStateDelegates : MonoBehaviour
    {
        public delegate void OnPlayerGroundedStateChange(PlayerGroundedState pState);
        public static OnPlayerGroundedStateChange onPlayerGroundedStateChange;


        public delegate void OnPlayerLadderTouchingStateChange(PlayerTouchingLadderState pState);
        public static OnPlayerLadderTouchingStateChange onPlayerLadderTouchingStateChange;


        public delegate void OnPlayerTurn(PlayerFacingDirectionState pState);
        public static OnPlayerTurn onPlayerTurn;

        public delegate void OnPlayerDeathStateChange(PlayerDeathState pState);
        public static OnPlayerDeathStateChange onPlayerDeathStateChange;

        public delegate PlayerDeathState GetPlayerDeathState();
        public static GetPlayerDeathState getPlayerDeathState;


    }
}
