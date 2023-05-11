using System.Collections;
using System.Collections.Generic;
using UnityEngine;



namespace Actors.Player.Items
{
    public class TestItemBaseScript : Item
    {
        public override void BeginSelf()
        {
            base.BeginSelf();
        }

        public override void UseItem()
        {
            Debug.Log("ITEM USED!!!");
        }
    }
}