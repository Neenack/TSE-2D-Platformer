using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Actors.EnemyNS;

using Delegates.Actors.Player;
using Delegates.Utility;


namespace Actors.Player.Collisions
{
    [RequireComponent(typeof(CircleCollider2D))]

    public class PlayerCollisions : MonoBehaviour
    {
        // This detects the player hitting the ground, rudimentary, but working
        [SerializeField]
        PlayerGroundCollisions _groundCollisionDetector;

        [SerializeField] AudioSource sound;
        [SerializeField] AudioClip death;

        bool _isDead = false;

        public void BeginSelf()
        {
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

        void OnTriggerStay2D(Collider2D col)
        {
            if (col.tag == "PlayerChildObjectTag" || _isDead) return;


            if (col.gameObject.tag == "Climbable" && PlayerStateDelegates.onPlayerLadderTouchingStateChange != null)
            {

                PlayerStateDelegates.onPlayerLadderTouchingStateChange(PlayerTouchingLadderState.Touching);
                return;
            }

            //Handles player death
            if (col.gameObject.tag == "Enemy")
            {
                sound.clip = death; sound.Play();
                if (StatisticsTrackingDelegates.onPlayerHitByEnemy != null) StatisticsTrackingDelegates.onPlayerHitByEnemy(col.gameObject.GetComponent<EnemyStats>().GetType());
                PlayerStateDelegates.onPlayerDeathStateChange(PlayerDeathState.Dead);
                _isDead = true;
            }

            else if (col.gameObject.tag == "Projectile")
            {
                sound.clip = death; sound.Play();
                if (StatisticsTrackingDelegates.onPlayerHitByEnemy != null) StatisticsTrackingDelegates.onPlayerHitByEnemy(EnemyType.Screamer);
                PlayerStateDelegates.onPlayerDeathStateChange(PlayerDeathState.Dead);
                _isDead = true;
            }

            else if (col.gameObject.tag == "Trap")
            {
                sound.clip = death; sound.Play();
                if (StatisticsTrackingDelegates.onPlayerHitByTrap != null) StatisticsTrackingDelegates.onPlayerHitByTrap();
                PlayerStateDelegates.onPlayerDeathStateChange(PlayerDeathState.Dead);
                _isDead = true;
            }
        }

        void OnTriggerExit2D(Collider2D col)
        {
            if (col.tag == "PlayerChildObjectTag") return;

            if (col.gameObject.tag == "Climbable" && PlayerStateDelegates.onPlayerLadderTouchingStateChange != null)
            {

                PlayerStateDelegates.onPlayerLadderTouchingStateChange(PlayerTouchingLadderState.Not_Touching);
                return;
            }

        }

        private void OnCollisionEnter2D(Collision2D col)
        {
            if (col.gameObject.tag == "PlayerChildObjectTag" || _isDead) return;
            //Handles player death
            //Handles player death
            if (col.gameObject.tag == "Enemy")
            {
                sound.clip = death; sound.Play();
                if (StatisticsTrackingDelegates.onPlayerHitByEnemy != null) StatisticsTrackingDelegates.onPlayerHitByEnemy(col.gameObject.GetComponent<EnemyStats>().GetType());
                PlayerStateDelegates.onPlayerDeathStateChange(PlayerDeathState.Dead);
                _isDead = true;
            }

            else if (col.gameObject.tag == "Projectile")
            {
                sound.clip = death; sound.Play();
                if (StatisticsTrackingDelegates.onPlayerHitByEnemy != null) StatisticsTrackingDelegates.onPlayerHitByEnemy(EnemyType.Screamer);
                PlayerStateDelegates.onPlayerDeathStateChange(PlayerDeathState.Dead);
                _isDead = true;
            }

            else if (col.gameObject.tag == "Trap")
            {
                sound.clip = death; sound.Play();
                if (StatisticsTrackingDelegates.onPlayerHitByTrap != null) StatisticsTrackingDelegates.onPlayerHitByTrap();
                PlayerStateDelegates.onPlayerDeathStateChange(PlayerDeathState.Dead);
                _isDead = true;
            }
        }

        void SetIsDead(PlayerDeathState pDS)
        {
            if (pDS == PlayerDeathState.Alive)
            {
                _isDead = false;
            }

        }
    }
}