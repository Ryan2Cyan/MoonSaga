using System;
using Resources.Scripts.General;
using UnityEngine;

namespace Resources.Scripts.Enemies.General{
    public class EnemyMovement : MonoBehaviour
    {
        private Rigidbody2D _rigidbody2D;

        // State:
        [SerializeField] private enemyMoveState _state;
        // Scripts:
        [SerializeField] private EnemyCollision _enemyColliderScript;
        [SerializeField] private GroundCheck _groundCheckScript;
        // Movement Values:
        [Range(0, 100f)] [SerializeField] private float _runSpeed = 37.5f;
        [Range(0, 30.0f)] [SerializeField] private float _damagedKnockBackX = 15f;
        [Range(0, 30.0f)] [SerializeField] private float _damagedKnockBackY = 15f;
        [SerializeField] private float _damagedKnockBackDelay = 0.1f;

        // Orientation:
        [SerializeField] internal bool _isFacingRight = true;
        
        // Knock back values:
        private float _knockBackTimer;

        private void Awake(){
            // Fetch components:
            _rigidbody2D = GetComponent<Rigidbody2D>();
            _state = enemyMoveState.Walking;
        }
        
        
        private void FixedUpdate(){
            ProcessStateMovement();
        }


        // State Functions:
        private void IdleInput(){
        }
        
        private void IdleMovement(){
        }
      
        private void WalkingMovement(){
            
            // Move enemy left or right depending on direction:
            if(_groundCheckScript._isGrounded)
                _rigidbody2D.velocity = _isFacingRight ? new Vector2(_runSpeed, 
                    _rigidbody2D.velocity.y) : new Vector2(-_runSpeed, _rigidbody2D.velocity.y);
            
        }
        private void DamagedMovement(){
            
        }

        

        private void ProcessStateMovement(){
            switch (_state){
                case enemyMoveState.Idle:
                    IdleMovement();
                    break;
                case enemyMoveState.Walking:
                    WalkingMovement();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
        

        // Input checks for switching state:
        private void SetDefaultState(){
        }
        private void IdleCheck(){
        }
        private void WalkCheck(){
            
        }
        private void DamagedCheck(){
            
        }
    }
    
    internal enum enemyMoveState{
        Idle, Walking
    }
}
