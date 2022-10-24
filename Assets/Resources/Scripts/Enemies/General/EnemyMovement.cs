using System;
using Resources.Scripts.General;
using Resources.Scripts.Player;
using UnityEngine;

namespace Resources.Scripts.Enemies.General{
    public class EnemyMovement : MonoBehaviour
    {
        // State:
        [SerializeField] internal enemyMoveState _state;
        
        // Scripts:
        private EnemyCollision _enemyColliderScript;
        private PlayerMovement _playerMovementScript;
        private RadiusChecker _groundCheckScript;
        private EnemyData _enemyDataScript;
        private EnemyRaycast _enemyRaycast;
        
        // Animator property index:
        private static readonly int State = Animator.StringToHash("State");

        private void Awake(){
            
            // Fetch components:
            _enemyRaycast = GetComponent<EnemyRaycast>();
            _enemyDataScript = GetComponent<EnemyData>();
            _enemyColliderScript = _enemyDataScript._triggerCollider.GetComponent<EnemyCollision>();
            _playerMovementScript = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>();
            _groundCheckScript = GetComponent<RadiusChecker>();
            _state = enemyMoveState.Walking;
        }

        private void Update(){
            ProcessStateInput();
            
            // Update animator:
            _enemyDataScript._animator.SetInteger(State, (int) _state);
        }

        private void FixedUpdate(){
            // Process all movements:
            ProcessStateMovement();
        }
        
        // State Functions:
        private void WalkingInput(){
            DamagedCheck();
            HitWallCheck();
        }
        private void WalkingMovement(){
            
            // Move enemy left or right depending on direction:
            if (_enemyColliderScript._collidingWithPlayer){
                _enemyColliderScript._collidingWithPlayer = false;
                _enemyDataScript._rigidbody2D.velocity = new Vector2(0f, 0f);
                return;
            }
            
            if(_groundCheckScript._collided && !_enemyColliderScript._collidingWithPlayer)
                _enemyDataScript._rigidbody2D.velocity = _enemyDataScript._isFacingRight ? 
                    new Vector2(_enemyDataScript._runSpeed, _enemyDataScript._rigidbody2D.velocity.y) : 
                    new Vector2(-_enemyDataScript._runSpeed, _enemyDataScript._rigidbody2D.velocity.y);
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
            _enemyDataScript._rigidbody2D.velocity = new Vector2(0f, 0f);
        }
        
        private void DeathInput(){

            _enemyDataScript._knockBackTimer -= Time.deltaTime;
            
            // After knock back [Inactive]:
            if (_enemyDataScript._knockBackTimer <= 0f){
                _state = enemyMoveState.Inactive;
                _enemyDataScript._rigidbody2D.velocity = new Vector2(_enemyDataScript._rigidbody2D.velocity.x, 
                    _enemyDataScript._rigidbody2D.velocity.y / 2.0f);
            }
        }
        private void DeathMovement(){
            
            // Knock back enemy based on position in relation to player:
            _enemyDataScript._rigidbody2D.velocity = _playerMovementScript.transform.position.x < transform.position.x ? 
                _enemyDataScript._knockBack : new Vector2(-_enemyDataScript._knockBack.x, _enemyDataScript._knockBack.y);
        }
        
        private void InactiveInput(){
            
            // If the enemy hits the ground, prevent them from moving, then disable the script:
            if (_groundCheckScript._collided){
                _enemyDataScript._rigidbody2D.constraints = RigidbodyConstraints2D.FreezeAll;
                _enemyDataScript._triggerCollider.SetActive(false);
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
            // if (_enemyColliderScript._collidingWithPlayer && _playerMovementScript._state == playerMoveState.DashHit)
            //     _state = enemyMoveState.Damaged;
            
        }
        private void DeathCheck(){
            // If HP is 0 [Death]:
            if (_enemyDataScript._hp <= 0f){
                
                _state = enemyMoveState.Death;
                _enemyDataScript._isActive = false;
                
                // Swap to death sprite:
                _enemyDataScript._triggerCollider.SetActive(false);
                _enemyDataScript._knockBackTimer = _enemyDataScript._knockBackDelay;
            }
        }
        private void HitWallCheck(){
            
            // If enemy walks into a wall - flip:
            if(_enemyRaycast._hitTarget)
                transform.localScale = UtilityFunctions.Flip(transform.localScale, 
                    ref _enemyDataScript._isFacingRight);
        }
    }
    
    internal enum enemyMoveState{
        Walking = 0, Damaged = 1, Death = 2, Inactive = 3
    }
}
