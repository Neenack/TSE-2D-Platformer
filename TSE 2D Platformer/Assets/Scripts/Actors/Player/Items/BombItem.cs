using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Delegates.Actors.Player;


namespace Actors.Player.Items
{
    public class BombItem : Item
    {
        public override void UseItem()
        {
            if(PlayerActionsDelegates.onPlayerUseBomb != null)
            {
                PlayerActionsDelegates.onPlayerUseBomb();
            }
        }
    }
}