using System;
using Resources.Scripts.General;
using Resources.Scripts.Player;
using UnityEngine;

namespace Resources.Scripts.Enemies.General{
    public class EnemyMovement : MonoBehaviour
    {
        private Rigidbody2D _rigidbody2D;

        // State:
        [SerializeField] internal enemyMoveState _state;
        // Scripts:
        [SerializeField] private EnemyCollision _enemyColliderScript;
        [SerializeField] private GroundCheck _groundCheckScript;
        [SerializeField] private PlayerMovement _playerMovementScript;
        [SerializeField] private EnemyData _enemyDataScript;
        
        // Movement Values:
        [Range(0, 100f)] [SerializeField] private float _runSpeed = 37.5f;
        // [Range(0, 30.0f)] [SerializeField] private float _damagedKnockBackX = 15f;
        // [Range(0, 30.0f)] [SerializeField] private float _damagedKnockBackY = 15f;
        // [SerializeField] private float _damagedKnockBackDelay = 0.1f;

        // Orientation:
        [SerializeField] internal bool _isFacingRight = true;
        
        // Knock back values:
        private float _knockBackTimer;

        private void Awake(){
            // Fetch components:
            _rigidbody2D = GetComponent<Rigidbody2D>();
            _state = enemyMoveState.Walking;
        }

        private void Update(){
            ProcessStateInput();
        }

        private void FixedUpdate(){
            // Process all movements:
            ProcessStateMovement();
        }
        
        // State Functions:
        private void WalkingInput(){
            DamagedCheck();
        }
        private void WalkingMovement(){
            
            // Move enemy left or right depending on direction:
            if (_enemyColliderScript._collidingWithPlayer){
                _enemyColliderScript._collidingWithPlayer = false;
                _rigidbody2D.velocity = new Vector2(0f, 0f);
                return;
            }
            
            if(_groundCheckScript._isGrounded && !_enemyColliderScript._collidingWithPlayer)
                _rigidbody2D.velocity = _isFacingRight ? new Vector2(_runSpeed, 
                    _rigidbody2D.velocity.y) : new Vector2(-_runSpeed, _rigidbody2D.velocity.y);
        }

        private void DamagedInput(){

            // Check if collision stops:
            if (!_enemyColliderScript._collidingWithPlayer)
                SetDefaultState();
        }
        private void DamagedMovement(){
            
            // Decrement HP:
            _enemyDataScript.DecrementHp(1f);
            
            // Freeze position:
            _rigidbody2D.velocity = new Vector2(0f, 0f);
        }

        
        private void ProcessStateMovement(){
            switch (_state){
                case enemyMoveState.Walking:
                    WalkingMovement();
                    break;
                case enemyMoveState.Damaged:
                    DamagedMovement();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
        
        private void ProcessStateInput(){
            switch (_state){
                case enemyMoveState.Walking:
                    WalkingInput();
                    break;
                case enemyMoveState.Damaged:
                    DamagedInput();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
        

        // Input checks for switching state:
        private void SetDefaultState(){
            _state = enemyMoveState.Walking;
        }
        private void DamagedCheck(){
            
            // Check if the enemy has collided with the player:
            if (_enemyColliderScript._collidingWithPlayer){
                if(_playerMovementScript._state == playerMoveState.Dash ||
                   _playerMovementScript._state == playerMoveState.DashDown)
                _state = enemyMoveState.Damaged;
            }
        }
    }
    
    internal enum enemyMoveState{
        Walking, Damaged
    }
}
