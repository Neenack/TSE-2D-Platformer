using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Delegates.Actors.Player;

namespace Actors.Player.Collisions
{
    [RequireComponent(typeof(BoxCollider2D))]
    public class PlayerGroundCollisions : MonoBehaviour
    {
        void OnTriggerStay2D(Collider2D col)
        {
            if ((col.gameObject.tag == "Walkable" || col.gameObject.tag == "Enemy") && PlayerStateDelegates.onPlayerGroundedStateChange != null)
            {
                PlayerStateDelegates.onPlayerGroundedStateChange(PlayerGroundedState.Grounded);
                return;
            }   
        }

        void OnTriggerExit2D(Collider2D col)
        {
            if ((col.gameObject.tag == "Walkable" || col.gameObject.tag == "Enemy") && PlayerStateDelegates.onPlayerGroundedStateChange != null)
            {
                PlayerStateDelegates.onPlayerGroundedStateChange(PlayerGroundedState.Aerial);
                return;
            }
        }
    }
}