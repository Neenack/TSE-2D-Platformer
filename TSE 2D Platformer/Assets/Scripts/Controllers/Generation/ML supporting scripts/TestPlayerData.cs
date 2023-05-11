using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace K_Means_Plus_Plus
{
    internal class TestPlayerData
    {
        internal static readonly PlayerData BadPlayer = new PlayerData
        {

            //Increase with skill
            EnemiesSpawned = 0,
            RoomsExplored = 0,
            ItemsUsed = 0,
            EnemiesKilled = 0,
            NearMissesWithEnemies = 0,
            NearMissesWithProjectiles = 0,
            BombKills = 0,
            RopesUsed = 0,



            //Decrease with skill
            Time = 200,
            LongestTimeIn1Room = 0,
            Jumps = 0,
            Attacks = 0,
            IdleTime = 0,
            EnemiesDetected = 0,
            DeathByAngryBob = 0,
            DeathByScreamer = 0,
            DeathByJumper = 0,
            DeathByTrap = 0,
        };

        internal static readonly PlayerData AveragePlayer = new PlayerData
        {
            //Increase with skill
            EnemiesSpawned = 0,
            RoomsExplored = 0,
            ItemsUsed = 0,
            EnemiesKilled = 0,
            NearMissesWithEnemies = 0,
            NearMissesWithProjectiles = 0,
            BombKills = 0,
            RopesUsed = 0,



            //Decrease with skill
            Time = 0,
            LongestTimeIn1Room = 0,
            Jumps = 0,
            Attacks = 0,
            IdleTime = 0,
            EnemiesDetected = 0,
            DeathByAngryBob = 0,
            DeathByScreamer = 0,
            DeathByJumper = 0,
            DeathByTrap = 0,
        };

        internal static readonly PlayerData GoodPlayer = new PlayerData
        {
            //Increase with skill
            EnemiesSpawned = 0,
            RoomsExplored = 0,
            ItemsUsed = 0,
            EnemiesKilled = 0,
            NearMissesWithEnemies = 0,
            NearMissesWithProjectiles = 0,
            BombKills = 0,
            RopesUsed = 0,



            //Decrease with skill
            Time = 0,
            LongestTimeIn1Room = 0,
            Jumps = 0,
            Attacks = 0,
            IdleTime = 0,
            EnemiesDetected = 0,
            DeathByAngryBob = 0,
            DeathByScreamer = 0,
            DeathByJumper = 0,
            DeathByTrap = 0,
        };

    }
}