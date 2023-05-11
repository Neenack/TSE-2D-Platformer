using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Actors.Player;

using Delegates.Actors.Player;



namespace Actors.Player.Animation
{
    public class PlayerAnimations : MonoBehaviour
    {

        Animator _animator;

        public void BeginSelf()
        {
            _animator = GetComponent<Animator>();

            SetupDelegates();    
        }

        void OnDisable()
        {
            RemoveDelegates();
        }


        void SetupDelegates()
        {
            PlayerAnimationDelegates.tEMP_ON_PLAYER_MOVEMENT += TEMP_SET_MOVING_TRIGGER;
            PlayerStateDelegates.onPlayerGroundedStateChange += SetAerialTrigger;
        }

        void RemoveDelegates()
        {
            PlayerAnimationDelegates.tEMP_ON_PLAYER_MOVEMENT -= TEMP_SET_MOVING_TRIGGER;
            PlayerStateDelegates.onPlayerGroundedStateChange -= SetAerialTrigger;
        }


        void TEMP_SET_MOVING_TRIGGER(bool moving)
        {
            if(moving)
            {
                _animator.SetBool("TEMP_TRIGGER_MOVING", true);
            }

            else
            {
                _animator.SetBool("TEMP_TRIGGER_MOVING", false);
            }
        }

        void SetAerialTrigger(PlayerGroundedState pState)
        {
            if(pState == PlayerGroundedState.Grounded)
            {
                _animator.SetBool("Aerial", false);
            }

            else
            {
                _animator.SetBool("Aerial", true);
            }
        }
    }
}