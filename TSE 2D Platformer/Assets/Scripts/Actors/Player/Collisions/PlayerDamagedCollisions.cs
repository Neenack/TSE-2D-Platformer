using System.Collections;
using System.Collections.Generic;
using UnityEngine;


using Actors.EnemyNS;

using Delegates.Actors.Player;
using Delegates.Utility;


namespace Actors.Player.Collisions
{
    public class PlayerDamagedCollisions : MonoBehaviour
    {
        bool _isDead;

        public AudioSource soundEffect;


        void Start()
        {
            _isDead = false;
            SetupDelegates();
        }

        void OnDisable()
        {
            RemoveDelegates();
        }

        public void SetupDelegates()
        {
            PlayerStateDelegates.onPlayerDeathStateChange += SetIsDead;
        }

        public void RemoveDelegates()
        {
            PlayerStateDelegates.onPlayerDeathStateChange -= SetIsDead;
        }


        void OnTriggerEnter2D(Collider2D col)
        {
            if(col.gameObject.tag == "Player" || _isDead) return;

            if(col.gameObject.tag == "Enemy")
            {
                if(StatisticsTrackingDelegates.onPlayerHitByEnemy != null) StatisticsTrackingDelegates.onPlayerHitByEnemy(col.gameObject.GetComponent<EnemyStats>().GetType());
                _isDead = true;
                return;
            }

            if(col.gameObject.tag == "Projectile")
            {
                if(StatisticsTrackingDelegates.onPlayerHitByEnemy != null) StatisticsTrackingDelegates.onPlayerHitByEnemy(EnemyType.Screamer);
                _isDead = true;
                return;
            }

            if(col.gameObject.tag == "Trap")
            {
                if(StatisticsTrackingDelegates.onPlayerHitByTrap != null) StatisticsTrackingDelegates.onPlayerHitByTrap();
                _isDead = true;
                return;
            }
        }

        void OnTriggerExit2D(Collider2D col)
        {
            if(col.tag == "Player") return;
        }


        void SetIsDead(PlayerDeathState pDS)
        {
            if(pDS == PlayerDeathState.Alive)
            {
                soundEffect.Play();

                _isDead = false;
            }
        }
    }
}