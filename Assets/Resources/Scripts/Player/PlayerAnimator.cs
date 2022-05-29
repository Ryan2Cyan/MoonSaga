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
        }

        private void IdleAnimation(){
        }
        private void WalkingAnimation(){
            _animator.SetBool(Walk, true);
        }
        private void JumpAnimation(){
            _animator.SetBool(Jump, true);
        }
        private void AirControlAnimation(){
            _animator.SetBool(Falling, true);
        }
        private void LandAnimation(){
            _animator.SetBool(Land, true);
        }
        
        private void ProcessStateAnimation(){
            ResetAnimator();
            switch(_state) {
                case playerMoveState.Idle:
                    IdleAnimation();
                    break;
                case playerMoveState.Walking:
                    WalkingAnimation();
                    break;
                case playerMoveState.Jump:
                    JumpAnimation();
                    break;
                case playerMoveState.AirControl:
                    AirControlAnimation();
                    break;
                case playerMoveState.Land:
                    LandAnimation();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}
