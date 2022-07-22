using System;
using Resources.Scripts.Enemies.General;
using Resources.Scripts.VFX;
using UnityEngine;

// Code within this class is responsible for collisions the player may have with objects
// or enemies, as well as updating the size of the collider based on the player's state:
namespace Resources.Scripts.Player{
    public class PlayerCollision : MonoBehaviour{

        // Scripts:
        private PlayerMovement _playerMovementScript;
        
        // Values:
        private GameObject[] _sceneEnemies;
        internal bool _enemyCollision;
        internal bool _enemyArmourCollision;
        internal bool _activeDamageCollision;
        
        // Collider values:
        private BoxCollider2D _boxCollider2D;
        private CircleCollider2D _circleCollider2D;
        [SerializeField] private Vector2 _originalColliderSize;
        [SerializeField] private Vector2 _originalColliderOffset;
        [SerializeField] private Vector2 _dashColliderSize;
        [SerializeField] private Vector2 _dashColliderOffset;
        [SerializeField] private Vector2 _dashDownColliderSize;
        [SerializeField] private Vector2 _dashDownColliderOffset;

        private void Awake(){
            
            // Fetch components:
            _boxCollider2D = GetComponent<BoxCollider2D>();
            _circleCollider2D = transform.parent.GetComponent<CircleCollider2D>();
            _playerMovementScript = transform.parent.gameObject.GetComponent<PlayerMovement>();
            
            // Ignore collision with enemy ground collider (circle colliders):
            _sceneEnemies = GameObject.FindGameObjectsWithTag("Enemy");
            if (_sceneEnemies.Length > 0){
                foreach (GameObject enemy in _sceneEnemies){
                    Physics2D.IgnoreCollision(enemy.GetComponent<CircleCollider2D>(), _circleCollider2D);
                }
            }
        }

        private void FixedUpdate(){
            
            // Change the shape of the players trigger collider depending on state:
            UpdateColliderSize();
        }

        private void OnTriggerEnter2D(Collider2D other){

            // Collide with enemy:
            if (other.gameObject.CompareTag("EnemyTrigger")){
                // Check if enemy is alive:
                if (other.transform.parent.GetComponent<EnemyData>()._isActive)
                    _enemyCollision = true;
            }
            
            if (other.gameObject.CompareTag("ActiveDamage")){
                _activeDamageCollision = true;
            }
            
            // Collision with enemy armour (causes defection):
            if (other.gameObject.CompareTag("EnemyArmour")){
                _enemyArmourCollision = true;
            }
            
            // Collide with shadow sapphire:
            if (other.gameObject.CompareTag("ShadowSapphire"))
                other.gameObject.GetComponent<ShadowSapphire>()._collided = true;
        }

        private void OnTriggerExit2D(Collider2D other){
            
            if (other.gameObject.CompareTag("EnemyTrigger"))
                _enemyCollision = false;
            
            if (other.gameObject.CompareTag("ActiveDamage")){
                _activeDamageCollision = false;
            }
            
            if (other.gameObject.CompareTag("EnemyArmour")){
                _enemyArmourCollision = false;
            }
        }

        private void UpdateColliderSize(){
            
            // Change the size of the player's collider depending on state:
            switch (_playerMovementScript._state){
                
                case playerMoveState.Idle:
                    _boxCollider2D.size = _originalColliderSize;
                    _boxCollider2D.offset = _originalColliderOffset;
                    break;
                case playerMoveState.Walking:
                    _boxCollider2D.size = _originalColliderSize;
                    _boxCollider2D.offset = _originalColliderOffset;
                    break;
                case playerMoveState.Jump:
                    _boxCollider2D.size = _originalColliderSize;
                    _boxCollider2D.offset = _originalColliderOffset;
                    break;
                case playerMoveState.DoubleJump:
                    _boxCollider2D.size = _originalColliderSize;
                    _boxCollider2D.offset = _originalColliderOffset;
                    break;
                case playerMoveState.AirControl:
                    _boxCollider2D.size = _originalColliderSize;
                    _boxCollider2D.offset = _originalColliderOffset;
                    break;
                case playerMoveState.Land:
                    _boxCollider2D.size = _originalColliderSize;
                    _boxCollider2D.offset = _originalColliderOffset;
                    break;
                case playerMoveState.Dash:
                    _boxCollider2D.size = _dashColliderSize;
                    _boxCollider2D.offset = _dashColliderOffset;
                    break;
                case playerMoveState.DashHit:
                    break;
                case playerMoveState.DashRecover:
                    _boxCollider2D.size = _originalColliderSize;
                    _boxCollider2D.offset = _originalColliderOffset;
                    break;
                case playerMoveState.Damaged:
                    break;
                case playerMoveState.DashDown:
                    _boxCollider2D.size = _dashDownColliderSize;
                    _boxCollider2D.offset = _dashDownColliderOffset;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}
