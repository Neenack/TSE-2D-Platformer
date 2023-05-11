using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Delegates.Actors.Player;
using Delegates.Utility;



namespace Actors.Player.Items
{
    public class RopeItem : Item
    {


        public override void UseItem()
        {
            if(PlayerActionsDelegates.onPlayerUseRope != null)
            {
                PlayerActionsDelegates.onPlayerUseRope();
            }
        }
    }
}