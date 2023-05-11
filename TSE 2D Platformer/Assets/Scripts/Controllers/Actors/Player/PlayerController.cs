using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Controllers.Utility;

using Actors.Player;
using Delegates.Actors.Player;

using Delegates.Utility;


namespace Controllers.Actors.PlayerNS
{
    public class PlayerController : MonoBehaviour
    {
        PlayerState _playerState;
        GameObject _player;
        Vector3 _startingPosition;
        SpriteRenderer _playerSpriteRenderer;
        CircleCollider2D _playerCircleCollider;
        Rigidbody2D _playerRigidbody;
        float _gravityScale;

        float _horizontal, _vertical;

        float _attack, _useItem, _pause;
        float _numpad1, _numpad2, _numpad3, _numpad4;

        bool _jumpButtonReleased, _jumpOnCooldown, _attackButtonReleased, _useItemButtonReleased, _PauseButtonReleased;
        bool _numpad1Released, _numpad2Released, _numpad3Released, _numpad4Released;


        float _attackWaitTime;
        bool _attackOnCooldown;



        float _respawnTimer = 1;
        // Stop Update & Fixed Update from taking place until spawn
        void Awake()
        {
            enabled = false;
        }
        
        // Called from GameController on spawn, enables Update & FixedUpdate.
        public void BeginSelf(Player player)
        {
            _playerState = player.GetComponent<PlayerState>();
            _player = GameObject.FindGameObjectWithTag("Player");
            _startingPosition = _player.transform.position;
            _playerSpriteRenderer = _player.GetComponent<SpriteRenderer>();
            _playerCircleCollider = _player.GetComponent<CircleCollider2D>();
            _playerRigidbody = _player.GetComponent<Rigidbody2D>();
            _gravityScale = _playerRigidbody.gravityScale;

            enabled = true;

            _jumpButtonReleased = true;
            _attackButtonReleased = true;
            _useItemButtonReleased = true;
            _PauseButtonReleased = true;

            _attackWaitTime = 0.25f;
            _attackOnCooldown = false;

            _numpad1Released = true;
            _numpad2Released = true;
            /*_numpad3Released = true;
            _numpad4Released = true;*/
        }

        public void PauseSelf()
        {
            enabled = false;
        }


        void Update()
        {
            // GetAxisRaw bypasses Time.deltaTime
            _pause = Input.GetAxisRaw("Pause");

            // Placed here as Time.deltaTime = 0 prevents FixedUpdate from working properly
            if(_pause > 0 && _PauseButtonReleased && UIDelegates.onPause != null)
            {
                UIDelegates.onPause();

                _PauseButtonReleased = false;
            }
            else if(_pause == 0)
            {
                _PauseButtonReleased = true;
            }

            if(GameController._isPaused) return;

            _horizontal = Input.GetAxis("Horizontal");
            _vertical = Input.GetAxis("Jump");

            _attack = Input.GetAxis("Attack");
            _useItem = Input.GetAxis("UseItem");

            _numpad1 = Input.GetAxis("Numpad1");
            _numpad2 = Input.GetAxis("Numpad2");
            /*_numpad3 = Input.GetAxis("Numpad3");
            _numpad4 = Input.GetAxis("Numpad4");*/
        }

