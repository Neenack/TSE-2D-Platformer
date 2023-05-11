using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.ML.Data;

namespace K_Means_Clustering
{
    internal class PlayerData
    {
        //Increase with skill
        [LoadColumn(0)]
        public float EnemiesSpawned;

        [LoadColumn(3)]
        public float RoomsExplored;

        [LoadColumn(6)]
        public float ItemsUsed;

        [LoadColumn(8)]
        public float EnemiesKilled;

        [LoadColumn(9)]
        public float NearMissesWithEnemies;

        [LoadColumn(10)]
        public float NearMissesWithProjectiles;

        [LoadColumn(11)]
        public float BombKills;

        [LoadColumn(12)]
        public float RopesUsed;



        //Decrease with skill
        [LoadColumn(2)]
        public float Time;

        [LoadColumn(4)]
        public float LongestTimeIn1Room;

        [LoadColumn(5)]
        public float Jumps;

        [LoadColumn(7)]
        public float Attacks;

        [LoadColumn(13)]
        public float IdleTime;

        [LoadColumn(14)]
        public float EnemiesDetected;

        [LoadColumn(15)]
        public float DeathByAngryBob;

        [LoadColumn(15)]
        public float DeathByScreamer;

        [LoadColumn(16)]
        public float DeathByJumper;

        [LoadColumn(17)]
        public float DeathByTrap;

       
        
    }
}
