using System.Collections;
using System.Collections.Generic;
using UnityEngine;



namespace Delegates.Actors.Player
{
    public class PlayerActionsDelegates : MonoBehaviour
    {
        public delegate void OnPlayerAttack();
        public static OnPlayerAttack onPlayerAttack;

        public delegate void OnPlayerUseItem();
        public static OnPlayerUseItem onPlayerUseItem;

        public delegate void OnPlayerSwitchItem(int itemNum);
        public static OnPlayerSwitchItem onPlayerSwitchItem;

        public delegate void OnPlayerUseBomb();
        public static OnPlayerUseBomb onPlayerUseBomb;

        public delegate void OnPlayerUseRope();
        public static OnPlayerUseRope onPlayerUseRope;

        public delegate void OnVaseInRange(ItemVaseController v);
        public static OnVaseInRange onVaseInRange;

        public delegate void OnVaseDestroyed();
        public static OnVaseDestroyed onVaseDestroyed;
    }
}
