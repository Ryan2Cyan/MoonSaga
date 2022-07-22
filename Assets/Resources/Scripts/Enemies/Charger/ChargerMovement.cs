using System;
using Resources.Scripts.Enemies.General;
using Resources.Scripts.General;
using Resources.Scripts.Player;
using UnityEngine;

// Code within this class is a state machine, responsible
// for the actions of the "Charger" enemy class":
namespace Resources.Scripts.Enemies.Charger{
    public class ChargerMovement : MonoBehaviour
    {
          // State:
        [SerializeField] internal enemyMoveState _state;
        
        // Scripts:
        private ChargerPFXSpawner _chargerPfxSpawnerScript;
        private EnemyCollision _enemyColliderScript;
        private PlayerMovement _playerMovementScript;
        private RadiusChecker _groundCheckScript;
        private ChargerData _chargerDataScript;
        [SerializeField] private EnemyRaycast _playerRaycastScript;
        [SerializeField] private EnemyRaycast _wallCheckScript;

        // Animator property index:
        private static readonly int State = Animator.StringToHash("State");

        private void Awake(){
            
            // Fetch components:
            _chargerPfxSpawnerScript = GetComponent<ChargerPFXSpawner>();
            _chargerDataScript = GetComponent<ChargerData>();
            _enemyColliderScript = _chargerDataScript._triggerCollider.GetComponent<EnemyCollision>();
            _playerMovementScript = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>();
            _groundCheckScript = GetComponent<RadiusChecker>();
            
            // Set default state:
            _state = enemyMoveState.Walking;
        }

        private void Update(){
            
            // Update state:
            ProcessStateInput();
            
            // Update animator:
            _chargerDataScript._animator.SetInteger(State, (int) _state);
        }

        private void FixedUpdate(){
            
            // Process all state movements:
            ProcessStateMovement();
        }
        
        // State Functions:
        private void WalkingInput(){
            
            DamagedCheck();
            ChargeCheck();
            HitWallCheck();
        }
        private void WalkingMovement(){

            // Move enemy left or right depending on direction:
            if(_groundCheckScript._collided)
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
            if (_groundCheckScript._collided){
                _chargerDataScript._sprite.GetComponent<SpriteRenderer>().sortingLayerName = "Far-Midground";
                _chargerDataScript._rigidbody2D.constraints = RigidbodyConstraints2D.FreezeAll;
                GetComponent<CircleCollider2D>().enabled = false;
                _groundCheckScript.enabled = false;
                enabled = false;
            }
        }
        
