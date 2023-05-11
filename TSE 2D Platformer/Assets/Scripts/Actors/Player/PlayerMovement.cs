using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Controllers.Utility.Statistics;

using Delegates.Actors.Player;
using Delegates.Utility;


namespace Actors.Player
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class PlayerMovement : MonoBehaviour
    {
        float _timeNotMoving;
        bool _currentlyIdle;

        // Stop FixedUpdate firing before everything is initialized
        void Awake()
        {
            enabled = false;

            _timeNotMoving = 0f;
            _currentlyIdle = false;
        }


        Rigidbody2D _rigidBody;

        public void BeginSelf()
        {
            enabled = true;

            _rigidBody = GetComponent<Rigidbody2D>();

            SetupDelegates();
        }

        void OnDisable()
        {
            RemoveDelegates();
        }

        
        void SetupDelegates()
        {
            PlayerMovementDelegates.onPlayerMoveHorizontal += MoveHorizontal;
            PlayerMovementDelegates.onPlayerJump += Jump;
            PlayerMovementDelegates.onPlayerClimb += Climb;

            PlayerStateDelegates.onPlayerLadderTouchingStateChange += UpdatePlayerGravityOnLadderStateChange;
        }

        void RemoveDelegates()
        {
            PlayerMovementDelegates.onPlayerMoveHorizontal -= MoveHorizontal;
            PlayerMovementDelegates.onPlayerJump -= Jump;
            PlayerMovementDelegates.onPlayerClimb -= Climb;

            PlayerStateDelegates.onPlayerLadderTouchingStateChange -= UpdatePlayerGravityOnLadderStateChange;
        }


        void MoveHorizontal(float horizontalSpeed)
        {
            _rigidBody.velocity = new Vector2(horizontalSpeed * GetComponent<PlayerStats>().GetSpeed(), _rigidBody.velocity.y);
        }

        void Jump()
        {
            _rigidBody.velocity = new Vector2(_rigidBody.velocity.x, GetComponent<PlayerStats>().GetJumpPower());

            if(StatisticsTrackingDelegates.onActionTracking != null) StatisticsTrackingDelegates.onActionTracking(ActionType.Jump);
        }


        void UpdatePlayerGravityOnLadderStateChange(PlayerTouchingLadderState pState)
        {
            if(pState == PlayerTouchingLadderState.Not_Touching)
            {
                _rigidBody.gravityScale = 6;
            }

            else
            {
                _rigidBody.velocity = new Vector2(_rigidBody.velocity.x, 0);
                _rigidBody.gravityScale = 0;
            }
        }

        void Climb(bool up)
        {
            if(up)
            {
                _rigidBody.velocity = new Vector2(_rigidBody.velocity.x, GetComponent<PlayerStats>().GetClimbSpeed());
            }

            else
            {
                _rigidBody.velocity = new Vector2(_rigidBody.velocity.x, GetComponent<PlayerStats>().GetClimbSpeed() * -1);
            }
        }




        // Used to set the player's motion state
        void FixedUpdate()
        {
            if(_rigidBody.velocity.magnitude > 0 && PlayerAnimationDelegates.tEMP_ON_PLAYER_MOVEMENT != null) PlayerAnimationDelegates.tEMP_ON_PLAYER_MOVEMENT(true);
            else if(PlayerAnimationDelegates.tEMP_ON_PLAYER_MOVEMENT != null) PlayerAnimationDelegates.tEMP_ON_PLAYER_MOVEMENT(false);

            if(_rigidBody.velocity.magnitude == 0) 
            {
                _timeNotMoving += Time.deltaTime;
            }
            else if(StatisticsTrackingDelegates.onIdleTracking != null)
            {
                _timeNotMoving = 0;
                _currentlyIdle = false;

                StatisticsTrackingDelegates.onIdleTracking(false);
            }

            if(_timeNotMoving >= 3 && StatisticsTrackingDelegates.onIdleTracking != null && !_currentlyIdle) 
            {
                _currentlyIdle = true;
                StatisticsTrackingDelegates.onIdleTracking(true);
            }
        }

        public float GetTimeNotMoving()
        {
            return _timeNotMoving;
        }
    }
}