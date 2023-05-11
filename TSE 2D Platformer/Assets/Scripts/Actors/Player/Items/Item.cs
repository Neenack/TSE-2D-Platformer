using System.Collections;
using System.Collections.Generic;
using UnityEngine;



namespace Actors.Player.Items
{
    public abstract class Item
    {
        public virtual void BeginSelf() {}

        public abstract void UseItem();
    }
}