        private void PauseInput(){
            
            DamagedCheck();
            DeathCheck();
            
            // Once timer expires, check if player is still in front of enemy:
            _chargerDataScript._chargePauseTimer -= Time.deltaTime;
            if (_chargerDataScript._chargePauseTimer <= 0f){
                // Charge at player:
                if (_playerRaycastScript._hitTarget){
                    _chargerDataScript._chargeTimer = _chargerDataScript._chargeTime;
                    _state = enemyMoveState.Charge;
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
            HitWallCheck();
            
            // Once timer has ran out, check if player is still in front of charger:
            _chargerDataScript._chargeTimer -= Time.deltaTime;
            if (_chargerDataScript._chargeTimer <= 0f){
                // Continue charge [Charge]:
                if (_playerRaycastScript._hitTarget)
                    _chargerDataScript._chargeTimer = _chargerDataScript._chargeTime;
                // Stop charge [Pause]:
                else{
                    _chargerDataScript._chargePauseTimer = _chargerDataScript._chargePauseTime;
                    _state = enemyMoveState.Pause;
                }
            }
            
        }
        private void ChargeMovement(){
            
            // Charge left or right depending on direction:
            if(_groundCheckScript._collided)
                _chargerDataScript._rigidbody2D.velocity = _chargerDataScript._isFacingRight ? 
                    new Vector2(_chargerDataScript._chargeSpeed, _chargerDataScript._rigidbody2D.velocity.y) : 
                    new Vector2(-_chargerDataScript._chargeSpeed, _chargerDataScript._rigidbody2D.velocity.y);
        }
        
        private void HitWallInput(){
            
            // Once timer (knock back) expires [Stunned]:
            _chargerDataScript._knockBackTimer -= Time.deltaTime;
            if (_chargerDataScript._knockBackTimer <= 0f){
                _chargerDataScript._stunTimer = _chargerDataScript._stunTime;
                _state = enemyMoveState.Stunned;
            }
        }
        private void HitWallMovement(){
            
            // Knock back the charger:
            _chargerDataScript._rigidbody2D.velocity =
                _chargerDataScript._isFacingRight ? 
                    new Vector2(-_chargerDataScript._knockBack.x, _chargerDataScript._knockBack.y) : 
                    new Vector2(_chargerDataScript._knockBack.x, _chargerDataScript._knockBack.y);
        }
        
        private void StunnedInput(){

            // Once timer expires [Recover]
            _chargerDataScript._stunTimer -= Time.deltaTime;
            if (_chargerDataScript._stunTimer <= 0f){
                // Flip enemy and re-activate armour collider:
                transform.localScale = UtilityFunctions.Flip(transform.localScale, 
                    ref _chargerDataScript._isFacingRight);
                _chargerDataScript._armourCollider.enabled = true;
                _chargerDataScript._chargePauseTimer = _chargerDataScript._chargePauseTime;
                _state = enemyMoveState.Recover;
            }
            
            // Check if the enemy has been hit by the player:
            if (_enemyColliderScript._collidingWithPlayer && _playerMovementScript._state == playerMoveState.DashHit){
                _chargerDataScript._armourCollider.enabled = true;
                _state = enemyMoveState.Damaged;
            }
        }
        private void StunnedMovement(){
            
            // Stop moving:
            if(_groundCheckScript._collided)
                _chargerDataScript._rigidbody2D.velocity = Vector2.zero;
        }
        
        private void RecoverInput(){
            
            DamagedCheck();

            _chargerDataScript._chargePauseTimer -= Time.deltaTime;
            if (_chargerDataScript._chargePauseTimer <= 0f)
                SetDefaultState();
        }
        private void RecoverMovement(){
            
            // Stop moving:
            _chargerDataScript._rigidbody2D.velocity = Vector2.zero;
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
                    HitWallMovement();
                    break;
                case enemyMoveState.Stunned:
                    StunnedMovement();
                    break;
                case enemyMoveState.Recover:
                    RecoverMovement();
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
                    HitWallInput();
                    break;
                case enemyMoveState.Stunned:
                    StunnedInput();
                    break;
                case enemyMoveState.Recover:
                    RecoverInput();
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
                _chargerDataScript._triggerCollider.SetActive(false);
                _chargerDataScript._armourCollider.gameObject.SetActive(false);
                _chargerDataScript._isActive = false;
                _chargerDataScript._knockBackTimer = _chargerDataScript._knockBackDelay;
                _state = enemyMoveState.Death;
            }
        }
        private void ChargeCheck(){
            
            if (_playerRaycastScript._hitTarget){
                _chargerDataScript._chargePauseTimer = _chargerDataScript._chargePauseTime;
                _state = enemyMoveState.Pause;
            }
        }
        private void HitWallCheck(){
            
            switch (_wallCheckScript._hitTarget){
                // If charging [HitWall -> Stunned]:
                case true when _state == enemyMoveState.Charge:
                    _chargerDataScript._knockBackTimer = _chargerDataScript._knockBackDelay;
                    _chargerDataScript._armourCollider.enabled = false;
                    _chargerPfxSpawnerScript.SpawnArmourSparkPfx();
                    _state = enemyMoveState.HitWall;
                    break;
                // Otherwise just flip:
                case true:
                    transform.localScale = UtilityFunctions.Flip(transform.localScale, 
                        ref _chargerDataScript._isFacingRight);
                    break;
            }
        }
    }
    
    internal enum enemyMoveState{
        Walking = 0, Damaged = 1, Death = 2, Inactive = 3, Pause = 4, Charge = 5, HitWall = 6, Stunned = 7, Recover = 8
    }
}
