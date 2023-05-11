using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Actors.Player.Actions;
using Actors.Player.Collisions;
using Actors.Player.Animation;



namespace Actors.Player
{
    [RequireComponent (typeof(PlayerStats))]
    [RequireComponent (typeof(PlayerMovement))]
    [RequireComponent (typeof(PlayerState))]
    [RequireComponent (typeof(PlayerCollisions))]
    [RequireComponent (typeof(PlayerSprite))]
    [RequireComponent (typeof(PlayerAnimations))]
    [RequireComponent (typeof(PlayerAttacking))]
    [RequireComponent (typeof(PlayerInventory))]

    public class Player : MonoBehaviour
    {
        [SerializeField]
        PlayerStats _statsComponent;

        [SerializeField]
        PlayerMovement _movementComponent;

        [SerializeField]
        PlayerState _stateComponent;

        [SerializeField]
        PlayerCollisions _collisionsComponent;

        [SerializeField]
        PlayerSprite _spriteComponent;

        [SerializeField]
        PlayerAnimations _animationsComponent;

        [SerializeField]
        PlayerAttacking _attackingComponent;

        [SerializeField]
        PlayerInventory _playerInventory;


        // Setup scripts that need to begin
        public void BeginSelf()
        {
            _movementComponent.BeginSelf();
            _stateComponent.BeginSelf();
            _collisionsComponent.BeginSelf();    
            _spriteComponent.BeginSelf();
            _animationsComponent.BeginSelf();
            _attackingComponent.BeginSelf();
            _playerInventory.BeginSelf();
        }
    }
}