        void FixedUpdate()
        {
            if(GameController._isPaused) return;

            //Reactivate collider and hitbox when alive
            if (_playerState.GetPlayerDeathState() == PlayerDeathState.Alive && _playerSpriteRenderer.enabled == false && _playerCircleCollider.enabled == false)
            {
                _playerSpriteRenderer.enabled = true;
                _playerCircleCollider.enabled = true;
                _playerRigidbody.gravityScale = _gravityScale;
            }

            //Player only moves if not dead
            if (_playerState.GetPlayerDeathState() == PlayerDeathState.Alive)
            {
                /// Handle Player Movement

                // Handle left/ right walking
                if (_horizontal != 0 && PlayerMovementDelegates.onPlayerMoveHorizontal != null)
                {
                    PlayerMovementDelegates.onPlayerMoveHorizontal(_horizontal);

                    if (_horizontal > 0 && PlayerStateDelegates.onPlayerTurn != null) PlayerStateDelegates.onPlayerTurn(PlayerFacingDirectionState.Right);
                    else if (PlayerStateDelegates.onPlayerTurn != null) PlayerStateDelegates.onPlayerTurn(PlayerFacingDirectionState.Left);
                }

                // Stop player
                else if (_horizontal == 0) PlayerMovementDelegates.onPlayerMoveHorizontal(_horizontal);

                // Handle jumping
                if (_vertical > 0 && !_jumpOnCooldown && _jumpButtonReleased && _playerState.GetGroundedState() == PlayerGroundedState.Grounded && _playerState.GetLadderTouchingState() != PlayerTouchingLadderState.Touching && PlayerMovementDelegates.onPlayerJump != null)
                {
                    _jumpButtonReleased = false;
                    StartCoroutine(JumpTimer());
                    PlayerMovementDelegates.onPlayerJump();
                }

                // Handle ladder movement
                else if (_vertical != 0 && _playerState.GetLadderTouchingState() == PlayerTouchingLadderState.Touching && PlayerMovementDelegates.onPlayerClimb != null)
                {
                    if (_vertical > 0) PlayerMovementDelegates.onPlayerClimb(true);
                    else PlayerMovementDelegates.onPlayerClimb(false);
                }

                // Jump button has been released
                else if (_vertical == 0) _jumpButtonReleased = true;



                /// Handle Player Actions
                // Attacking
                if (_attack > 0 && !_attackOnCooldown && _attackButtonReleased && PlayerActionsDelegates.onPlayerAttack != null)
                {
                    _attackButtonReleased = false;
                    
                    StartCoroutine(AttackTimer());

                    PlayerActionsDelegates.onPlayerAttack();
                }

                else if (_attack == 0)
                {
                    _attackButtonReleased = true;
                }


                // Using Item
                if (_useItem > 0 && _useItemButtonReleased && PlayerActionsDelegates.onPlayerUseItem != null)
                {
                    _useItemButtonReleased = false;

                    PlayerActionsDelegates.onPlayerUseItem();
                }

                else if (_useItem == 0)
                {
                    _useItemButtonReleased = true;
                }
            }
           
            CheckPlayerDeath();

            // Numpad1
            if(_numpad1 > 0 && _numpad1Released && PlayerActionsDelegates.onPlayerSwitchItem != null)
            {
                _numpad1Released = false;

                PlayerActionsDelegates.onPlayerSwitchItem(1);
            }

            else if(_numpad1 == 0)
            {
                _numpad1Released = true;
            }


            // Numpad2
            if(_numpad2 > 0 && _numpad2Released && PlayerActionsDelegates.onPlayerSwitchItem != null)
            {
                _numpad2Released = false;

                PlayerActionsDelegates.onPlayerSwitchItem(2);
            }

            else if(_numpad2 == 0)
            {
                _numpad2Released = true;
            }


            // Numpad3
            /*if(_numpad3 > 0 && _numpad3Released && PlayerActionsDelegates.onPlayerSwitchItem != null)
            {
                _numpad3Released = false;

                PlayerActionsDelegates.onPlayerSwitchItem(3);
            }

            else if(_numpad3 == 0)
            {
                _numpad3Released = true;
            }


            // Numpad4
            if(_numpad4 > 0 && _numpad4Released && PlayerActionsDelegates.onPlayerSwitchItem != null)
            {
                _numpad4Released = false;

                PlayerActionsDelegates.onPlayerSwitchItem(4);
            }

            else if(_numpad4 == 0)
            {
                _numpad4Released = true;
            }*/
        }

        // Fix that stops player from jumping every frame since they haven't entirely left the ground yet.
        IEnumerator JumpTimer()
        {
            _jumpOnCooldown = true;
            yield return new WaitForSeconds(0.05f);
            _jumpOnCooldown = false;
        }

        IEnumerator AttackTimer()
        {
            _attackOnCooldown = true;
            yield return new WaitForSeconds(_attackWaitTime);
            _attackOnCooldown = false;
        }

        void CheckPlayerDeath()
        {
            //Handles player death - moves player back to the start of the level, and disables collider and renderer for a second
            if (_playerState.GetPlayerDeathState() == PlayerDeathState.Dead)
            {
                if (_playerCircleCollider.enabled == true && _playerSpriteRenderer.enabled == true)
                {
                    _playerSpriteRenderer.enabled = false;
                    _playerCircleCollider.enabled = false;
                    _playerRigidbody.gravityScale = 0.0f;
                    _playerRigidbody.velocity = Vector3.zero;
                    

                    //Reset bools: prevents input not working on spawning
                    _jumpButtonReleased = true;
                    _jumpOnCooldown = false;
                    _useItemButtonReleased = true;
                    _attackButtonReleased = true;

                    //Set timer
                    _respawnTimer = 1;
                }
                //1 second timer
                else                 
                {
                    if (_respawnTimer > 0)
                    {                      
                        _respawnTimer -= Time.deltaTime;
                        return;
                    }

                    //Spawn player after timer - moves player to starting point
                    _player.transform.position = _startingPosition;
                    PlayerStateDelegates.onPlayerDeathStateChange(PlayerDeathState.Alive);
                }
            }
        }
    }
}