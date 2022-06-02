using System;
using UnityEngine;

namespace Resources.Scripts.Player{
    public class PlayerAnimator : MonoBehaviour{
        
        [SerializeField] private PlayerMovement _playerMovementScript;
        [SerializeField] private playerMoveState _state;
        [SerializeField] private Animator _animator;
        
        // Property Indexes:
        private static readonly int Walk = Animator.StringToHash("Walk");
        private static readonly int Jump = Animator.StringToHash("Jump");
        private static readonly int Falling = Animator.StringToHash("Falling");
        private static readonly int Land = Animator.StringToHash("Land");
        private static readonly int Dash = Animator.StringToHash("Dash");


        private void Update(){
            // Assign player's current state:
            _state = _playerMovementScript._state;
            
            // Change animation based on player's current state:
            ProcessStateAnimation();
        }

        private void ResetAnimator(){
            _animator.SetBool(Walk, false);
            _animator.SetBool(Jump, false);
            _animator.SetBool(Falling, false);
            _animator.SetBool(Land, false);
            _animator.SetBool(Dash, false);
        }
        private void ProcessStateAnimation(){
            ResetAnimator();
            switch(_state) {
                case playerMoveState.Idle:
                    break;
                case playerMoveState.Walking:
                    _animator.SetBool(Walk, true);
                    break;
                case playerMoveState.Jump:
                    _animator.SetBool(Jump, true);
                    break;
                case playerMoveState.AirControl:
                    _animator.SetBool(Falling, true);
                    break;
                case playerMoveState.Land:
                    _animator.SetBool(Land, true);
                    break;
                case playerMoveState.Dash:
                    _animator.SetBool(Dash, true);
                    break;
                case playerMoveState.DashHit:
                    _animator.SetBool(Falling, true);
                    break;
                case playerMoveState.Damaged:
                    break;
                case playerMoveState.DoubleJump:
                    _animator.SetBool(Jump, true);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}
