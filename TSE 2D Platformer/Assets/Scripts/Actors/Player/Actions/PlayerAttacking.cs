using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Actors.Player.Collisions;
using Actors.EnemyNS;

using Controllers.Utility.Statistics;

using Delegates.Actors.Player;
using Delegates.Actors.EnemyNS;
using Delegates.Utility;



namespace Actors.Player.Actions
{
    public class PlayerAttacking : MonoBehaviour
    {
        [SerializeField]
        PlayerAttackCollisions _playerAttackingCollisions;

        [SerializeField]
        GameObject _attackObjectChild;
        PlayerAttackCollisions _playerChildAttackingCollisions;

        [SerializeField] AudioSource attackSound;
        [SerializeField] AudioClip attack, breakVase, enemyDeath;



        [SerializeField] SpriteRenderer _sprite;

        public void BeginSelf()
        {
            _playerAttackingCollisions.BeginSelf();

            _playerChildAttackingCollisions = _attackObjectChild.GetComponent<PlayerAttackCollisions>();
            _playerChildAttackingCollisions.BeginSelf();

            _sprite.enabled = false;

            SetupDelegates();
        }

        void OnDisable()
        {
            RemoveDelegates();
        }

        void SetupDelegates()
        {
            PlayerActionsDelegates.onPlayerAttack += Attack;
        }

        void RemoveDelegates()
        {
            PlayerActionsDelegates.onPlayerAttack -= Attack;
        }

        void Attack()
        {
            attackSound.clip = attack;
            attackSound.Play();

            StartCoroutine(DisplaySprite());
            StartCoroutine(Attacking());

            /*foreach(Enemy e in _playerAttackingCollisions.GetEnemiesList())
            {
                if(EnemyStatsDelegates.onEnemyHit != null)
                {
                    EnemyStatsDelegates.onEnemyHit(e);
                }
            }

            if (EnemyStatsDelegates.onEnemyDeathCheck != null) EnemyStatsDelegates.onEnemyDeathCheck(_playerAttackingCollisions.GetEnemiesList());

            _playerAttackingCollisions.ClearInRange();*/

            if (StatisticsTrackingDelegates.onActionTracking != null) StatisticsTrackingDelegates.onActionTracking(ActionType.Attack);
        }

        void DestroyVase(ItemVaseController vase)
        {
            attackSound.clip = breakVase;
            attackSound.Play();

            Destroy(vase.gameObject);

            if (PlayerActionsDelegates.onVaseDestroyed != null) PlayerActionsDelegates.onVaseDestroyed();
        }

        IEnumerator DisplaySprite()
        {
            _sprite.enabled = true;
            yield return new WaitForSeconds(0.25f);
            _sprite.enabled = false;
        }


        IEnumerator Attacking()
        {
            float attackingTime = 0.24f;
            float timeGone = 0f;

            while(timeGone < attackingTime)
            {   
                //Need to check child object (attack object) too
                foreach (Enemy e in _playerChildAttackingCollisions.GetEnemiesList())
                {
                    if (EnemyStatsDelegates.onEnemyHit != null)
                    {
                        attackSound.clip = enemyDeath;
                        attackSound.Play();
                        EnemyStatsDelegates.onEnemyHit(e);
                    }
                }

                if (EnemyStatsDelegates.onEnemyDeathCheck != null) EnemyStatsDelegates.onEnemyDeathCheck(_playerChildAttackingCollisions.GetEnemiesList());

                _playerChildAttackingCollisions.ClearInRange();

                for(int i = 0; i < _playerChildAttackingCollisions.GetVasesList().Count; i++)
                {
                    DestroyVase(_playerChildAttackingCollisions.GetVasesList()[i]);
                    i++;
                }

                yield return null;
                timeGone += Time.deltaTime;
            }
        }
    }
}