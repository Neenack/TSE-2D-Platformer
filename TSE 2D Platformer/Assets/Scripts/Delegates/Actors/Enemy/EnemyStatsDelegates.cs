using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Actors.EnemyNS;


namespace Delegates.Actors.EnemyNS
{
    public class EnemyStatsDelegates : MonoBehaviour
    {
        public delegate void OnEnemyHit(Enemy e);
        public static OnEnemyHit onEnemyHit;

        public delegate void OnEnemyDeathCheck(List<Enemy> Es);
        public static OnEnemyDeathCheck onEnemyDeathCheck;

        public delegate void OnEnemyHPChange(Enemy e, int val, bool up);
        public static OnEnemyHPChange onEnemyHPChange;

        public delegate void OnEnemyDeath(Enemy e);
        public static OnEnemyDeath onEnemyDeath;
    }
}