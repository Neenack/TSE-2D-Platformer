using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Actors.EnemyNS;



namespace Delegates.Actors.EnemyNS
{
    public class EnemySpawningDelegates : MonoBehaviour
    {
        public delegate void OnEnemySpawn(Enemy e);
        public static OnEnemySpawn onEnemySpawn;

        public delegate void OnTrapSpawn();
        public static OnTrapSpawn onTrapSpawn;

        public delegate void OnEnemyDespawn(Enemy e);
        public static OnEnemyDespawn onEnemyDespawn;
    }
}

