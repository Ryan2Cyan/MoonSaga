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
        
        // Child objects:
        [SerializeField] private GameObject _sprite;
        [SerializeField] private GameObject _deathSprite;
        [SerializeField] private GameObject _triggerCollider;

        // Movement Values:
        [Range(0, 100f)] [SerializeField] private float _runSpeed = 37.5f;
        
        // Orientation:
        [SerializeField] internal bool _isFacingRight = true;
        
        // Knock back values:
        [SerializeField] private Vector2 _knockBack;
        [SerializeField] private float _knockBackDelay = 0.1f;
        private float _knockBackTimer;
        
        // Values:
        [SerializeField] internal bool _isActive = true;

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

            DeathCheck();
        }
        private void DamagedMovement(){
            
            // Decrement HP:
            _enemyDataScript.DecrementHp(1f);
            
            // Freeze position:
            _rigidbody2D.velocity = new Vector2(0f, 0f);
        }
        
        private void DeathInput(){

            _knockBackTimer -= Time.deltaTime;
            
            // After knock back [Inactive]:
            if (_knockBackTimer <= 0f){
                _state = enemyMoveState.Inactive;
                _rigidbody2D.velocity = new Vector2(_rigidbody2D.velocity.x, _rigidbody2D.velocity.y / 2.0f);
            }
        }

        private void DeathMovement(){
            
            // Knock back enemy based on position in relation to player:
            _rigidbody2D.velocity = _playerMovementScript.transform.position.x < transform.position.x ? _knockBack : 
                    new Vector2(-_knockBack.x, _knockBack.y);
        }
        
        private void InactiveInput(){
            
            // If the enemy hits the ground, prevent them from moving, then disable the script:
            if (_groundCheckScript._isGrounded){
                _rigidbody2D.constraints = RigidbodyConstraints2D.FreezeAll;
                _triggerCollider.SetActive(false);
                GetComponent<CircleCollider2D>().enabled = false;
                _groundCheckScript.enabled = false;
                enabled = false;
            }
        }


        private void ProcessStateMovement(){
            switch (_state){
                case enemyMoveState.Walking:
                    WalkingMovement();
                    break;
                case enemyMoveState.Damaged:
                    DamagedMovement();
                    break;
                case enemyMoveState.Death:
                    DeathMovement();
                    break;
                case enemyMoveState.Inactive:
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
                case enemyMoveState.Death:
                    DeathInput();
                    break;
                case enemyMoveState.Inactive:
                    InactiveInput();
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
            
            // Check if the enemy has been hit by the player:
            if (_enemyColliderScript._collidingWithPlayer && _playerMovementScript._state == playerMoveState.DashHit)
                _state = enemyMoveState.Damaged;
            
        }

        private void DeathCheck(){
            // If HP is 0 [Death]:
            if (_enemyDataScript._hp <= 0f){
                
                _state = enemyMoveState.Death;
                _isActive = false;
                
                // Swap to death sprite:
                _triggerCollider.SetActive(false);
                _sprite.SetActive(false);
                _deathSprite.SetActive(true);
                _knockBackTimer = _knockBackDelay;
            }
        }
    }
    
    internal enum enemyMoveState{
        Walking, Damaged, Death, Inactive
    }
}
