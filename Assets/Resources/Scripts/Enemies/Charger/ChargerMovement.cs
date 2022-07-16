using System;
using Resources.Scripts.Enemies.PillBug;
using Resources.Scripts.General;
using Resources.Scripts.Player;
using UnityEngine;

namespace Resources.Scripts.Enemies.Charger{
    public class ChargerMovement : MonoBehaviour
    {
          // State:
        [SerializeField] internal enemyMoveState _state;
        
        // Scripts:
        private EnemyCollision _enemyColliderScript;
        private PlayerMovement _playerMovementScript;
        private GroundCheck _groundCheckScript;
        private ChargerData _chargerDataScript;
        private ChargerRaycast _chargerRaycastScript;

        // Animator property index:
        private static readonly int State = Animator.StringToHash("State");

        private void Awake(){
            
            // Fetch components:
            _chargerRaycastScript = GetComponent<ChargerRaycast>();
            _chargerDataScript = GetComponent<ChargerData>();
            _enemyColliderScript = _chargerDataScript._triggerCollider.GetComponent<EnemyCollision>();
            _playerMovementScript = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>();
            _groundCheckScript = GetComponent<GroundCheck>();
            _state = enemyMoveState.Walking;
        }

        private void Update(){
            ProcessStateInput();
            
            // Update animator:
            _chargerDataScript._animator.SetInteger(State, (int) _state);
        }

        private void FixedUpdate(){
            // Process all movements:
            ProcessStateMovement();
        }
        
        // State Functions:
        private void WalkingInput(){
            
            DamagedCheck();
            ChargeCheck();
        }
        private void WalkingMovement(){
            
            // If colliding with player, stop moving:
            if (_enemyColliderScript._collidingWithPlayer){
                _enemyColliderScript._collidingWithPlayer = false;
                _chargerDataScript._rigidbody2D.velocity = new Vector2(0f, 0f);
                return;
            }
            
            // Move enemy left or right depending on direction:
            if(_groundCheckScript._isGrounded && !_enemyColliderScript._collidingWithPlayer)
                _chargerDataScript._rigidbody2D.velocity = _chargerDataScript._isFacingRight ? 
                    new Vector2(_chargerDataScript._runSpeed, _chargerDataScript._rigidbody2D.velocity.y) : 
                    new Vector2(-_chargerDataScript._runSpeed, _chargerDataScript._rigidbody2D.velocity.y);
        }

        private void DamagedInput(){

            // Check if collision stops:
            if (!_enemyColliderScript._collidingWithPlayer)
                SetDefaultState();

            DeathCheck();
        }
        private void DamagedMovement(){
            
            // Decrement HP:
            _chargerDataScript.DecrementHp(1f);
            
            // Freeze position:
            _chargerDataScript._rigidbody2D.velocity = new Vector2(0f, 0f);
        }
        
        private void DeathInput(){

            _chargerDataScript._knockBackTimer -= Time.deltaTime;
            
            // After knock back [Inactive]:
            if (_chargerDataScript._knockBackTimer <= 0f){
                _chargerDataScript._rigidbody2D.velocity = new Vector2(_chargerDataScript._rigidbody2D.velocity.x, 
                    _chargerDataScript._rigidbody2D.velocity.y / 2.0f);
                _state = enemyMoveState.Inactive;
            }
        }
        private void DeathMovement(){
            
            // Knock back enemy based on position in relation to player:
            _chargerDataScript._rigidbody2D.velocity = _playerMovementScript.transform.position.x < transform.position.x ? 
                _chargerDataScript._knockBack : new Vector2(-_chargerDataScript._knockBack.x, _chargerDataScript._knockBack.y);
        }
        
        private void InactiveInput(){
            
            // If the enemy hits the ground, prevent them from moving, then disable the script:
            if (_groundCheckScript._isGrounded){
                _chargerDataScript._rigidbody2D.constraints = RigidbodyConstraints2D.FreezeAll;
                _chargerDataScript._triggerCollider.SetActive(false);
                GetComponent<CircleCollider2D>().enabled = false;
                _groundCheckScript.enabled = false;
                enabled = false;
            }
        }
        
        private void PauseInput(){
            
            DamagedCheck();
            DeathCheck();
            
            _chargerDataScript._chargeTimer -= Time.deltaTime;
            if (_chargerDataScript._chargeTimer <= 0f){
                // Charge at player:
                if (_chargerRaycastScript._hitPlayer){
                    _state = enemyMoveState.Charge;
                    _chargerDataScript._chargeTimer = _chargerDataScript._chargeTime;
                }
                // Player is no longer there, return to walking:
                else
                    SetDefaultState();
            }
        }
        private void PauseMovement(){
            
            // Stop moving:
            _chargerDataScript._rigidbody2D.velocity = Vector2.zero;
        }
        
        private void ChargeInput(){
            
            DamagedCheck();
            DeathCheck();
            
            // Once timer has ran out, check if player is still in front of charger:
            _chargerDataScript._chargeTimer -= Time.deltaTime;
            if (_chargerDataScript._chargeTimer <= 0f){
                if (_chargerRaycastScript._hitPlayer){
                    _chargerDataScript._chargeTimer = _chargerDataScript._chargeTime;
                }
                else{
                    _chargerDataScript._chargeTimer = _chargerDataScript._chargePauseTime;
                    _state = enemyMoveState.Pause;
                }
            }
            
        }
        private void ChargeMovement(){
            
            // If colliding with player, stop moving:
            if (_enemyColliderScript._collidingWithPlayer){
                _enemyColliderScript._collidingWithPlayer = false;
                _chargerDataScript._rigidbody2D.velocity = new Vector2(0f, 0f);
                return;
            }
            
            // Charge left or right depending on direction:
            if(_groundCheckScript._isGrounded && !_enemyColliderScript._collidingWithPlayer)
                _chargerDataScript._rigidbody2D.velocity = _chargerDataScript._isFacingRight ? 
                    new Vector2(_chargerDataScript._chargeSpeed, _chargerDataScript._rigidbody2D.velocity.y) : 
                    new Vector2(-_chargerDataScript._chargeSpeed, _chargerDataScript._rigidbody2D.velocity.y);
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
                case enemyMoveState.Pause:
                    PauseMovement();
                    break;
                case enemyMoveState.Charge:
                    ChargeMovement();
                    break;
                case enemyMoveState.HitWall:
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
                case enemyMoveState.Pause:
                    PauseInput();
                    break;
                case enemyMoveState.Charge:
                    ChargeInput();
                    break;
                case enemyMoveState.HitWall:
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
            if (_chargerDataScript._hp <= 0f){
                
                _state = enemyMoveState.Death;
                _chargerDataScript._isActive = false;
                
                // Swap to death sprite:
                _chargerDataScript._triggerCollider.SetActive(false);
                _chargerDataScript._knockBackTimer = _chargerDataScript._knockBackDelay;
            }
        }
        private void ChargeCheck(){
            if (_chargerRaycastScript._hitPlayer){
                _state = enemyMoveState.Pause;
                _chargerDataScript._chargeTimer = _chargerDataScript._chargePauseTime;
            }
        }
    }
    
    internal enum enemyMoveState{
        Walking = 0, Damaged = 1, Death = 2, Inactive = 3, Pause = 4, Charge = 5, HitWall = 6
    }
}
