using System;
using UnityEngine;

namespace Resources.Scripts.Player{
    public class PlayerAnimator : MonoBehaviour{
        
        [SerializeField] private PlayerMovement _playerMovementScript;
        [SerializeField] private playerMoveState _state;
        [SerializeField] private Animator _animator;
        
        // Property Indexes:
        private static readonly int Walk = Animator.StringToHash("Walk");


        private void Update(){
            // Assign player's current state:
            _state = _playerMovementScript._state;
            
            // Change animation based on player's current state:
            ProcessStateAnimation();
        }

        private void IdleAnimation(){
            _animator.SetBool(Walk, false);
        }
        private void WalkingAnimation(){
            _animator.SetBool(Walk, true);
        }
        
        private void ProcessStateAnimation(){
            switch(_state) {
                case playerMoveState.Idle:
                    IdleAnimation();
                    break;
                case playerMoveState.Walking:
                    WalkingAnimation();
                    break;
                case playerMoveState.Jump:
                    
                    break;
                case playerMoveState.AirControl:
                    
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}
