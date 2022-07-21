using System;
using Resources.Scripts.Enemies.General;
using Resources.Scripts.General;
using Resources.Scripts.Player;
using UnityEngine;

// Code within this class is a state machine, responsible
// for the actions of the "Bomber" enemy class":
namespace Resources.Scripts.Enemies.Bomber{
    public class BomberMovement : MonoBehaviour
    {
         // State:
        [SerializeField] internal enemyMoveState _state;
        
        // Scripts:
        private EnemyCollision _enemyColliderScript;
        private PlayerMovement _playerMovementScript;
        private BomberData _bomberDataScript;
        private EnemyRaycast _wallCheckScript;
        [SerializeField] private RadiusChecker _groundCheckScript;
        
        // Animator property index:
        private static readonly int State = Animator.StringToHash("State");

        private void Awake(){
            
            // Fetch components:
            _wallCheckScript = GetComponent<EnemyRaycast>();
            _bomberDataScript = GetComponent<BomberData>();
            _enemyColliderScript = _bomberDataScript._triggerCollider.GetComponent<EnemyCollision>();
            _playerMovementScript = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>();
            _groundCheckScript = GetComponent<RadiusChecker>();
            
            // Set default state:
            _state = enemyMoveState.Walking;
        }

        private void Update(){
            
            // Update state:
            ProcessStateInput();
            
            // Update animator:
            _bomberDataScript._animator.SetInteger(State, (int) _state);
        }

        private void FixedUpdate(){
            
            // Process all state movements:
            ProcessStateMovement();
        }
        
        // State Functions:
        private void WalkingInput(){

            AgroCheck();
            DamagedCheck();
            HitWallCheck();
        }
        private void WalkingMovement(){
            
            // Move enemy left or right depending on direction:
            if(_groundCheckScript._collided)
                _bomberDataScript._rigidbody2D.velocity = _bomberDataScript._isFacingRight ? 
                    new Vector2(_bomberDataScript._runSpeed, _bomberDataScript._rigidbody2D.velocity.y) : 
                    new Vector2(-_bomberDataScript._runSpeed, _bomberDataScript._rigidbody2D.velocity.y);
        }

        private void DamagedInput(){

            // Check if collision stops:
            if (!_enemyColliderScript._collidingWithPlayer)
                SetDefaultState();

            DeathCheck();
        }
        private void DamagedMovement(){
            
            // Decrement HP:
            _bomberDataScript.DecrementHp(1f);
            
            // Freeze position:
            _bomberDataScript._rigidbody2D.velocity = new Vector2(0f, 0f);
        }
        
        private void DeathInput(){

            _bomberDataScript._knockBackTimer -= Time.deltaTime;
            
            // After knock back [Inactive]:
            if (_bomberDataScript._knockBackTimer <= 0f){
                _state = enemyMoveState.Inactive;
                _bomberDataScript._rigidbody2D.velocity = new Vector2(_bomberDataScript._rigidbody2D.velocity.x, 
                    _bomberDataScript._rigidbody2D.velocity.y / 2.0f);
            }
        }
        private void DeathMovement(){
            
            // Knock back enemy based on position in relation to player:
            _bomberDataScript._rigidbody2D.velocity = _playerMovementScript.transform.position.x < transform.position.x ? 
                _bomberDataScript._knockBack : new Vector2(-_bomberDataScript._knockBack.x, _bomberDataScript._knockBack.y);
        }
        
        private void InactiveInput(){
            
            // If the enemy hits the ground, prevent them from moving, then disable the script:
            if (_groundCheckScript._collided){
                _bomberDataScript._sprite.GetComponent<SpriteRenderer>().sortingLayerName = "Far-Midground";
                _bomberDataScript._rigidbody2D.constraints = RigidbodyConstraints2D.FreezeAll;
                GetComponent<CircleCollider2D>().enabled = false;
                _groundCheckScript.enabled = false;
                enabled = false;
            }
        }
        
        private void AgroInput(){
            
            DamagedCheck();
            
            // Once timer expires [Shoot]:
            _bomberDataScript._agroTimer -= Time.deltaTime;
            if (_bomberDataScript._agroTimer <= 0f){
                _state = enemyMoveState.Shoot;
                _bomberDataScript._shootTimer = _bomberDataScript._shootTime;
            }

        }
        private void AgroMovement(){
            
            // Stop moving:
            _bomberDataScript._rigidbody2D.velocity = Vector2.zero;
        }
        
        private void ShootInput(){
            
            DamagedCheck();
            
            // Once timer expires shoot a bomb, then [Walking]:
            _bomberDataScript._shootTimer -= Time.deltaTime;
            if (_bomberDataScript._shootTimer <= 0f){
                Instantiate(UnityEngine.Resources.Load<GameObject>("Prefabs/EnemyWeapons&Items/Contact-Bomb"),
                    new Vector3(transform.position.x, transform.position.y, transform.position.z),
                    Quaternion.identity);
                _bomberDataScript._coolDownTimer = _bomberDataScript._coolDownTime;
                SetDefaultState();
            }

        }
        private void ShootMovement(){
            
            // Stop moving:
            _bomberDataScript._rigidbody2D.velocity = Vector2.zero;
        }
        
        private void IdleInput(){
            
            DamagedCheck();
            
            // Until timer expires, enemy can't attack:
            _bomberDataScript._coolDownTimer -= Time.deltaTime;
            if (_bomberDataScript._coolDownTimer <= 0f)
                _state = enemyMoveState.Walking;
        }
        private void IdleMovement(){
            
            // Stop moving:
            _bomberDataScript._rigidbody2D.velocity = Vector2.zero;
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
                case enemyMoveState.Agro:
                    AgroMovement();
                    break;
                case enemyMoveState.Shoot:
                    ShootMovement();
                    break;
                case enemyMoveState.Idle:
                    IdleMovement();
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
                case enemyMoveState.Agro:
                    AgroInput();
                    break;
                case enemyMoveState.Shoot:
                    ShootInput();
                    break;
                case enemyMoveState.Idle:
                    IdleInput();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
        

        // Input checks for switching state:
        private void SetDefaultState(){
            _state = enemyMoveState.Idle;
        }
        private void DamagedCheck(){
            
            // Check if the enemy has been hit by the player:
            if (_enemyColliderScript._collidingWithPlayer && _playerMovementScript._state == playerMoveState.DashHit)
                _state = enemyMoveState.Damaged;
        }
        private void DeathCheck(){
            
            // If HP is 0 [Death]:
            if (_bomberDataScript._hp <= 0f){
                _bomberDataScript._triggerCollider.SetActive(false);
                _bomberDataScript._isActive = false;
                _bomberDataScript._knockBackTimer = _bomberDataScript._knockBackDelay;
                _state = enemyMoveState.Death;
            }
        }
        private void HitWallCheck(){
            
            // If enemy walks into a wall - flip:
            if(_wallCheckScript._hitTarget)
                transform.localScale = UtilityFunctions.Flip(transform.localScale, 
                    ref _bomberDataScript._isFacingRight);
        }
        private void AgroCheck(){
            
            // Check if the player is in range:
            if (_bomberDataScript._playerRadiusCheckerScript._collided){
                _bomberDataScript._agroTimer = _bomberDataScript._agroTime;
                _state = enemyMoveState.Agro;
            }
        }
    }
    
    internal enum enemyMoveState{
        Walking = 0, Damaged = 1, Death = 2, Inactive = 3, Agro = 4, Shoot = 5, Idle = 6
    }
    
}
