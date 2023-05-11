using System.Collections;
using System.Collections.Generic;
using UnityEngine;



namespace Delegates.Actors.Player
{
    public class PlayerAnimationDelegates : MonoBehaviour
    {
        public delegate void TEMP_ON_PLAYER_MOVEMENT(bool moving);
        public static TEMP_ON_PLAYER_MOVEMENT tEMP_ON_PLAYER_MOVEMENT;

        public delegate void OnPlayerAerialChange(bool aerialState);
        public static OnPlayerAerialChange onPlayerAerialChange;
    }
}