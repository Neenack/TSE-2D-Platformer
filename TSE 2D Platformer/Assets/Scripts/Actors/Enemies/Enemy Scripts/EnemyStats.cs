using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Delegates.Actors.EnemyNS;



namespace Actors.EnemyNS
{
    public class EnemyStats : MonoBehaviour
    {
        [SerializeField]
        int _maxHp;

        int _currentHp;

        [SerializeField]
        int _attackPower;

        [SerializeField]
        EnemyType _type;


        void BeginSelf()
        {
            _currentHp = _maxHp;
        }


        public EnemyType GetType()
        {
            return _type;
        }

        
        public void HpChange(int change, bool up)
        {
            if(up)
            {
                _currentHp += change;
                if(_currentHp > _maxHp)
                {
                    _currentHp = _maxHp;
                }
            }
        }

        public void CheckDeath()
        {
            if(_currentHp <= 0 && EnemyStatsDelegates.onEnemyDeath != null)
            {
                EnemyStatsDelegates.onEnemyDeath(this.gameObject.GetComponent<Enemy>());
            }
        }
    }

    public enum EnemyType
    {
        AngryBob,
        Jumper,
        Screamer
    }
}