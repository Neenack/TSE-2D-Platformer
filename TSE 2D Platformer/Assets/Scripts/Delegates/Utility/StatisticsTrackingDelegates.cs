using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Actors.EnemyNS;

using Controllers.Utility.Statistics;



namespace Delegates.Utility
{
    public class StatisticsTrackingDelegates : MonoBehaviour
    {
        public delegate void OnEnemyDeathTracking();
        public static OnEnemyDeathTracking onEnemyDeathTracking;

        public delegate void OnRoomExplored();
        public static OnRoomExplored onRoomExplored;

        public delegate void OnIdleTracking(bool isIdle);
        public static OnIdleTracking onIdleTracking;

        public delegate void OnActionTracking(ActionType aType);
        public static OnActionTracking onActionTracking;

        public delegate void OnEnemyNearMiss();
        public static OnEnemyNearMiss onEnemyNearMiss;

        public delegate void OnProjectileNearMiss();
        public static OnProjectileNearMiss onProjectileNearMiss;

        public delegate void OnBombKill();
        public static OnBombKill onBombKill;

        public delegate void OnRopeUsed();
        public static OnRopeUsed onRopeUsed;

        public delegate void OnEnemyDetected();
        public static OnEnemyDetected onEnemyDetected;

        public delegate void OnPlayerHitByEnemy(EnemyType eT);
        public static OnPlayerHitByEnemy onPlayerHitByEnemy;

        public delegate void OnPlayerHitByTrap();
        public static OnPlayerHitByTrap onPlayerHitByTrap;

        public delegate void OnVaseDestroyed();
        public static OnVaseDestroyed onVaseDestroyed;
    }
}